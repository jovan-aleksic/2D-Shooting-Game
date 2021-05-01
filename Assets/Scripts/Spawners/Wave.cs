using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    [SerializeField] private string name = "Wave";

    public string WaveName => name;
    [SerializeField] private int numberObjectsInWave = 3;
    [SerializeField] private int numberObjectsToSpawnAtATime = 1;

    [SerializeField] private float timeBetweenSpawning = 5.0f;

    private int m_currentObjectNumber = 0;
    [SerializeField] private GameObject[] prefabsToSpawn;

    private Transform m_container;
    private Bounds m_screenBounds;
    private GameMoveDirectionEnum m_gameMoveDirection;
    private Vector3 m_positionToSpawnAt;
    private List<GameObject> m_spawnedObjects;
    public bool HasObjects => m_spawnedObjects != null && m_spawnedObjects.Count > 0 || m_currentObjectNumber < numberObjectsInWave;

    private WaitForSeconds m_spawnWaitTime;
    private MonoBehaviour m_monoBehaviour;
    private bool m_hasBeenInit;
    private Coroutine m_spawnRoutine;
    public bool IsActive { get; private set; }

    public void Awake()
    {
        m_currentObjectNumber = 0;
        IsActive = false;
        m_spawnRoutine = null;
    }

    public void InitWave(Transform container, Bounds screenBounds, GameMoveDirectionEnum gameMoveDirection, MonoBehaviour monoBehaviour)
    {
        m_container = container;
        m_screenBounds = screenBounds;
        m_gameMoveDirection = gameMoveDirection;
        m_monoBehaviour = monoBehaviour;
        m_spawnWaitTime = new WaitForSeconds(timeBetweenSpawning);
        m_spawnedObjects = new List<GameObject>();
        m_hasBeenInit = prefabsToSpawn != null && prefabsToSpawn.Length > 0;
    }

    private IEnumerator SpawnRoutine()
    {
        if (prefabsToSpawn == null || prefabsToSpawn.Length < 1) yield break;
        if (m_container == null) yield break;
        if (m_spawnedObjects == null) m_spawnedObjects = new List<GameObject>();

        IsActive = true;

        while (m_currentObjectNumber < numberObjectsInWave)
        {
            m_positionToSpawnAt = PositionHelper.GetRandomPosition(m_gameMoveDirection, m_screenBounds);
            for (int i = 0; i < numberObjectsToSpawnAtATime; i++)
            {
                int prefabIndex = Random.Range(0, prefabsToSpawn.Length);
                m_spawnedObjects.Add(Object.Instantiate(prefabsToSpawn[prefabIndex], m_positionToSpawnAt,
                                                        Quaternion.identity, m_container));
                m_currentObjectNumber++;
            }

            yield return m_spawnWaitTime;
        }

        while (HasObjects)
        {
            for (int i = m_spawnedObjects.Count - 1; i > -1; i--)
            {
                if (m_spawnedObjects[i] == null)
                    m_spawnedObjects.Remove(m_spawnedObjects[i]);
            }

            yield return m_spawnWaitTime;
        }
    }

    public void StartWave()
    {
        if (!m_hasBeenInit) return;
        StopWave();

        m_spawnRoutine = m_monoBehaviour.StartCoroutine(SpawnRoutine());
    }

    public void StopWave()
    {
        if (!m_hasBeenInit) return;

        if (m_spawnRoutine != null)
            m_monoBehaviour.StopCoroutine(m_spawnRoutine);

        IsActive = false;
    }

    public void DestroyAllObjects()
    {
        for (int i = m_spawnedObjects.Count - 1; i > -1; i--)
        {
            GameObject spawnedObject = m_spawnedObjects[i];
            m_spawnedObjects.Remove(spawnedObject);
            Object.Destroy(spawnedObject);
        }
    }
}
