using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Damageable : MonoBehaviour
{
    [Header("Health")]
    [SerializeField]
    private StatReference lives;

    [SerializeField] private StatReference shieldHealth;

    [Header("Tags And Events")]
    [Tooltip("The Tags of the game objects that can do damage to this game object.")]
    [SerializeField]
    private string[] receivesDamageFromTags;

    [SerializeField] private SoundEffect damagedSoundEffect;
    private bool m_hasDamagedSoundEffect;

    [Tooltip("The Game Event to raise when this game object has no more lives and gets destroyed.")] [SerializeField]
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
    }

    private void Start()
    {
        lives.ResetStat();
        shieldHealth.ResetStat();
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
        if (shieldHealth.Value > 0)
        {
            shieldHealth.Remove(1);
            return;
        }

        lives.Remove(1);
        if (m_hasDamagedSoundEffect) damagedSoundEffect.Play();
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
        Destroy(gameObject);
    }
}