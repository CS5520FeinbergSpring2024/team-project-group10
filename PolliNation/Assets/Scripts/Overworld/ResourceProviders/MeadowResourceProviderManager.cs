using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Instantiates PollenProvider and NectarProvider objects in the scene. 
/// Manages their placement and values.
/// </summary>
public class MeadowResourceProviderManager : MonoBehaviour, IResourceAmountToEmissionRateConverter
{
    // Fields to in editor.
    // PollenProvider
    [SerializeField]
    private int _numPollenProviders = 24;
    [SerializeField]
    private GameObject _pollenProviderPrefab;
    // Min distance between flowers of any type.
    [SerializeField]
    private float _minDistanceApart = 6;
    // NectarProvider
    [SerializeField]
    private int _numNectarProviders = 16;
    [SerializeField]
    private GameObject _nectarProviderPrefab;

    // Other fields.
    private List<GameObject> _pollenProviders = new();
    private List<GameObject> _nectarProviders = new();
    private List<GameObject> _allProviders = new();

    // For polar locations. Generated locations fall in
    // the ring between these two radii.
    private string _boundaryWallTag = "Meadow_Boundary";
    // Set radius in case boundaryTag doesn't exist in world.
    private float _locationOuterRadius = 50;
    private readonly int _locationInnerRadius = 15;

    // For flower rotations.
    private readonly int _xRotationDegrees = 10;
    private readonly int _zRotationDegrees = 10;

    // Storing settings in distionaries easier functions.
    private readonly Type _pollenKey = typeof(PollenProvider);
    private readonly Type _nectarKey = typeof(NectarProvider);
    private readonly List<Type> _acceptedKeys = new();

    // For generating production constraints.
    /* A future version could tie these values to the player's progress,
     * e.g. by multiplying regeneration time by a factor of the player's 
     * inventory count.
     */
    // Pollen
    private readonly int _pollenSecondsToCollectTotalMin = 0;
    private readonly int _pollenSecondsToCollectTotalMax = 7;
    private readonly int _pollenTotalCollectableAmountMin = 10;
    private readonly int _pollenTotalCollectableAmountMax = 45;
    private readonly int _pollenRegenerationTimeSecondsMin = 5;
    private readonly int _pollenRegenerationTimeSecondsMax = 15;

    // Nectar
    // Keeping these as separate variables instead of placing them in a map
    // so they can be serialized if needed.
    private readonly int _nectarSecondsToCollectTotalMin = 0;
    private readonly int _nectarSecondsToCollectTotalMax = 7;
    private readonly int _nectarTotalCollectableAmountMin = 10;
    private readonly int _nectarTotalCollectableAmountMax = 45;
    private readonly int _nectarRegenerationTimeSecondsMin = 5;
    private readonly int _nectarRegenerationTimeSecondsMax = 15;

    // Settings dictionaries for easier programming.
    // Shared
    private readonly Dictionary<Type, int> _secondsToCollectTotalMin = new();
    private readonly Dictionary<Type, int> _secondsToCollectTotalMax = new();
    private readonly Dictionary<Type, int> _totalCollectableAmountMin = new();
    private readonly Dictionary<Type, int> _totalCollectableAmountMax = new();
    private readonly Dictionary<Type, int> _regenerationTimeSecondsMin = new();
    private readonly Dictionary<Type, int> _regenerationTimeSecondsMax = new();

    // For visual representation of flowers' capacity.
    // These may need to be adjusted to obtain good visual distinction.
    private readonly int _pollenParticleEmissionRateMin = 0;
    private readonly int _pollenParticleEmissionRateMax = 25;
    private readonly int _nectarParticleEmissionRateMin = 0;
    private readonly int _nectarParticleEmissionRateMax = 25;

    private readonly Dictionary<Type, int> _particleEmissionRateMin = new();
    private readonly Dictionary<Type, int> _particleEmissionRateMax = new();
    private readonly Dictionary<Type, int> _amountToEmissionConversionFactor = new();

    /// <summary>
    /// Generates a random Vector3 location on the plane within the 
    /// ring between _locationInnterRadius and _locationOuterRadius.
    /// </summary>
    /// <returns>The generated location.</returns>
    private Vector3 GenerateRandomRingLocation()
    {
        float distance = UnityEngine.Random.Range(_locationInnerRadius,
                                                  _locationOuterRadius);
        float angle = (float)(UnityEngine.Random.Range(0, 360) * (Math.PI / 180));
        float x = (float)Math.Cos(angle) * distance;
        float z = (float)Math.Sin(angle) * distance;
        float y = 0;  // All on the same vertical plane.
        return new Vector3(x, y, z);
    }

