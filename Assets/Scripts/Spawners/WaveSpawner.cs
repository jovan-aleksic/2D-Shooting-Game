using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveSpawner", menuName = "Spawners/Wave Spawner", order = 0)]
public class WaveSpawner : ScriptableObject, ISpawner
{
    [SerializeField] private GameEvent waveSpawnerComplete;
    [SerializeField] private Wave[] waves;

    [SerializeField] private CoolDownTimer timeBetweenWaves;
    [SerializeField] private GameEvent timerStarted;

    [SerializeField] private bool waitAllObjectsDestroyed = true;

    [SerializeField] private bool useWaitTimeWithDestroyAllObjects;

    [SerializeField] private BoundsVariable screenBounds;

    [SerializeField] private GameMoveDirectionReference gameMoveDirection;

    [SerializeField] private WaveSpawner nextWaveSpawner;

    private Wave m_currentWave;
    [SerializeField] private StringReference currentWaveName;

    private int m_currentWaveNumber;

    private MonoBehaviour m_monoBehaviour;

    private Coroutine m_updateRoutine;

    private bool m_hasBeenInit;

    private bool m_hasNextWaveSpawner;

    private void Awake()
    {
        //Debug.Log("Wave Spawner Awake: " + name);
        m_hasBeenInit = false;
        m_currentWaveNumber = 0;
        m_currentWave = null;
        m_updateRoutine = null;
        m_hasNextWaveSpawner = false;

        foreach (Wave wave in waves)
        {
            wave.Awake();
        }
    }

    #region Implementation of ISpawner

    /// <inheritdoc />
    public void Init(Transform waveContainer, MonoBehaviour monoBehaviour)
    {
        Awake();

        m_monoBehaviour = monoBehaviour;

        if (waveContainer == null) return;
        if (m_monoBehaviour == null) return;
        if (waves == null || waves.Length < 0) return;

        foreach (Wave wave in waves)
        {
            wave.InitWave(waveContainer, screenBounds.Value, gameMoveDirection.Value, monoBehaviour);
        }

        m_hasNextWaveSpawner = nextWaveSpawner != null;

        if (m_hasNextWaveSpawner)
            nextWaveSpawner.Init(waveContainer, monoBehaviour);

        m_hasBeenInit = true;

        //Debug.Log("Init: " + name + " = " + m_hasBeenInit + ", current wave number = " + m_currentWaveNumber);
    }

    /// <inheritdoc />
    public void Start()
    {
        if (!m_hasBeenInit) return;

        if (m_updateRoutine != null)
            Stop();
        SetCurrentWave();
        StartTimer();
        if (m_currentWaveNumber < waves.Length)
            m_updateRoutine = m_monoBehaviour.StartCoroutine(UpdateSpawner());
        else if (m_hasNextWaveSpawner)
            nextWaveSpawner.Start();
    }

    /// <inheritdoc />
    public void Stop()
    {
        if (!m_hasBeenInit) return;

        if (m_updateRoutine != null)
            m_monoBehaviour.StopCoroutine(m_updateRoutine);

        m_currentWave = null;

        // Loop trough the waves starting at the end.
        // If the Wave is active and has objects set the current wave number to the index of this wave.
        // So that when the spawner is Started again it starts where it left off
        for (int i = waves.Length - 1; i > -1; i--)
        {
            Wave wave = waves[i];
            // ReSharper disable once UseNullPropagationWhenPossible
            if (wave == null) continue;
            if (wave.IsActive && wave.HasObjects)
                m_currentWaveNumber = i;
            wave.StopWave();
        }

        if (m_hasNextWaveSpawner)
            nextWaveSpawner.Stop();
    }

    /// <inheritdoc />
    public void DestroyAllSpawnedObjects()
    {
        if (!m_hasBeenInit) return;

        if (waitAllObjectsDestroyed)
            m_currentWave.DestroyAllObjects();
        else
        {
            foreach (Wave wave in waves)
            {
                wave.DestroyAllObjects();
            }
        }

        if (m_hasNextWaveSpawner)
            nextWaveSpawner.DestroyAllSpawnedObjects();
    }

    #endregion

    private void StartTimer()
    {
        m_monoBehaviour.StartCoroutine(timeBetweenWaves.CoolDown());
        if (timerStarted != null) timerStarted.Raise();
    }

    private void SetCurrentWave()
    {
        if (m_currentWaveNumber < waves.Length)
            m_currentWave = waves[m_currentWaveNumber];

        currentWaveName.Value = m_currentWave.WaveName;
    }

    private IEnumerator UpdateSpawner()
    {
        while (timeBetweenWaves.IsActive) yield return null;

        while (m_currentWaveNumber < waves.Length)
        {
            if (!waitAllObjectsDestroyed || useWaitTimeWithDestroyAllObjects)
                while (timeBetweenWaves.IsActive)
                    yield return null;

            m_currentWave.StartWave();

            while (m_currentWave.IsActive && m_currentWaveNumber < waves.Length)
            {
                // Debug.Log("Wave Spawner Update: " + name + " m_currentWave.IsActive = " + m_currentWave.IsActive +
                //           "| m_currentWaveNumber = " + (m_currentWaveNumber) +
                //           "| m_currentWave.HasObjects = " + m_currentWave.HasObjects);
                // if wait for wave destroy all objects and current wave Has Objects is true
                if (waitAllObjectsDestroyed && m_currentWave.HasObjects)
                    yield return null;
                // else if  wait for wave destroy all objects and wave has objects is false
                else if (waitAllObjectsDestroyed && !m_currentWave.HasObjects)
                {
                    // increase the current wave number
                    m_currentWaveNumber++;
                    // Set the current wave
                    SetCurrentWave();
                    // if useWaitTimeWithDestroyAllObjects Start the time Between Waves
                    if (useWaitTimeWithDestroyAllObjects) StartTimer();
                }
                else
                {
                    // increase the current wave number
                    m_currentWaveNumber++;
                    // Set the current wave
                    SetCurrentWave();
                    // start the time Between Waves
                    StartTimer();
                }

                yield return null;
            }
        }

        if (m_hasNextWaveSpawner)
            nextWaveSpawner.Start();
        else if (waveSpawnerComplete != null)
        {
            while (m_currentWave.HasObjects)
                yield return null;

            waveSpawnerComplete.Raise();
        }
    }
}
