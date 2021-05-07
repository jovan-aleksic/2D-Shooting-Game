using UnityEngine;
using Debug = UnityEngine.Debug;

[RequireComponent(typeof(Collider2D))]
public class Damageable : MonoBehaviour
{
    [SerializeField] private GameObject objectToDestroy;

    [Header("Health")]
    [SerializeField]
    protected StatReference lives;

    [SerializeField] protected StatReference shieldHealth;

    [SerializeField] protected GameObject shieldGameObject;

    private bool m_hasShieldGameObject;

    [Header("Tags And Events")]
    [InfoBox("The Tags of the game objects that can do damage to this game object.", order = 1)]
    [Tooltip("The Tags of the game objects that can do damage to this game object.")]
    [SerializeField]
    [Tag]
    private string[] receivesDamageFromTags;

    [SerializeField] private SoundEffect damagedSoundEffect;
    private bool m_hasDamagedSoundEffect;

    [Tooltip("The Game Event to raise when this game object has no more lives and gets destroyed.")]
    [InfoBox("The Game Event to raise when this game object has no more lives and gets destroyed. Not Required")]
    [SerializeField]
    private GameEvent gameObjectDestroyed;

    [Header("Explosion VFX")]
    [SerializeField]
    private GameObject explosionVFX;

    [SerializeField] private AudioClip destroyedSoundEffect;
    private bool m_hasDestroyedSoundEffect;

    private void Awake()
    {
        m_hasDamagedSoundEffect = damagedSoundEffect != null;

        m_hasDestroyedSoundEffect = destroyedSoundEffect != null;

        if (objectToDestroy == null) objectToDestroy = gameObject;

        m_hasShieldGameObject = shieldGameObject != null;
    }

    protected virtual void Start()
    {
        Debug.Assert(lives != null, nameof(lives) + " != null");
        lives.ResetStat();
        Debug.Assert(shieldHealth != null, nameof(shieldHealth) + " != null");
        shieldHealth.ResetStat();
        DisableShields();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (receivesDamageFromTags == null) return;

        foreach (string doesDamageTag in receivesDamageFromTags)
        {
            Debug.Assert(other != null, nameof(other) + " != null");
            if (!other.CompareTag(doesDamageTag)) continue;

            Damage();
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

        Debug.Assert(lives != null, nameof(lives) + " != null");
        lives.Remove(1);
        if (m_hasDamagedSoundEffect)
        {
            Debug.Assert(damagedSoundEffect != null, nameof(damagedSoundEffect) + " != null");
            damagedSoundEffect.Play();
        }

        if (lives.Value > 0) return;

        if (gameObjectDestroyed != null)
            gameObjectDestroyed.Raise();

        if (explosionVFX != null)
        {
            Transform transform1 = transform;
            Instantiate(explosionVFX, transform1.position, transform1.rotation);
        }

        if (m_hasDestroyedSoundEffect)
            AudioSource.PlayClipAtPoint(destroyedSoundEffect, transform.position);
        Destroy(objectToDestroy);
    }

    private void DisableShields()
    {
        Debug.Assert(shieldHealth != null, nameof(shieldHealth) + " != null");
        if (!m_hasShieldGameObject || shieldHealth.Value != 0) return;
        Debug.Assert(shieldGameObject != null, nameof(shieldGameObject) + " != null");
        shieldGameObject.SetActive(false);
    }
}