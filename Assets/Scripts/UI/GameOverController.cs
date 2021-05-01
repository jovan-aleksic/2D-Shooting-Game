using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = System.Diagnostics.Debug;

[RequireComponent(typeof(Animator))]
public class GameOverController : MonoBehaviour
{
    private Animator m_animator;
    private static readonly int GameOver = Animator.StringToHash("GameOver");

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    public void DisplayGameOver()
    {
        Debug.Assert(m_animator != null, nameof(m_animator) + " != null");
        m_animator.SetBool(GameOver, true);
    }
}
