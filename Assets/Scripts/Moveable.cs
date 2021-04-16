using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class Moveable : MonoBehaviour
{
    /// <summary>
    /// How Fast the game object moves in game.
    /// </summary>
    [Tooltip("The Speed that the game object moves in game.")] [SerializeField]
    private float movementSpeed = 4.0f;

    [SerializeField] private GameMoveDirectionReference gameMoveDirection;

    /// <summary>
    /// The direction to move the game object.
    /// </summary>
    [Tooltip("The direction the Game Object Should move. This value gets set by the game move direction value. If you are using a component that will change the move direction of this object then set this to the same variable.")] [SerializeField]
    public Vector3Reference moveDirection;

    [SerializeField] private BoundsVariable bounds;

    [Tooltip("Actions to perform when the Game object goes out of bounds. If this is not Set then the game object that this is attached to will be destroyed.")]
    [SerializeField]
    private UnityEvent outOfBoundsEvent;

    private Vector3 m_position = Vector3.zero;

    private void Awake()
    {
        moveDirection.Value = PositionHelper.GetDirection(gameMoveDirection.Value);
    }

    /// <summary>
    ///  Update is called once per frame.
    /// </summary>
    private void Update()
    {
        Move();
    }

    /// <summary>
    /// Moves the game object.
    /// </summary>
    private void Move()
    {
        // m_direction * movementSpeed * Real Time(Time.deltaTime)
        transform.Translate(moveDirection.Value * (movementSpeed * Time.deltaTime));
        CheckBounds();
    }

    private void CheckBounds()
    {
        m_position = transform.position;

        if (!(m_position.x < bounds.Min.x) && !(m_position.x > bounds.Max.x) &&
            !(m_position.y < bounds.Min.y) && !(m_position.y > bounds.Max.y)) return;

        if (outOfBoundsEvent != null && outOfBoundsEvent.GetPersistentEventCount() > 0)
            outOfBoundsEvent.Invoke();
        else
            Destroy((gameObject));
    }
}
