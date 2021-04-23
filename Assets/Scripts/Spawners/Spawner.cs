using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawner", menuName = "Spawners/Spawner", order = 0)]
public class Spawner : ScriptableObject, ISpawner
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

    private bool m_hasBeenInit;

    [SerializeField] private BoundsVariable screenBounds;

    [SerializeField] private GameMoveDirectionVariable gameMoveDirectionVariable;

    private Vector3 m_positionToSpawnAt;

    private Coroutine m_spawnRoutine;

    private MonoBehaviour m_monoBehaviour;

    private Transform m_container;

    #region Implementation of ISpawner

    public void Init(Transform container, MonoBehaviour monoBehaviour)
    {
        m_container = container;
        m_monoBehaviour = monoBehaviour;
        m_spawnWaitTime = new WaitForSeconds(spawnTime);
        m_spawnStartTime = new WaitForSeconds(timeToWaitBeforeStarting);
        m_spawnedObjects = new List<GameObject>();
        m_hasBeenInit = true;
    }

    public void Start()
    {
        if (!m_hasBeenInit) return;
        Stop();
        m_spawnRoutine = m_monoBehaviour.StartCoroutine(SpawnRoutine());
    }

    public void Stop()
    {
        if (!m_hasBeenInit) return;
        if (m_spawnRoutine != null)
            m_monoBehaviour.StopCoroutine(SpawnRoutine());
    }

    public void DestroyAllSpawnedObjects()
    {
        for (int i = m_spawnedObjects.Count - 1; i > -1; i--)
        {
            GameObject spawnedObject = m_spawnedObjects[i];
            m_spawnedObjects.Remove(spawnedObject);
            Destroy(spawnedObject);
        }
    }

    #endregion

    private IEnumerator SpawnRoutine()
    {
        if (prefabToSpawn == null) yield break;
        if (m_container == null) yield break;
        if (m_spawnedObjects == null) m_spawnedObjects = new List<GameObject>();

        yield return m_spawnStartTime;

        while (playerLives.Value > 0)
        {
            m_positionToSpawnAt = PositionHelper.GetRandomPosition(gameMoveDirectionVariable.Value, screenBounds.Value);
            m_spawnedObjects.Add(Instantiate(prefabToSpawn, m_positionToSpawnAt, Quaternion.identity, m_container));

            for (int i = m_spawnedObjects.Count - 1; i > -1; i--)
            {
                if (m_spawnedObjects[i] == null)
                    m_spawnedObjects.Remove(m_spawnedObjects[i]);
            }

            yield return m_spawnWaitTime;
        }
    }
}
