using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Instantiates PollenProvider objects in the scene. Manages their placement
/// and values.
/// </summary>
public class MeadowResourceProviderManager : MonoBehaviour
{
    // Fields to in editor.

    [SerializeField]
    private int _numPollenProviders = 24;
    [SerializeField]
    private GameObject _pollenProviderPrefab;

    // Other fields.
    private List<GameObject> _pollenProviders = new();

    // For polar locations. Generated locations fall in
    // the ring between these two radii.
    private string _boundaryWallTag = "Meadow_Boundary";
    // Set radius in case boundaryTag doesn't exist in world.
    private float _locationOuterRadius = 50;
    private readonly int _locationInnerRadius = 15;
    [SerializeField]
    private float _minDistanceApart = 3;

    // For flower rotations.
    private readonly int _xRotationDegrees = 10;
    private readonly int _zRotationDegrees = 10;

    // For generating production constraints.
    /* A future version could tie these values to the player's progress,
     * e.g. by multiplying regeneration time by a factor of the player's 
     * inventory count.
     */
    private readonly int _secondsToCollectTotalMin = 0;
    private readonly int _secondsToCollectTotalMax = 7;
    private readonly int _totalCollectableAmountMin = 10;
    private readonly int _totalCollectableAmountMax = 45;
    private readonly int _regenerationTimeSecondsMin = 5;
    private readonly int _regenerationTimeSecondsMax = 15;

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
                                                            int maxIterations=1000)
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
    private void SetRandomProductionValues(FlowerResourceProvider provider)
    {
        if (provider != null)
        {
            int secondsToCollectTotal = UnityEngine.Random.Range(
                        _secondsToCollectTotalMin, _secondsToCollectTotalMax + 1);
            int totalCollectableAmount = UnityEngine.Random.Range(
                _totalCollectableAmountMin, _totalCollectableAmountMax + 1);
            int regenerationTimeSeconds = UnityEngine.Random.Range(
                _regenerationTimeSecondsMin, _regenerationTimeSecondsMax + 1);
            provider.SetValues(provider.ResourceType, totalCollectableAmount,
                                     secondsToCollectTotal, regenerationTimeSeconds);
        }
    }

    /// <summary>
    /// Spawn <c>count</c> number of the given <c>prefab</c> at random locations.
    /// </summary>
    /// <param name="prefab">The prefab to spawn objects from.</param>
    /// <param name="count">The number of objects to spawn.</param>
    private void SpawnObjects(GameObject prefab, int count = 1)
    {
        try
        {
            for (int i = 0; i < count; i++)
            {
                Vector3 location = GenerateRandomLocationWithValidDistance(
                    _pollenProviders, _minDistanceApart);
                if (!Vector3.positiveInfinity.Equals(location))
                {
                    Debug.Log("valid");
                    GameObject instance = Instantiate(prefab,
                                                  GenerateRandomRingLocation(),
                                                  GenerateRandomRotation());
                    FlowerResourceProvider providerScript =
                            instance.GetComponent<FlowerResourceProvider>();
                    SetRandomProductionValues(providerScript);
                    _pollenProviders.Add(instance);
                }
                else {
                    Debug.Log("invalid");
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

    void Awake()
    {
        _locationOuterRadius = MapBoundaryUtilityScript.FindMinBoundaryDistance(_boundaryWallTag);
    }

    void Start()
    {
        SpawnObjects(_pollenProviderPrefab, _numPollenProviders);
    }
}
