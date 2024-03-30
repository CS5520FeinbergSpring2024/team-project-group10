using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemySpawner : MonoBehaviour
{
    private float prevSpawnTime;
    //private float spawnDelay = 1;
    [SerializeField] private int maxEnemies = 20;
    public GameObject enemy; 
    private int spawnCount = 0;
    private List<UnityEngine.Vector3> enemyStartingPositions = new();
    private float xAxisLimit = 0;
    private float zAxisLimit = 0;
    private float apothem;
    private float boundaryCushion;
    [SerializeField] private float minDistanceFromOthers = 25;

    void Awake()
    {
        // get locations of all boundarys to map out max spawning distances on both axis
        GameObject[] boundaries = GameObject.FindGameObjectsWithTag("Meadow_Boundary");
        FindMapBoundaries(boundaries);
    }

    void Start()
    {
        while (SpawnCheck())
        {
            SpawnEnemy();
        }
    }


    /// <summary>
    /// Method conditions to spawn an enemy
    /// </summary>
    private bool SpawnCheck() 
    {
        // check if condition to spawn another enemy
        if (spawnCount < maxEnemies)
        {
            return true;
        } else 
        {
        return false;
        }
    }

    /// <summary>
    /// Method to spawn a new enemy 
    /// </summary>
    void SpawnEnemy()
    {  
        UnityEngine.Vector3 enemyPositon =  RandomEnemySpawnPosition();
        Instantiate(enemy, enemyPositon, UnityEngine.Quaternion.identity);
        enemyStartingPositions.Add(enemyPositon);
        // update stored spawn time
        prevSpawnTime = Time.time;
        // update enemy count
        spawnCount += 1;
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
        // (1) wasp is above minimum distance from map center
        /// and (2) position is not too close to other wasp spawns
        do{
            x = UnityEngine.Random.Range(-xAxisLimit + boundaryCushion, xAxisLimit - boundaryCushion);
            z = UnityEngine.Random.Range(-zAxisLimit + boundaryCushion, zAxisLimit- boundaryCushion);
            generatedPosition = new UnityEngine.Vector3(x,0,z);
            counter += 1;
            // break loop if not possible to find points that meet criteria
            if (counter >= 1000)
            {
                break;
            }
        }
        while ((UnityEngine.Vector3.Distance(generatedPosition, new UnityEngine.Vector3(0,0,0)) < (apothem - boundaryCushion)) 
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
                UnityEngine.Vector3 wallPos = boundary.transform.position;
        
                if (Math.Abs(wallPos.x) > xAxisLimit)
                {   
                    xAxisLimit = Math.Abs(wallPos.x) ;
                }
                if (Math.Abs(wallPos.z) > zAxisLimit)
                {   
                    zAxisLimit = Math.Abs(wallPos.z) ;
                }
            }  
            //Debug.Log("x axis limit: " + xAxisLimit);

            apothem = Mathf.Sqrt(3)/2 * xAxisLimit;
            //Debug.Log("apothem " + apothem);

            boundaryCushion = xAxisLimit * .2f;
    }
}
