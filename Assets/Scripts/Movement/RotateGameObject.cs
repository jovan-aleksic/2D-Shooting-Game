using UnityEngine;

public class RotateGameObject : MonoBehaviour
{
    [SerializeField] private float speed = 40f;
    [SerializeField] private Vector3 rotationEulers = Vector3.forward;

    private void Update()
    {
        transform.Rotate(rotationEulers * (speed * Time.deltaTime));
    }
}