    /// <summary>
    /// Attempts to generate a random location that is at least minDistance away from all of the
    /// GameObjects in others. If one cannot be found within maxIterations, returns
    /// Vector3.positiveInfinity.
    /// </summary>
    /// <param name="others">The game objects to stay minDistance away from.</param>
    /// <param name="minDistance">The minimum distance between this location and those of the
    /// objects in others.</param>
    /// <param name="maxIterations">The number of times to try finding a valid location.</param>
    /// <returns>The generated valid location or Vector3.positiveInfinity if no valid location
    /// could be found in maxIterations attempts.</returns>
    private Vector3 GenerateRandomLocationWithValidDistance(List<GameObject> others,
                                                            float minDistance,
                                                            int maxIterations = 1000)
    {
        if (others == null || minDistance <= 0)
        {
            return GenerateRandomRingLocation();
        }
        Vector3 location = GenerateRandomRingLocation();
        for (int i = 0; i < maxIterations; i++)
        {
            bool valid = true;
            foreach (GameObject existingObj in others)
            {
                if (Vector3.Distance(location, existingObj.transform.position) < minDistance)
                {
                    valid = false;
                }
            }
            if (valid)
            {
                return location;
            }
            location = GenerateRandomRingLocation();
        }
        Debug.Log("[MeadowResourceProviderManager]"
                  + " Could not find location at least " + minDistance + " away from others.");
        return Vector3.positiveInfinity;
    }

    /// <summary>
    /// Generates a Quaternion rotation with random x and z rotations within
    /// +- _xRotationDegrees and +- _zRotationDegrees, respectively, and a y-rotation
    /// between 0 and 360.
    /// </summary>
    /// <returns></returns>
    private Quaternion GenerateRandomRotation()
    {
        float x = UnityEngine.Random.Range(-_xRotationDegrees, _xRotationDegrees);
        float z = UnityEngine.Random.Range(-_zRotationDegrees, _zRotationDegrees);
        float y = UnityEngine.Random.Range(0, 360);
        return Quaternion.Euler(x, y, z);
    }

    /// <summary>
    /// Sets the given PollenProFlowerResourceProvidervider's secondsToCollectTotal, 
    /// totalCollectableAmount, and regenerationTimeSeconds to random values.
    /// </summary>
    /// <param name="FlowerResourceProvider">The FlowerResourceProvider for which to set
    /// values.</param>
    /// <param name="settingsKey">The key to use to look up production value settings.</param>
    private void SetRandomProductionValues(FlowerResourceProvider provider, Type settingsKey)
    {
        if (provider != null)
        {
            int secondsToCollectTotal = UnityEngine.Random.Range(
                        _secondsToCollectTotalMin[settingsKey], _secondsToCollectTotalMax[settingsKey] + 1);
            int totalCollectableAmount = UnityEngine.Random.Range(
                _totalCollectableAmountMin[settingsKey], _totalCollectableAmountMax[settingsKey] + 1);
            int regenerationTimeSeconds = UnityEngine.Random.Range(
                _regenerationTimeSecondsMin[settingsKey], _regenerationTimeSecondsMax[settingsKey] + 1);
            provider.SetValues(provider.ResourceType, totalCollectableAmount,
                                     secondsToCollectTotal, regenerationTimeSeconds);
        }
    }

    /// <summary>
    /// Spawn up to <c>count</c> number of the given <c>prefab</c> at random locations.
    /// </summary>
    /// <param name="prefab">The prefab to spawn objects from.</param>
    /// <param name="instanceList">The list in which to store spawned instances.</param>
    /// <param name="locationConflicts">Other GameObjects that this should be spawned _minDistanceApart from.</param>
    /// <param name="settingsKey">The key to use to look up production value settings.</param>
    /// <param name="count">The number of objects to spawn.</param>
    private void SpawnObjects(GameObject prefab, List<GameObject> instanceList, 
                              List<GameObject> locationConflicts, Type settingsKey, int count = 1)
    {
        try
        {
            for (int i = 0; i < count; i++)
            {
                Vector3 location = GenerateRandomLocationWithValidDistance(
                    locationConflicts, _minDistanceApart);
                if (!Vector3.positiveInfinity.Equals(location))
                {
                    GameObject instance = Instantiate(prefab,
                                                  location,
                                                  GenerateRandomRotation());
                    FlowerResourceProvider providerScript =
                            instance.GetComponent<FlowerResourceProvider>();
                    SetRandomProductionValues(providerScript, settingsKey);
                    providerScript.EmissionRateConverter = this;
                    instanceList.Add(instance);
                    locationConflicts.Add(instance);
                }
            }
        }
        catch (UnassignedReferenceException e)
        {
            Debug.LogError(e);
        }
        catch (ArgumentException e)
        {
            Debug.LogError(e);
        }
    }

