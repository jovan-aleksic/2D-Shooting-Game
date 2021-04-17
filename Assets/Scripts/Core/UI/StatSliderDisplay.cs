using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class StatSliderDisplay : MonoBehaviour
{
    private Slider m_slider;
    private Text m_statText;
    private bool m_hasText;

    [SerializeField] private string statName = "Stat";

    [SerializeField] private StatReference stat;

    [SerializeField] private GameEvent statUpdateEvent;

    GameEventListener m_statUpdateGameEventListener;

    public Gradient gradient;
    public Image fill;

    private void Start()
    {
        m_slider = GetComponent<Slider>();
        m_statText = GetComponentInChildren<Text>();
        m_hasText = m_statText != null;

        UpdateDisplay();

        m_statUpdateGameEventListener = gameObject.AddComponent<GameEventListener>();
        m_statUpdateGameEventListener.response = new UnityEvent();
        m_statUpdateGameEventListener.response.AddListener(UpdateDisplay);
        m_statUpdateGameEventListener.@event = statUpdateEvent;
        statUpdateEvent.RegisterListener(m_statUpdateGameEventListener);
    }

    private void UpdateDisplay()
    {
        m_slider.maxValue = stat.Max;
        m_slider.value = stat.Value;
        fill.color = gradient.Evaluate(m_slider.normalizedValue);

        if (m_hasText)
        {
            m_statText.text = statName + ": " + stat;
        }
    }
}
