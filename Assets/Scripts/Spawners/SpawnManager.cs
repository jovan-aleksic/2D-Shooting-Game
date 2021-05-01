using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private CodedGameEventListener waveSpawnerCompetedEvent;
    [SerializeField] private CodedGameEventListener playerDestroyedEvent;
    [SerializeField] private bool usePlayerDestroyedEvent;
    [SerializeField] private List<ScriptableObject> enemySpawners = new List<ScriptableObject>();
    [SerializeField] private List<ScriptableObject> powerUpSpawners = new List<ScriptableObject>();

    [SerializeField] private Transform enemyContainer;
    [SerializeField] private Transform powerUpContainer;

    private void OnValidate()
    {
        for (int i = 0; i < enemySpawners.Count; i++)
        {
            if (enemySpawners[i] == null) continue;
            if (!(enemySpawners[i] is ISpawner))
            {
                enemySpawners[i] = null;
            }
        }

        for (int i = 0; i < powerUpSpawners.Count; i++)
        {
            if (powerUpSpawners[i] == null) continue;
            if (!(powerUpSpawners[i] is ISpawner))
            {
                powerUpSpawners[i] = null;
            }
        }
    }

    private void Awake()
    {
        enemySpawners.RemoveAll(item => !(item is ISpawner));
        powerUpSpawners.RemoveAll(item => !(item is ISpawner));
    }

    private void OnDisable()
    {
        waveSpawnerCompetedEvent.OnDisable();
        if (usePlayerDestroyedEvent) playerDestroyedEvent.OnDisable();
    }

    private void OnEnable()
    {
        waveSpawnerCompetedEvent.OnEnable(StopSpawning);
        if (usePlayerDestroyedEvent) playerDestroyedEvent?.OnEnable(StopSpawning);
    }

    private void Start()
    {
        if (enemyContainer == null && enemySpawners.Count > 0)
        {
            Debug.LogError("There is no container to spawn Enemies in. Please Add one in the Inspector!", this);
        }

        foreach (ISpawner enemySpawner in enemySpawners.Cast<ISpawner>())
        {
            enemySpawner.Init(enemyContainer, this);
        }

        if (powerUpContainer == null && powerUpSpawners.Count > 0)
        {
            Debug.LogError("There is no container to spawn Power Ups in. Please Add one in the Inspector!", this);
        }

        foreach (ISpawner powerUpSpawner in powerUpSpawners.Cast<ISpawner>())
        {
            powerUpSpawner.Init(powerUpContainer, this);
        }
    }

    public void StartSpawning()
    {
        foreach (ISpawner enemySpawner in enemySpawners.Cast<ISpawner>())
        {
            enemySpawner.Start();
        }

        foreach (ISpawner powerUpSpawner in powerUpSpawners.Cast<ISpawner>())
        {
            powerUpSpawner.Start();
        }
    }

    private void StopSpawning()
    {
        foreach (ISpawner enemySpawner in enemySpawners.Cast<ISpawner>())
        {
            enemySpawner.Stop();
        }

        foreach (ISpawner powerUpSpawner in powerUpSpawners.Cast<ISpawner>())
        {
            powerUpSpawner.Stop();
        }
    }
}
