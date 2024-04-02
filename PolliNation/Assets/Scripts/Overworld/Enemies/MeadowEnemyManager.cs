using System;
using System.Collections.Generic;
using UnityEngine;

public class MeadowEnemyManager : MonoBehaviour
{
    public GameObject wasp; 
    [SerializeField] private int numWasps = 5;
    private List<UnityEngine.Vector3> enemyStartingPositions = new();
    private float maxSpawnDistance;
    private float minSpawnDistance;
    private bool spawnPositionAvailable = true;
    private string boundaryWallTag = "Meadow_Boundary";
    [SerializeField] private float minDistanceFromOthers = 15;

    void Awake()
    {
        // get locations of all boundarys to map out max and min spawning distances 
        float minBoundaryDistance = MapBoundaryUtilityScript.FindMinBoundaryDistance(boundaryWallTag);
        maxSpawnDistance = minBoundaryDistance * 0.8f;
        minSpawnDistance = minBoundaryDistance * 0.6f;
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
        float x, z, zSign, distance;
        UnityEngine.Vector3 generatedPosition;
        int counter = 0;

        // generate new positions until conditions are met...
        // position is not too close to other wasp spawns
        do 
        {
            distance = UnityEngine.Random.Range(minSpawnDistance , maxSpawnDistance);
            x = UnityEngine.Random.Range(-distance, distance);
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
}
