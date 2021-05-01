using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Scene] [SerializeField] private int newGameScene = 1;
    public void LoadGame()
    {
        SceneManager.LoadScene(sceneBuildIndex: newGameScene);
    }

    public void ExitGame()
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
