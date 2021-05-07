using UnityEngine;
using Debug = System.Diagnostics.Debug;

public class PowerUp : MonoBehaviour
{
    [Tag] [SerializeField] private string collectorTag = "Player";
    [Tag] [SerializeField] private string destroyedByTag = "Power Up Destroyer";

    [SerializeField] private AudioClip collectedSoundEffect;
    private bool m_hasCollectedSoundEffect;

    [SerializeField] private GameEvent collectedGameEvent;

    private void Awake()
    {
        m_hasCollectedSoundEffect = collectedSoundEffect != null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Assert(other != null, nameof(other) + " != null");
        if (!string.IsNullOrEmpty(collectorTag) && other.CompareTag(collectorTag))
        {
            if (m_hasCollectedSoundEffect)
            {
                AudioSource.PlayClipAtPoint(collectedSoundEffect, transform.position);
            }

            if (collectedGameEvent != null)
                collectedGameEvent.Raise();

            Destroy(gameObject);
        }

        else if (!string.IsNullOrEmpty(destroyedByTag) && other.CompareTag(destroyedByTag))
        {
            Destroy(gameObject);
        }
    }
}
