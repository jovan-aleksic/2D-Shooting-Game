using UnityEngine;

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

    [SerializeField] private CodedGameEventListener statUpdateGameEventListener;
    private static readonly int Color = Shader.PropertyToID("_Color");

    void Start()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_propBlock = new MaterialPropertyBlock();

        UpdateSpriteColor();
    }

    private void OnEnable() => statUpdateGameEventListener.OnEnable(UpdateSpriteColor);

    private void OnDisable() => statUpdateGameEventListener.OnDisable();

    private void UpdateSpriteColor()
    {
        float shieldHealthPercentage = shieldHealth.Value / shieldHealth.Max;
        m_spriteRenderer.GetPropertyBlock(m_propBlock);
        m_propBlock.SetColor(Color,
                             new Color(shieldHealthPercentage, shieldHealthPercentage, shieldHealthPercentage,
                                       shieldHealthPercentage));
        m_spriteRenderer.SetPropertyBlock(m_propBlock);
    }
}
