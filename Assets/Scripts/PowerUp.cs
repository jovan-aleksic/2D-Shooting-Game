using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private const int MAXTargets = 10;

    [Tag] [SerializeField] private string m_collectorTag = "Player";
    [Tag] [SerializeField] private string m_destroyedByTag = "Power Up Destroyer";

    [SerializeField] private AudioClip m_collectedSoundEffect;
    private bool m_hasCollectedSoundEffect;

    [SerializeField] private GameEvent m_collectedGameEvent;

    [InputAxis] [SerializeField] private string m_collectorInput = "Fire3";
    private bool m_shouldMoveTowardsTarget;
    private string[] m_targetTag;
    [SerializeField] private float m_moveTowardsTargetSpeed = 10f;
    private Transform m_target;
    private readonly Vector2 m_seekSize = new Vector2(10, 10);

    private void Awake()
    {
        m_hasCollectedSoundEffect = m_collectedSoundEffect != null;
        m_targetTag = new[] {m_collectorTag};
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Assert(other != null, nameof(other) + " != null");
        if (!string.IsNullOrEmpty(m_collectorTag) && other.CompareTag(m_collectorTag))
        {
            if (m_hasCollectedSoundEffect)
            {
                AudioSource.PlayClipAtPoint(m_collectedSoundEffect, transform.position);
            }

            if (m_collectedGameEvent != null)
                m_collectedGameEvent.Raise();

            Destroy(gameObject);
        }

        else if (!string.IsNullOrEmpty(m_destroyedByTag) && other.CompareTag(m_destroyedByTag))
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        m_shouldMoveTowardsTarget = Input.GetButton(m_collectorInput);
    }

    private void FixedUpdate()
    {
        if (!m_shouldMoveTowardsTarget) return;
        m_target = PhysicsHelper.GetFirstTargetHit(transform, Vector2.zero, m_seekSize, MAXTargets, m_targetTag);

        if (m_target == null) return;
        Vector3 targetPosition = m_target.position;
        (transform).position =
            Vector3.MoveTowards(transform.position, targetPosition, m_moveTowardsTargetSpeed * Time.deltaTime);
    }
}
