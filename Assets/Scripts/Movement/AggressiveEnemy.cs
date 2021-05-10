using UnityEngine;

public class AggressiveEnemy : EnemyMovement
{
    [SerializeField] private float m_rotationSpeed = 60f;

    [SerializeField] private Vector3 m_offset = Vector3.zero;
    [SerializeField] private Vector3 m_seekSize = Vector3.one;

    [Tag] [SerializeField] private string[] m_seekTargetTag = new[] {"Player"};

    [SerializeField] private int m_maxHits = 10;

    private Transform m_target;

    #region Overrides of EnemyMovement

    #region Overrides of Moveable

    protected override void OnDrawGizmosSelected()
    {
        Color color = Gizmos.color;
        Gizmos.color = Color.magenta;
        PhysicsHelper.DrawBoxCast(transform, m_offset, m_seekSize, m_maxHits, m_seekTargetTag, false);
        Gizmos.color = color;
    }

    /// <inheritdoc />
    protected override void Move()
    {
        Transform localTransform;
        Vector3 targetPosition = (localTransform = transform).position;

        // If there is a target move towards it.Vector3 targetPosition = m_target.position;
        if (m_target != null)
        {
            targetPosition = m_target.position;
            localTransform.position =
                Vector3.MoveTowards(transform.position, targetPosition, m_movementSpeed * Time.deltaTime);

            Vector3 relativePos = targetPosition - localTransform.position;
            Quaternion newRotation = Quaternion.LookRotation(relativePos, Vector3.forward);
            newRotation = Quaternion.Euler(0, 0, newRotation.eulerAngles.z);

            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, newRotation, m_rotationSpeed * Time.deltaTime);
        }
        // else move normally
        else
        {
            Vector3 relativePos = targetPosition - localTransform.position;
            Quaternion newRotation = Quaternion.LookRotation(relativePos, Vector3.forward);
            newRotation = Quaternion.Euler(0, 0, newRotation.eulerAngles.z);
            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, newRotation, m_rotationSpeed * Time.deltaTime);

            base.Move();
        }
    }

    #endregion

    #endregion

    private void FixedUpdate()
    {
        m_target = PhysicsHelper.GetFirstTargetHit(transform, m_offset, m_seekSize, m_maxHits, m_seekTargetTag);
    }
}
