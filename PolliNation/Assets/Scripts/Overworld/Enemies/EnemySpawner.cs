using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private float prevSpawnTime;
    private float spawnDelay = 5;
    private int maxEnemies = 3;
    private float rangeLimit = 25;
    public GameObject enemy; 
    private float minDistance = 20;

    private int spawnCount = 0;

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
        Instantiate(enemy, RandomEnemySpawnPosition(), Quaternion.identity);

        // update stored spawn time
        prevSpawnTime = Time.time;
        // update enemy count
        spawnCount += 1;
    }

    private Vector3 RandomEnemySpawnPosition()
    {
        // get random x and z values within specified range
        float x = UnityEngine.Random.Range(-rangeLimit, rangeLimit);
        float z = UnityEngine.Random.Range(-rangeLimit, rangeLimit);
        // keep same height as origin
        var y = 0;
        // generate new random numbers until one of the axis is greater than min distance
        while (Math.Abs(x) < minDistance || Math.Abs(z) < minDistance) {
            x = UnityEngine.Random.Range(-rangeLimit, rangeLimit);
            z = UnityEngine.Random.Range(-rangeLimit, rangeLimit);
        }
        return new Vector3(x,y,z);
    }
}
