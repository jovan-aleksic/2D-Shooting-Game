using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
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
        
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(1);
    }
}
