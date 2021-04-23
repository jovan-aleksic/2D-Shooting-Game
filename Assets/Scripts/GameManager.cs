using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CodedGameEventListener waveSpawnerComplete;
    [SerializeField] private int spawnerCompleteSceneToLoad;

    private void Awake()
    {
        waveSpawnerComplete?.Init(gameObject, WaveSpawnerCompleted);
    }

    private void OnDisable() => waveSpawnerComplete?.OnDisable();
    private void OnEnable() => waveSpawnerComplete?.OnEnable();


    void Update()
    {
        if (Input.GetButtonDown("Exit Game"))
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #elif UNITY_WEBGL
                Application.OpenURL("https://jameslafritz.com/");
            #else
                Application.Quit();
            #endif
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
}
