using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject wasp; 
    [SerializeField] private int numWasps = 5;
    private List<UnityEngine.Vector3> enemyStartingPositions = new();
    private UnityEngine.Vector3 mapCenter = new(0,0,0);
    private float maxSpawnDistance;
    private float minSpawnDistance;
    private bool spawnPositionAvailable = true;
    [SerializeField] private float minDistanceFromOthers = 15;

    void Awake()
    {
        // get locations of all boundarys to map out max spawning distances on both axis
        GameObject[] boundaries = GameObject.FindGameObjectsWithTag("Meadow_Boundary");
        FindMapBoundaries(boundaries);
    }

    void Start()
    {
        SpawnEnemies(wasp, numWasps);
    }


    /// <summary>
    /// Spawns <c>numEnemies</c> number of given <c>enemyPrefab</c> at random locations.
    /// </summary>
    /// <param name="enemyPrefab">The enemy prefab to spawn objects from.</param>
    /// <param name="numEnemies">The number of enemies to spawn.</param>
    void SpawnEnemies(GameObject enemyPrefab, int numEnemies = 1)
    {   
        for (int i = 0; i < numEnemies; i++)
        {
            if (spawnPositionAvailable) 
            {
                UnityEngine.Vector3 enemyPositon =  RandomEnemySpawnPosition();
                Instantiate(enemyPrefab, enemyPositon, UnityEngine.Quaternion.identity);
                enemyStartingPositions.Add(enemyPositon);
            }
        }

    }

    /// <summary>
    ///  Finds a random spawn position for an enemy that is within map limits,
    ///  atleast a specified distance away from the map center, and atleast the
    ///  set distance away from other spawned enemeies. 
    /// </summary>
    /// <returns> randomly generated position </resturns>
    private UnityEngine.Vector3 RandomEnemySpawnPosition()
    {
        // initialize loop variables
        float x, z, xSign, zSign, distance;
        UnityEngine.Vector3 generatedPosition;
        int counter = 0;

        // generate new positions until conditions are met...
        // position is not too close to other wasp spawns
        do 
        {
            distance = UnityEngine.Random.Range(minSpawnDistance , maxSpawnDistance);
            x = UnityEngine.Random.Range(-distance, distance);
            xSign = UnityEngine.Random.Range(0,2)*2 -1;
            x *= xSign;
            z = (float) Math.Sqrt(Math.Pow(distance,2) - Math.Pow(x,2));
            zSign = UnityEngine.Random.Range(0,2)*2 -1;
            z *= zSign;
            generatedPosition = new UnityEngine.Vector3(x,0,z);
            counter += 1;
            // break loop if can't find position far enough away from other spawned enemies
            if (counter >= 1000)
            {
                Debug.Log("No spawn position found that meets minDistance from other enemies criteria. Stopped spawning.");
                spawnPositionAvailable = false;
                break;
            }
        } 
        while(!CheckSpawningPosition(generatedPosition));
        return generatedPosition;
    }

    /// <summary>
    /// Check if position is atleast the set minimum distance apart from all other spawned enemies.
    /// </summary>
    /// <returns>true if position is minimum distance from all other spawned enemies, otherwise false.</returns>
    private Boolean CheckSpawningPosition(UnityEngine.Vector3 generatedPosition)
    {   
        if (enemyStartingPositions != null) {   
            foreach (UnityEngine.Vector3 position in enemyStartingPositions) {
                float distance = UnityEngine.Vector3.Distance(generatedPosition, position);
                if (distance <= minDistanceFromOthers)
                {   
                    return false;
                }
            }   
        }
        return true;
    }

    /// <summary>
    /// method to get bounds from boundary gameobjects surrounding map
    /// </summary>
    private void FindMapBoundaries(GameObject[] boundaries) {
        float distanceToCenter = float.MaxValue;

        foreach (GameObject boundary in boundaries) 
        {
                // get position of wall
                UnityEngine.Vector3 wallPos = boundary.transform.position;
                // get distance of boundary from center
                float boundaryDistance = UnityEngine.Vector3.Distance(mapCenter, boundary.transform.position);
                if (boundaryDistance < distanceToCenter) 
                {
                    distanceToCenter = boundaryDistance;
                }
        }
        // set max and min distance as 80% and 60% of distance from center to put on outer ring of map    
        maxSpawnDistance = distanceToCenter * 0.8f;
        minSpawnDistance = distanceToCenter * 0.6f; 
    }
}
