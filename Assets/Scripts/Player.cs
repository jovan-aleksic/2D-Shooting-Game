using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    /// <summary>
    /// How fast the player should move in game.
    /// </summary>
    [Tooltip("The Speed that the player moves in game.")]
    [SerializeField]
    private float movementSpeed = 5.0f;

    /// <summary>
    /// The bounds to keep the player position in.
    /// </summary>
    [Tooltip("The Bounds to keep the player in, Is drawn in a yellow box in the scene view")]
    [SerializeField]
    private Bounds screenBounds = new Bounds(Vector3.zero, Vector3.one);

    /// <summary>
    /// The direction to move the player.
    /// </summary>
    private Vector3 m_direction = Vector3.zero;

    /// <summary>
    /// The position that the player is at.
    /// </summary>
    private Vector3 m_position = Vector3.zero;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    private void Start()
    {
        // Take the current position = screen bounds center
        transform.position = screenBounds.center;
    }

    /// <summary>
    ///  Update is called once per frame.
    /// </summary>
    private void Update()
    {
        Move();
    }

    /// <summary>
    /// Moves the player.
    /// </summary>
    private void Move()
    {
        m_direction.x = Input.GetAxis("Horizontal");
        m_direction.y = Input.GetAxis("Vertical");
        // m_direction * movementSpeed * Real Time(Time.deltaTime)
        transform.Translate(m_direction * (movementSpeed * Time.deltaTime));

        // Cache the players position
        m_position = transform.position;

        m_position.y = Mathf.Clamp(m_position.y, screenBounds.min.y, screenBounds.center.y);

        if (m_position.x > screenBounds.max.x)
            m_position.x = screenBounds.min.x;

        if (m_position.x < screenBounds.min.x)
            m_position.x = screenBounds.max.x;

        // set the players position to the cache position
        transform.position = m_position;
    }

    /// <summary>
    /// Implement OnDrawGizmosSelected to draw a gizmo if the object is selected.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        // Draw a Wire cube to indicate where the player is bound too.
        Vector3 boundsCenter = screenBounds.center;
        boundsCenter.y = screenBounds.center.y - screenBounds.extents.y / 2;
        Vector3 boundsSize = screenBounds.size;
        boundsSize.y = screenBounds.extents.y;
        Gizmos.DrawWireCube(boundsCenter, boundsSize);
    }
}
