using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
public class GameOverController : MonoBehaviour
{
    private Animator m_animator;
    private static readonly int GameOver = Animator.StringToHash("GameOver");

    private void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    public void DisplayGameOver()
    {
        m_animator.SetBool(GameOver, true);
    }
}
