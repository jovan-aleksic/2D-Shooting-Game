using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public List<Spawner> spawners = new List<Spawner>();

    public List<Transform> gameObjectContainers = new List<Transform>();

    private void Start()
    {
        if (gameObjectContainers.Count < spawners.Count)
        {
            Debug.LogError("There are more spawners then there are containers." +
                           "Please ensure that the is a container for each spawner");
        }

        foreach (Spawner spawner in spawners)
        {
            spawner.InitSpawner();
        }
    }

    public void StartSpawning()
    {
        int i = 0;
        foreach (Spawner spawner in spawners)
        {
            spawner.spawnRoutine = StartCoroutine(spawner.SpawnRoutine(gameObjectContainers[i]));
            i++;
        }
    }
}