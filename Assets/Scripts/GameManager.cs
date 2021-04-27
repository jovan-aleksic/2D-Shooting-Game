using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CodedGameEventListener waveSpawnerComplete;
    [SerializeField] private int spawnerCompleteSceneToLoad;

    [SerializeField] private IntReference score;
    private int m_savedScore;

    [SerializeField] private CodedGameEventListener playerDestroyed;

    [SerializeField] private GameOverController gameOverController;

    private bool m_gameOver;

    private void OnDisable()
    {
        waveSpawnerComplete?.OnDisable();
        playerDestroyed?.OnDisable();
    }

    private void OnEnable()
    {
        waveSpawnerComplete?.OnEnable(WaveSpawnerCompleted);
        playerDestroyed?.OnEnable(OnPlayerDestroyed);
    }

    private void Awake()
    {
        m_savedScore = score.Value;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Exit Game"))
        {
            SceneManager.LoadScene(0);
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
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(spawnerCompleteSceneToLoad);
    }

    private void OnPlayerDestroyed()
    {
        if (gameOverController != null)
            gameOverController.DisplayGameOver();
        m_gameOver = true;
    }

    private void RestartLevel()
    {
        score.Value = m_savedScore;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
