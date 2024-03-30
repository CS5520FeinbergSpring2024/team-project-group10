using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeadowResourceProviderManager : MonoBehaviour
{
    [SerializeField]
    private int _numPollenProviders = 24;
    [SerializeField]
    private GameObject _pollenProviderPrefab;
    // For polar locations. Generated locations fall in
    // the ring between these two radii.
    private readonly int _locationOuterRadius = 50;
    private readonly int _locationInnerRadius = 15;

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
                Instantiate(_pollenProviderPrefab,
                            GenerateRandomRingLocation(),
                            Quaternion.identity);
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


    // Start is called before the first frame update
    void Start()
    {
        SpawnObjects(_pollenProviderPrefab, _numPollenProviders);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
