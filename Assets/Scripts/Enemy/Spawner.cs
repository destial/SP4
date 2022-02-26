using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    private float nextSpawnTime;

    public int playerLevel;

    [SerializeField] private GameObject zombiePrefab;
    [SerializeField] private float spawnDelay = 10;

    private List<GameObject> zombies = new List<GameObject>();

   private void Update()
    {
        if(ShouldSpawn())
        {
            Spawn();
        }
        if (LevelManager.instance.currentLevel != playerLevel) {
            foreach (GameObject zombie in zombies) {
                if (zombie != null) Destroy(zombie);
            }
        }
    }

    private void Spawn()
    {
        nextSpawnTime = Time.time + spawnDelay;
        GameObject zombie = Instantiate(zombiePrefab, transform.position, transform.rotation);
        zombies.Add(zombie);
    }

    private bool ShouldSpawn()
    {

        return Time.time > nextSpawnTime && LevelManager.instance.currentLevel == playerLevel;
    }    

}
