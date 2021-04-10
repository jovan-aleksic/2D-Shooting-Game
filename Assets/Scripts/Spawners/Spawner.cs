using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawner", menuName = "Spawner", order = 0)]
public class Spawner : ScriptableObject
{
    /// <summary>
    /// The amount of time to wait before spawning begins.
    /// </summary>
    [SerializeField] private float timeToWaitBeforeStarting = 0.2f;

    /// <summary>
    /// The amount of Time to wait between spawning.
    /// </summary>
    [SerializeField] private float spawnTime = 5.0f;

    /// <summary>
    /// The Prefab to spawn.
    /// </summary>
    [SerializeField] private GameObject prefabToSpawn;

    /// <summary>
    /// The Player Lives Variable.
    /// </summary>
    [SerializeField] private StatReference playerLives;

    private WaitForSeconds m_spawnWaitTime;

    private WaitForSeconds m_spawnStartTime;

    private List<GameObject> m_spawnedObjects;

    [HideInInspector] public Coroutine spawnRoutine;

    private bool m_hasBeenInit;

    [SerializeField] protected BoundsVariable screenBounds;

    [SerializeField] private GameMoveDirectionVariable gameMoveDirectionVariable;

    private Vector3 m_positionToSpawnAt;

    public void InitSpawner()
    {
        m_spawnWaitTime = new WaitForSeconds(spawnTime);
        m_spawnStartTime = new WaitForSeconds(timeToWaitBeforeStarting);
        m_spawnedObjects = new List<GameObject>();
        m_hasBeenInit = true;
    }

    public IEnumerator SpawnRoutine(Transform container)
    {
        if (!m_hasBeenInit) InitSpawner();

        if (prefabToSpawn == null) yield break;
        if (container == null) yield break;
        if (m_spawnedObjects == null) m_spawnedObjects = new List<GameObject>();

        yield return m_spawnStartTime;

        while (playerLives.Value > 0)
        {
            m_positionToSpawnAt = PositionHelper.GetRandomPosition(gameMoveDirectionVariable.Value, screenBounds.Value);
            m_spawnedObjects.Add(Instantiate(prefabToSpawn, m_positionToSpawnAt, Quaternion.identity, container));
            yield return m_spawnWaitTime;
        }
    }

    public void DestroySpawnObjects()
    {
        foreach (GameObject spawnedObject in m_spawnedObjects.Where(spawnedObject => spawnedObject != null))
        {
            Destroy(spawnedObject);
        }

        m_spawnedObjects = new List<GameObject>();
    }
}
