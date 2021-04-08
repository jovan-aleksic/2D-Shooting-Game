using UnityEngine;

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
    }
}
