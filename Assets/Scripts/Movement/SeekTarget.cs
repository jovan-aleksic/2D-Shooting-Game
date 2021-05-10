using UnityEngine;

public class SeekTarget : MonoBehaviour
{
    private Transform m_target;

    [SerializeField] private Vector3 m_offset = Vector3.zero;
    [SerializeField] private Vector3 m_seekSize = Vector3.one;
    [SerializeField] private int m_maxTargets = 10;
    [Tag] [SerializeField] private string[] m_targetTag = new[] {"Enemy"};

    [SerializeField] private float m_movementSpeed = 10.0f;

    [SerializeField] private float m_rotationSpeed = 60f;

    [SerializeField] private float m_timeToLive = 15f;

    [SerializeField] private GameMoveDirectionReference m_gameMoveDirection;

    private void Start()
    {
        if (m_timeToLive > 0)
            Destroy(gameObject, m_timeToLive);
    }

    private void FixedUpdate()
    {
        m_target = PhysicsHelper.GetFirstTargetHit(transform, m_offset, m_seekSize, m_maxTargets, m_targetTag);

        if (m_target != null)
        {
            Transform localTransform;
            Vector3 targetPosition = m_target.position;
            (localTransform = transform).position =
                Vector3.MoveTowards(transform.position, targetPosition, m_movementSpeed * Time.deltaTime);

            Vector3 relativePos = targetPosition - localTransform.position;
            Quaternion newRotation = Quaternion.LookRotation(relativePos, Vector3.back);
            newRotation = Quaternion.Euler(0, 0, newRotation.eulerAngles.z);

            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, newRotation, m_rotationSpeed * Time.deltaTime);
        }
        else
            transform.Translate(PositionHelper.GetDirection(m_gameMoveDirection) * (m_movementSpeed * Time.deltaTime));
    }

    private void OnDrawGizmos()
    {
        PhysicsHelper.DrawBoxCast(transform, m_offset, m_seekSize, m_maxTargets, m_targetTag, false);
    }
}
