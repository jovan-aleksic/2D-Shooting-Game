using UnityEngine;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

[RequireComponent(typeof(Collider2D))]
public class Damageable : MonoBehaviour
{
    [FormerlySerializedAs("objectToDestroy")] [SerializeField] private GameObject m_objectToDestroy;

    [Header("Health")]
    [SerializeField]
    [FormerlySerializedAs("lives")]
    protected StatReference m_lives;

    // ReSharper disable once InconsistentNaming
    [SerializeField] protected StatReference shieldHealth;

    // ReSharper disable once InconsistentNaming
    [SerializeField] protected GameObject shieldGameObject;

    private bool m_hasShieldGameObject;

    [Header("Tags And Events")]
    [InfoBox("The Tags of the game objects that can do damage to this game object.", order = 1)]
    [Tooltip("The Tags of the game objects that can do damage to this game object.")]
    [FormerlySerializedAs("receivesDamageFromTags")]
    [SerializeField]
    [Tag]
    private string[] m_receivesDamageFromTags;

    [Tooltip("The Game Event to raise when this game object has no more lives and gets destroyed.")]
    [InfoBox("The Game Event to raise when this game object has no more lives and gets destroyed. Not Required")]
    [FormerlySerializedAs("gameObjectDestroyed")]
    [SerializeField]
    private GameEvent m_gameObjectDestroyed;


    [SerializeField] private float m_timeBeforeCausingDamage = 1f;
    private float m_nextDamageTime;

    [FormerlySerializedAs("damagedSoundEffect")] [SerializeField]
    private SoundEffect m_damagedSoundEffect;

    private bool m_hasDamagedSoundEffect;

    [Header("Explosion VFX")]
    [FormerlySerializedAs("explosionVFX")]
    [SerializeField]
    private GameObject m_explosionVFX;

    [FormerlySerializedAs("destroyedSoundEffect")] [SerializeField]
    private AudioClip m_destroyedSoundEffect;

    private bool m_hasDestroyedSoundEffect;

    private void Awake()
    {
        m_hasDamagedSoundEffect = m_damagedSoundEffect != null;

        m_hasDestroyedSoundEffect = m_destroyedSoundEffect != null;

        if (m_objectToDestroy == null) m_objectToDestroy = gameObject;

        m_hasShieldGameObject = shieldGameObject != null;
    }

    protected virtual void Start()
    {
        Debug.Assert(m_lives != null, nameof(m_lives) + " != null");
        m_lives.ResetStat();
        Debug.Assert(shieldHealth != null, nameof(shieldHealth) + " != null");
        shieldHealth.ResetStat();
        DisableShields();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (m_receivesDamageFromTags == null) return;

        foreach (string doesDamageTag in m_receivesDamageFromTags)
        {
            Debug.Assert(other != null, nameof(other) + " != null");
            if (!other.CompareTag(doesDamageTag)) continue;

            Damage();
            m_nextDamageTime = Time.time + m_timeBeforeCausingDamage;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (Time.time < m_nextDamageTime) return;
        if (m_receivesDamageFromTags == null) return;

        foreach (string doesDamageTag in m_receivesDamageFromTags)
        {
            Debug.Assert(other != null, nameof(other) + " != null");
            if (!other.CompareTag(doesDamageTag)) continue;

            Damage();
            m_nextDamageTime = Time.time + m_timeBeforeCausingDamage;
            return;
        }
    }

    /// <summary>
    /// Deal damage to this GameObject.
    /// Destroy the Game Object.
    /// </summary>
    protected virtual void Damage()
    {
        Debug.Assert(shieldHealth != null, nameof(shieldHealth) + " != null");
        if (shieldHealth.Value > 0)
        {
            shieldHealth.Remove(1);
            DisableShields();
            return;
        }

        Debug.Assert(m_lives != null, nameof(m_lives) + " != null");
        m_lives.Remove(1);
        if (m_hasDamagedSoundEffect)
        {
            Debug.Assert(m_damagedSoundEffect != null, nameof(m_damagedSoundEffect) + " != null");
            m_damagedSoundEffect.Play();
        }

        if (m_lives.Value > 0) return;

        if (m_gameObjectDestroyed != null)
            m_gameObjectDestroyed.Raise();

        if (m_explosionVFX != null)
        {
            Transform transform1 = transform;
            Instantiate(m_explosionVFX, transform1.position, transform1.rotation);
        }

        if (m_hasDestroyedSoundEffect)
            AudioSource.PlayClipAtPoint(m_destroyedSoundEffect, transform.position);
        Destroy(m_objectToDestroy);
    }

    private void DisableShields()
    {
        Debug.Assert(shieldHealth != null, nameof(shieldHealth) + " != null");
        if (!m_hasShieldGameObject || shieldHealth.Value != 0) return;
        Debug.Assert(shieldGameObject != null, nameof(shieldGameObject) + " != null");
        shieldGameObject.SetActive(false);
    }
}