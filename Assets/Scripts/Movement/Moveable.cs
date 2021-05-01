using UnityEngine;

[DisallowMultipleComponent]
public class Moveable : MonoBehaviour
{
    /// <summary>
    /// How Fast the game object moves in game.
    /// </summary>
    [InfoBox("The Speed that the game object moves in game.")]
    [Tooltip("The Speed that the game object moves in game.")]
    [SerializeField]
    private float movementSpeed = 4.0f;

    [SerializeField] protected GameMoveDirectionReference gameMoveDirection;

    protected Vector3 moveDirection;

    [SerializeField] protected BoundsVariable bounds;

    private Vector3 m_position = Vector3.zero;

    private void Awake()
    {
        moveDirection = PositionHelper.GetDirection(gameMoveDirection.Value);
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
        SetMoveDirection();
        // m_direction * movementSpeed * Real Time(Time.deltaTime)
        transform.Translate(moveDirection * (movementSpeed * Time.deltaTime));
        CheckBounds();
    }

    protected virtual void SetMoveDirection() { }

    protected virtual void CheckBounds()
    {
        m_position = transform.position;

        if (bounds is { } &&
            !(m_position.x < bounds.Min.x) && !(m_position.x > bounds.Max.x) &&
            !(m_position.y < bounds.Min.y) && !(m_position.y > bounds.Max.y)) return;

        Destroy((gameObject));
    }
}
