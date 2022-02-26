using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    private float nextSpawnTime;

    public int playerLevel;

    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private float spawnDelay = 10;



   private void Update()
    {
        if(ShouldSpawn())
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        nextSpawnTime = Time.time + spawnDelay;
        Instantiate(zombiePrefab, transform.position, transform.rotation);
    }

    private bool ShouldSpawn()
    {

        return Time.time > nextSpawnTime && LevelManager.instance.currentLevel == playerLevel;
    }    

}
