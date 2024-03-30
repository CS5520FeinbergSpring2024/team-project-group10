using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private int maxEnemies = 5;
    public GameObject enemy; 
    private int spawnCount = 0;
    private List<UnityEngine.Vector3> enemyStartingPositions = new();
    private float distanceToCenter = 1000;
    private float cushion = 5;
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
        SpawnEnemy();
    }


    /// <summary>
    /// Method to spawn a new enemy 
    /// </summary>
    void SpawnEnemy()
    {   
        for (int i = 0; i < maxEnemies; i++){
            if (spawnPositionAvailable) {
                UnityEngine.Vector3 enemyPositon =  RandomEnemySpawnPosition();
                Instantiate(enemy, enemyPositon, UnityEngine.Quaternion.identity);
                enemyStartingPositions.Add(enemyPositon);
                // update enemy count
                spawnCount += 1;
            }
        }
    }

    /// <summary>
    ///  Finds a random spawn position for an enemy that is within map limits
    ///  found by FindMapBoundaries(), atleast a specified distance away from the map center,
    ///  and CheckSpawningPosition() is true. 
    /// </summary>
    private UnityEngine.Vector3 RandomEnemySpawnPosition()
    {
        // initialize loop variables
        float x,z;
        int counter = 0;
        UnityEngine.Vector3 generatedPosition;
        // generate new positions until conditions are met...
        // (1) wasp is less than 90% of way to closest wall distance
        // (2) wasp is more than 60% of the distance to the closest wall distance
        /// and (3) position is not too close to other wasp spawns
        do{
            x = UnityEngine.Random.Range(-distanceToCenter + cushion, distanceToCenter - cushion);
            z = UnityEngine.Random.Range(-distanceToCenter+ cushion, distanceToCenter- cushion);
            generatedPosition = new UnityEngine.Vector3(x,0,z);
            counter += 1;
            // break loop if not possible to find points that meet criteria
            if (counter >= 1000)
            {
                Debug.Log("Broke loop in EnemySpawner");
                spawnPositionAvailable = false;
                break;
            }
        }
        while (UnityEngine.Vector3.Distance(generatedPosition, new UnityEngine.Vector3(0,0,0)) > (distanceToCenter * 0.9)
        || UnityEngine.Vector3.Distance(generatedPosition, new UnityEngine.Vector3(0,0,0)) < (distanceToCenter * 0.6)
        || !CheckSpawningPosition(generatedPosition));
        return generatedPosition;
    }

    // method to check that enemies are spawning atleast a minimum distance apart
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

    // method to get bounds from boundary gameobjects surrounding hexagon shaped map
    private void FindMapBoundaries(GameObject[] boundaries) {

        foreach (GameObject boundary in boundaries) {
                // get position of wall
                UnityEngine.Vector3 wallPos = boundary.transform.position;

                // get distance of boundary from center
                float boundaryDistance = UnityEngine.Vector3.Distance(new UnityEngine.Vector3(0,0,0), boundary.transform.position);
                if (boundaryDistance < distanceToCenter) {
                    distanceToCenter = boundaryDistance;
                }
        }     
    }
}
