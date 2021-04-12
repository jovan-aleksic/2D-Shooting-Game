using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public List<Spawner> enemySpawners = new List<Spawner>();

    public List<Spawner> powerUpSpawners = new List<Spawner>();

    public Transform enemyContainer;
    public Transform powerUpContainer;

    private void Start()
    {
        if (enemyContainer == null && enemySpawners.Count > 0)
        {
            Debug.LogError("There is no container to spawn Enemies in. Please Add one in the Inspector!", this);
        }

        foreach (Spawner enemySpawner in enemySpawners)
        {
            enemySpawner.InitSpawner();
        }

        if (powerUpContainer == null && powerUpSpawners.Count > 0)
        {
            Debug.LogError("There is no container to spawn Power Ups in. Please Add one in the Inspector!", this);
        }

        foreach (Spawner powerUpSpawner in powerUpSpawners)
        {
            powerUpSpawner.InitSpawner();
        }
    }

    public void StartSpawning()
    {
        foreach (Spawner enemySpawner in enemySpawners)
        {
            enemySpawner.spawnRoutine = StartCoroutine(enemySpawner.SpawnRoutine(enemyContainer));
        }

        foreach (Spawner powerUpSpawner in powerUpSpawners)
        {
            powerUpSpawner.spawnRoutine = StartCoroutine(powerUpSpawner.SpawnRoutine(powerUpContainer));
        }
    }
}