using UnityEngine;
using Debug = System.Diagnostics.Debug;

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

    private void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_propBlock = new MaterialPropertyBlock();

        if (shieldHealth == null) gameObject.SetActive(false);
    }

    private void Start()
    {
        UpdateSpriteColor();
    }

    private void OnEnable()
    {
        if (statUpdateGameEventListener != null) statUpdateGameEventListener.OnEnable(UpdateSpriteColor);
    }

    private void OnDisable()
    {
        if (statUpdateGameEventListener != null) statUpdateGameEventListener.OnDisable();
    }

    private void UpdateSpriteColor()
    {
        Debug.Assert(shieldHealth != null, nameof(shieldHealth) + " != null");
        float shieldHealthPercentage = shieldHealth.Value / shieldHealth.Max;
        Debug.Assert(m_spriteRenderer != null, nameof(m_spriteRenderer) + " != null");
        m_spriteRenderer.GetPropertyBlock(m_propBlock);
        Debug.Assert(m_propBlock != null, nameof(m_propBlock) + " != null");
        m_propBlock.SetColor(Color,
                             new Color(shieldHealthPercentage, shieldHealthPercentage, shieldHealthPercentage,
                                       shieldHealthPercentage));
        m_spriteRenderer.SetPropertyBlock(m_propBlock);
    }
}
