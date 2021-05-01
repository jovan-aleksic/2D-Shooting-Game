using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Debug = System.Diagnostics.Debug;

[RequireComponent(typeof(Text), typeof(Animator))]
public class WaveTimerDisplay : MonoBehaviour
{
    private Text m_text;
    private Animator m_animator;
    [SerializeField] private StringReference currentWaveName;

    [SerializeField] private FloatReference waveTimerPercentage;

    private bool m_hasWaveName, m_hasWaveTimerPercentage;

    [SerializeField] private CodedGameEventListener gameEventListener;
    private static readonly int ShowDisplay = Animator.StringToHash("ShowDisplay");

    private Coroutine m_displayCoroutine;

    private void Awake()
    {
        m_text = GetComponent<Text>();
        m_animator = GetComponent<Animator>();

        m_hasWaveName = currentWaveName is {Value: { }};
        m_hasWaveTimerPercentage = waveTimerPercentage is {Value: { }};
    }

    private void OnDisable()
    {
        if (gameEventListener != null) gameEventListener.OnDisable();
    }

    private void OnEnable()
    {
        if (gameEventListener != null) gameEventListener.OnEnable(EnableDisplay);
    }

    private void EnableDisplay()
    {
        if (m_displayCoroutine != null)
            StopCoroutine(m_displayCoroutine);

        m_displayCoroutine = StartCoroutine(DisplayCoroutine());
    }

    private IEnumerator DisplayCoroutine()
    {
        Debug.Assert(m_text != null, nameof(m_text) + " != null");
        if (m_hasWaveName)
        {
            Debug.Assert(currentWaveName != null, nameof(currentWaveName) + " != null");
            m_text.text = currentWaveName.Value;
        }

        Debug.Assert(m_animator != null, nameof(m_animator) + " != null");
        m_animator.SetBool(ShowDisplay, true);

        if (m_hasWaveTimerPercentage)
        {
            Debug.Assert(waveTimerPercentage != null, nameof(waveTimerPercentage) + " != null");
            while (waveTimerPercentage.Value < 1)
            {
                yield return null;
            }
        }

        yield return new WaitForSeconds(0.5f);

        m_animator.SetBool(ShowDisplay, false);
    }
}