    /// <summary>
    /// Convert a value on the totalCollectableAmount scale to its equivalent on the
    /// particleEmissionRate scale.
    /// </summary>
    /// <param name="amount">Amount of the resource available to collect.</param>
    /// <param name="settingsKey">The key to use to look up production value settings. 
    /// Typically the type of the object calling this method.</param>
    /// <returns>The equivalent of the given amount on the particle emmision rate scale or -1 if the
    /// given settingsKey is not recognized.</returns>
    public float EmissionRateFromResourceAmount(float amount, Type settingsKey)
    {
        if (settingsKey == null || !_acceptedKeys.Contains(settingsKey)) {
            Debug.Log(string.Format("[{0}: {0}] Error: settingsKey {0} not recognized.", 
                      GetType(), "EmissionRateFromResourceAmount", settingsKey));
            return -1;
        }
        if (amount <= _totalCollectableAmountMin[settingsKey])
        {
            return _particleEmissionRateMin[settingsKey];
        }
        if (amount > _totalCollectableAmountMax[settingsKey])
        {
            return _particleEmissionRateMax[settingsKey];
        }
        return (amount - _totalCollectableAmountMin[settingsKey]) * _amountToEmissionConversionFactor[settingsKey]
                    + _particleEmissionRateMin[settingsKey];
    }

    void Awake()
    {
        _locationOuterRadius = MapBoundaryUtilityScript.FindMinBoundaryDistance(_boundaryWallTag);

        // Recognized dictionary keys.
        _acceptedKeys.Add(_pollenKey);
        _acceptedKeys.Add(_nectarKey);

        // Add individual settings to dictionaries.
        _secondsToCollectTotalMin.Add(_pollenKey, _pollenSecondsToCollectTotalMin);
        _secondsToCollectTotalMax.Add(_pollenKey, _pollenSecondsToCollectTotalMax);
        _totalCollectableAmountMin.Add(_pollenKey, _pollenTotalCollectableAmountMin);
        _totalCollectableAmountMax.Add(_pollenKey, _pollenTotalCollectableAmountMax);
        _regenerationTimeSecondsMin.Add(_pollenKey, _pollenRegenerationTimeSecondsMin);
        _regenerationTimeSecondsMax.Add(_pollenKey, _pollenRegenerationTimeSecondsMax);
        _particleEmissionRateMin.Add(_pollenKey, _pollenParticleEmissionRateMin);
        _particleEmissionRateMax.Add(_pollenKey, _pollenParticleEmissionRateMax);
        _amountToEmissionConversionFactor.Add(_pollenKey, 0);

        _secondsToCollectTotalMin.Add(_nectarKey, _nectarSecondsToCollectTotalMin);
        _secondsToCollectTotalMax.Add(_nectarKey, _nectarSecondsToCollectTotalMax);
        _totalCollectableAmountMin.Add(_nectarKey, _nectarTotalCollectableAmountMin);
        _totalCollectableAmountMax.Add(_nectarKey, _nectarTotalCollectableAmountMax);
        _regenerationTimeSecondsMin.Add(_nectarKey, _nectarRegenerationTimeSecondsMin);
        _regenerationTimeSecondsMax.Add(_nectarKey, _nectarRegenerationTimeSecondsMax);
        _particleEmissionRateMin.Add(_nectarKey, _nectarParticleEmissionRateMin);
        _particleEmissionRateMax.Add(_nectarKey, _nectarParticleEmissionRateMax);
        _amountToEmissionConversionFactor.Add(_nectarKey, 0);
    }

    void Start()
    {
        // Set up emission rate conversion.
        _amountToEmissionConversionFactor[_pollenKey] =
                (_totalCollectableAmountMax[_pollenKey] - _totalCollectableAmountMin[_pollenKey])
                / (_particleEmissionRateMax[_pollenKey] - _particleEmissionRateMin[_pollenKey]);
        _amountToEmissionConversionFactor[_nectarKey] =
                (_totalCollectableAmountMax[_nectarKey] - _totalCollectableAmountMin[_nectarKey])
                / (_particleEmissionRateMax[_nectarKey] - _particleEmissionRateMin[_nectarKey]);

        // Spawn objects.
        SpawnObjects(_pollenProviderPrefab, _pollenProviders, _allProviders, _pollenKey, _numPollenProviders);
        SpawnObjects(_nectarProviderPrefab, _nectarProviders, _allProviders, _nectarKey, _numNectarProviders);
    }
}
