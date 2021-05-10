using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = System.Diagnostics.Debug;

public class GameManager : MonoBehaviour
{
    [Scene] [SerializeField] private string m_mainMenuScene;

    [SerializeField] private CodedGameEventListener m_waveSpawnerComplete;
    [Scene] [SerializeField] private int m_spawnerCompleteSceneToLoad;

    [SerializeField] private IntReference m_score;
    private int m_savedScore;

    [SerializeField] private CodedGameEventListener m_playerDestroyed;

    [SerializeField] private GameOverController m_gameOverController;

    [SerializeField] private Text m_winText;

    private bool m_gameOver;

    private void OnDisable()
    {
        m_waveSpawnerComplete?.OnDisable();
        m_playerDestroyed?.OnDisable();
    }

    private void OnEnable()
    {
        m_waveSpawnerComplete?.OnEnable(WaveSpawnerCompleted);
        m_playerDestroyed?.OnEnable(OnPlayerDestroyed);
    }

    private void Awake()
    {
        Debug.Assert(m_score != null, nameof(m_score) + " != null");
        m_savedScore = m_score.Value;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Exit Game"))
        {
            SceneManager.LoadScene(m_mainMenuScene);
        }

        if (m_gameOver && Input.GetButtonDown("Restart Level"))
        {
            RestartLevel();
        }
    }

    private void WaveSpawnerCompleted()
    {
        StartCoroutine(WaveSpawnerCompleteRoutine());
    }

    private IEnumerator WaveSpawnerCompleteRoutine()
    {
        if (m_winText is { }) m_winText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(m_spawnerCompleteSceneToLoad);
    }

    private void OnPlayerDestroyed()
    {
        if (m_gameOverController != null)
            m_gameOverController.DisplayGameOver();
        m_gameOver = true;
    }

    private void RestartLevel()
    {
        Debug.Assert(m_score != null, nameof(m_score) + " != null");
        m_score.Value = m_savedScore;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
