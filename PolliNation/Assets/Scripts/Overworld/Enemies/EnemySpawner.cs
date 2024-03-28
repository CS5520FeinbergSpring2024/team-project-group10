using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemySpawner : MonoBehaviour
{
    private float prevSpawnTime;
    //private float spawnDelay = 1;
    [SerializeField] private int maxEnemies = 3;
    public GameObject enemy; 
    private int spawnCount = 0;
    float spawnRange = 27;
    float minSpawnDistance = 25;
    private List<UnityEngine.Vector3> enemyStartingPositions = new();

    void Start()
    {
        while (SpawnCheck())
        {
            SpawnEnemy();
        }
    }

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

    private UnityEngine.Vector3 RandomEnemySpawnPosition()
    {
        // initialize loop variables
        float x,z;
        UnityEngine.Vector3 generatedPosition;
        // generate new positions until conditions are met...
        // (1) wasp is above minimum distance and (2) position is not too close to other wasp spawns
        do{
            x = UnityEngine.Random.Range(-spawnRange, spawnRange);
            z = UnityEngine.Random.Range(-spawnRange, spawnRange);
            generatedPosition = new UnityEngine.Vector3(x,0,z);
        }
        while ((UnityEngine.Vector3.Distance(generatedPosition, new UnityEngine.Vector3(0,0,0)) < minSpawnDistance) 
        || !CheckSpawningPosition(generatedPosition));
        
        return generatedPosition;
    }

    // method to check that enemies are spawning atleast a minimum distance apart
    private Boolean CheckSpawningPosition(UnityEngine.Vector3 generatedPosition)
    {   float minDistanceFromOthers = 10;
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
