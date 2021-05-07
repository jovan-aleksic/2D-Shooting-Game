using UnityEngine;

public class AggressiveEnemy : EnemyMovement
{
    [SerializeField] private float rotationSpeed = 60f;

    [SerializeField] private Vector3 offset = Vector3.zero;
    [SerializeField] private Vector3 seekSize = Vector3.one;

    [Tag] [SerializeField] private string[] seekTargetTag = new[] {"Player"};

    [SerializeField] private int maxHits = 10;

    private Transform m_target;

    #region Overrides of EnemyMovement

    #region Overrides of Moveable

    protected override void OnDrawGizmosSelected()
    {
        PhysicsHelper.DrawBoxCast(transform, offset, seekSize, maxHits, seekTargetTag, false);
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
                Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);

            Vector3 relativePos = targetPosition - localTransform.position;
            Quaternion newRotation = Quaternion.LookRotation(relativePos, Vector3.forward);
            newRotation = Quaternion.Euler(0, 0, newRotation.eulerAngles.z);

            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
        }
        // else move normally
        else
        {
            Vector3 relativePos = targetPosition - localTransform.position;
            Quaternion newRotation = Quaternion.LookRotation(relativePos, Vector3.forward);
            newRotation = Quaternion.Euler(0, 0, newRotation.eulerAngles.z);
            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);

            base.Move();
        }
    }

    #endregion

    #endregion

    private void FixedUpdate()
    {
        m_target = PhysicsHelper.GetFirstTargetHit(transform, offset, seekSize, maxHits, seekTargetTag);
    }
}
