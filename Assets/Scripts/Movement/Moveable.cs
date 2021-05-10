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
    protected float m_movementSpeed = 4.0f;


    [SerializeField] protected GameMoveDirectionReference m_gameMoveDirection;

    protected Vector3 moveDirection;


    [SerializeField] protected BoundsVariable m_bounds;


    private Vector3 m_position = Vector3.zero;

    private void Awake()
    {
        Debug.Assert(m_gameMoveDirection != null, nameof(m_gameMoveDirection) + " != null");
        moveDirection = PositionHelper.GetDirection(m_gameMoveDirection.Value);
    }

    /// <summary>
    ///  Update is called once per frame.
    /// </summary>
    private void Update()
    {
        Move();
    }

    /// <summary>
    /// Implement OnDrawGizmosSelected to draw a gizmo if the object is selected.
    /// </summary>
    protected virtual void OnDrawGizmosSelected()
    {
        if (m_bounds == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(m_bounds.Value.center, m_bounds.Value.size);
    }

    /// <summary>
    /// Moves the game object.
    /// </summary>
    protected virtual void Move()
    {
        SetMoveDirection();
        // m_direction * movementSpeed * Real Time(Time.deltaTime)
        transform.Translate(moveDirection * (m_movementSpeed * Time.deltaTime));
        CheckBounds();
    }

    protected virtual void SetMoveDirection() { }

    protected virtual void CheckBounds()
    {
        m_position = transform.position;

        if (m_bounds is { } &&
            !(m_position.x < m_bounds.Min.x) && !(m_position.x > m_bounds.Max.x) &&
            !(m_position.y < m_bounds.Min.y) && !(m_position.y > m_bounds.Max.y)) return;

        Destroy((gameObject));
    }
}
