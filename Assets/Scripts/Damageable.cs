using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Damageable : MonoBehaviour
{
    [Header("Lives")]
    [SerializeField]
    private IntReference lives;

    [SerializeField] private int maxLives = 1;

    [Header("Shields")]
    [SerializeField] private GameObject shield;

    private bool m_hasShields;
    private bool ShieldActive => m_hasShields && shield.activeInHierarchy;

    [Header("Tags And Events")]
    [Tooltip("The Tags of the game objects that can do damage to this game object.")]
    [SerializeField]
    private string[] receivesDamageFromTags;

    [SerializeField] private GameEvent damageReceived;

    [SerializeField] private SoundEffect damagedSoundEffect;
    private bool m_hasDamagedSoundEffect;

    [Tooltip("The Game Event to raise when this game object has no more lives and gets destroyed.")] [SerializeField]
    private GameEvent gameObjectDestroyed;

    [Header("Explosion VFX")]
    [SerializeField] private GameObject explosionVFX;

    [SerializeField] private AudioClip destroyedSoundEffect;
    private bool m_hasDestroyedSoundEffect;

    private void Awake()
    {
        lives.Value = maxLives;

        m_hasShields = shield != null;

        m_hasDamagedSoundEffect = damagedSoundEffect != null;

        m_hasDestroyedSoundEffect = destroyedSoundEffect != null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        foreach (string doesDamageTag in receivesDamageFromTags)
        {
            if (!other.CompareTag(doesDamageTag)) continue;

            Damage();
        }
    }

    /// <summary>
    /// Deal damage to this GameObject.
    /// Destroy the Game Object.
    /// </summary>
    private void Damage()
    {
        if (ShieldActive)
        {
            shield.SetActive(false);
            return;
        }

        lives.Value--;
        if (damageReceived != null) damageReceived.Raise();
        if (m_hasDamagedSoundEffect) damagedSoundEffect.Play();
        if (lives.Value > 0) return;
        lives.Value = 0;

        if (gameObjectDestroyed != null)
            gameObjectDestroyed.Raise();

        if (explosionVFX != null)
            Instantiate(explosionVFX, transform.position, transform.rotation);
        if (m_hasDestroyedSoundEffect)
            AudioSource.PlayClipAtPoint(destroyedSoundEffect, transform.position);
        Destroy(gameObject);
    }
}