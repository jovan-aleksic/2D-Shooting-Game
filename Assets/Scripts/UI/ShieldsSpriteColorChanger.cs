using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Change the Shields Sprite Color Parameter based on the percentage of health that the shield has.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class ShieldsSpriteColorChanger : MonoBehaviour
{
    private SpriteRenderer m_spriteRenderer;

    [SerializeField] private StatReference shieldHealth;

    [SerializeField] private GameEvent statUpdateEvent;

    private GameEventListener m_statUpdateGameEventListener;

    void Start()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();

        m_statUpdateGameEventListener = gameObject.AddComponent<GameEventListener>();
        m_statUpdateGameEventListener.response = new UnityEvent();
        m_statUpdateGameEventListener.response.AddListener(UpdateSpriteColor);
        m_statUpdateGameEventListener.@event = statUpdateEvent;
        statUpdateEvent.RegisterListener(m_statUpdateGameEventListener);

        UpdateSpriteColor();
    }

    private void UpdateSpriteColor()
    {
        float shieldHealthPercentage = shieldHealth.Value / shieldHealth.Max;
        m_spriteRenderer.color = new Color(shieldHealthPercentage, shieldHealthPercentage, shieldHealthPercentage, shieldHealthPercentage);
    }
}
