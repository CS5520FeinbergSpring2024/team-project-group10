using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemySpawner : MonoBehaviour
{
    private float prevSpawnTime;
    private float spawnDelay = 1;
    private int maxEnemies = 3;
    public GameObject enemy; 
    private int spawnCount = 0;
    private List<UnityEngine.Vector3> enemyStartingPositions = new();

    private bool SpawnCheck() 
    {
        // check if conditions to spawn another enemy
        if ((Time.time > prevSpawnTime + spawnDelay) && (spawnCount < maxEnemies))
        {
            return true;
        } else 
        {
        return false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SpawnCheck() == true)
        {
            SpawnEnemy();
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
        float x,z,x_sign,z_sign;
        UnityEngine.Vector3 generatedPosition;
        float rangeLowerLimit = 20;
        float rangeUpperLimit = 30;
        float minSpawnDistance = 25;
        // generate new positions until conditions are met...
        // (1) on axis is above minimum distacne and (2) position is not too close to other spawns
        do{
            x = UnityEngine.Random.Range(rangeLowerLimit, rangeUpperLimit);
            x_sign = UnityEngine.Random.value < 0.5f ? -1f : 1f;
            x *= x_sign;
            z = UnityEngine.Random.Range(rangeLowerLimit, rangeUpperLimit);
            z_sign = UnityEngine.Random.value < 0.5f ? -1f : 1f;
            z *= z_sign;
            generatedPosition = new UnityEngine.Vector3(x,0,z);
        }
        while ((UnityEngine.Vector3.Distance(generatedPosition, new UnityEngine.Vector3(0,0,0)) < minSpawnDistance) 
        || !CheckSpawningPosition(generatedPosition));
        
        return generatedPosition;
    }

    // method to check that enemies are spawning atleast a minimum distance apart
    private Boolean CheckSpawningPosition(UnityEngine.Vector3 generatedPosition)
    {   float minDistanceFromOthers = 20;
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
