using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Change the Shields Sprite Color Parameter based on the percentage of health that the shield has.
/// Uses Material Property Block to keep it from creating an extra copy of the material.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class ShieldsSpriteColorChanger : MonoBehaviour
{
    private SpriteRenderer m_spriteRenderer;
    private MaterialPropertyBlock m_propBlock;

    [SerializeField] private StatReference shieldHealth;

    [SerializeField] private CodedGameEventListener m_statUpdateGameEventListener;

    void Start()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_propBlock = new MaterialPropertyBlock();

        UpdateSpriteColor();
    }

    private void OnEnable() => m_statUpdateGameEventListener.OnEnable(UpdateSpriteColor);

    private void OnDisable() => m_statUpdateGameEventListener.OnDisable();

    private void UpdateSpriteColor()
    {
        float shieldHealthPercentage = shieldHealth.Value / shieldHealth.Max;
        m_spriteRenderer.GetPropertyBlock(m_propBlock);
        m_propBlock.SetColor("_Color",
                             new Color(shieldHealthPercentage, shieldHealthPercentage, shieldHealthPercentage,
                                       shieldHealthPercentage));
        m_spriteRenderer.SetPropertyBlock(m_propBlock);
    }
}
