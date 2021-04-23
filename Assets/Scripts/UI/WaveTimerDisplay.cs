using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text), typeof(Animator))]
public class WaveTimerDisplay : MonoBehaviour
{
    private Text m_text;
    private Animator m_animator;
    [SerializeField] private StringReference currentWaveName;

    [SerializeField] private FloatReference waveTimerPercentage;

    [SerializeField] private CodedGameEventListener gameEventListener;
    private static readonly int ShowDisplay = Animator.StringToHash("ShowDisplay");

    private Coroutine m_displayCoroutine;

    private void Start()
    {
        m_text = GetComponent<Text>();
        m_animator = GetComponent<Animator>();
        gameEventListener.Init(gameObject, EnableDisplay);
    }

    private void OnDisable() => gameEventListener.OnDisable();
    private void OnEnable() => gameEventListener.OnEnable();

    private void EnableDisplay()
    {
        if (m_displayCoroutine != null)
            StopCoroutine(m_displayCoroutine);

        m_displayCoroutine = StartCoroutine(DisplayCoroutine());
    }

    private IEnumerator DisplayCoroutine()
    {
        m_text.text = currentWaveName.Value;
        m_animator.SetBool(ShowDisplay, true);

        while (waveTimerPercentage.Value < 1)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        m_animator.SetBool(ShowDisplay, false);
    }
}
