using System.Linq;
using UnityEngine;

public class SeekTarget : MonoBehaviour
{
    private Transform m_target;

    [SerializeField] private string targetTag = "Enemy";

    [SerializeField] private float movementSpeed = 10.0f;

    [SerializeField] private float timeToLive = 15f;

    [SerializeField] private GameMoveDirectionReference gameMoveDirection;

    void Start()
    {
        if (timeToLive > 0)
            Destroy(gameObject, timeToLive);

        m_target = GameObject.FindGameObjectsWithTag(targetTag).OrderBy(
            go => Vector3.Distance(go.transform.position, transform.position)
        ).FirstOrDefault()?.transform;
    }

    [SerializeField] private float rotationSpeed = 60f;

    private void Update()
    {
        if (m_target != null)
        {
            Transform localTransform;
            Vector3 targetPosition = m_target.position;
            (localTransform = transform).position =
                Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);

            Vector3 relativePos = targetPosition - localTransform.position;
            Quaternion newRotation = Quaternion.LookRotation(relativePos, Vector3.back);
            newRotation = Quaternion.Euler(0, 0, newRotation.eulerAngles.z);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);

            // transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(relativePos, Vector3.back), rotationSpeed * Time.deltaTime);
            // transform.Translate(transform.forward * movementSpeed * Time.deltaTime);
        }
        else
            transform.Translate(PositionHelper.GetDirection(gameMoveDirection) * (movementSpeed * Time.deltaTime));
    }
}
