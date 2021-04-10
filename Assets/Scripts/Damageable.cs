using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Damageable : MonoBehaviour
{
    [Header("Lives")]
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
    [SerializeField] private GameObject explosionVFX;

    [SerializeField] private AudioClip destroyedSoundEffect;
    private bool m_hasDestroyedSoundEffect;

    private void Awake()
    {
        //lives.Value = maxLives;
        lives.ResetStat();
        shieldHealth.ResetStat();

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
        //if (ShieldActive)
        if (shieldHealth.Value > 0)
        {
            //shield.SetActive(false);
            shieldHealth.Remove(1);
            return;
        }

        //lives.Value--;
        //if (damageReceived != null) damageReceived.Raise();
        lives.Remove(1);
        if (m_hasDamagedSoundEffect) damagedSoundEffect.Play();
        if (lives.Value > 0) return;
        //lives.Value = 0;

        if (gameObjectDestroyed != null)
            gameObjectDestroyed.Raise();

        if (explosionVFX != null)
            Instantiate(explosionVFX, transform.position, transform.rotation);
        if (m_hasDestroyedSoundEffect)
            AudioSource.PlayClipAtPoint(destroyedSoundEffect, transform.position);
        Destroy(gameObject);
    }
}