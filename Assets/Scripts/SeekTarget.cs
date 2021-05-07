using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class SeekTarget : MonoBehaviour
{
    private Transform m_target;

    [SerializeField] private Vector3 offset = Vector3.zero;
    [SerializeField] private Vector3 seekSize = Vector3.one;
    [SerializeField] private int maxTargets = 10;
    [Tag] [SerializeField] private string[] targetTag = new[] {"Enemy"};

    [SerializeField] private float movementSpeed = 10.0f;

    [SerializeField] private float rotationSpeed = 60f;

    [SerializeField] private float timeToLive = 15f;

    [SerializeField] private GameMoveDirectionReference gameMoveDirection;

    private void Start()
    {
        if (timeToLive > 0)
            Destroy(gameObject, timeToLive);
    }

    private void FixedUpdate()
    {
        m_target = PhysicsHelper.GetFirstTargetHit(transform, offset, seekSize, maxTargets, targetTag);

        if (m_target != null)
        {
            Transform localTransform;
            Vector3 targetPosition = m_target.position;
            (localTransform = transform).position =
                Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);

            Vector3 relativePos = targetPosition - localTransform.position;
            Quaternion newRotation = Quaternion.LookRotation(relativePos, Vector3.back);
            newRotation = Quaternion.Euler(0, 0, newRotation.eulerAngles.z);

            transform.rotation =
                Quaternion.RotateTowards(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
        }
        else
            transform.Translate(PositionHelper.GetDirection(gameMoveDirection) * (movementSpeed * Time.deltaTime));
    }

    private void OnDrawGizmos()
    {
        PhysicsHelper.DrawBoxCast(transform, offset, seekSize, maxTargets, targetTag, false);
    }
}
