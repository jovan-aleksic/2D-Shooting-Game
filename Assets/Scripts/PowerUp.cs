using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    // power up identifier

    [SerializeField] private float movementSpeed = 4.0f;
    [SerializeField] private BoundsVariable bounds;
    private Vector3 m_moveDirection = new Vector3(0, -1, 0);
    private Vector3 m_position = Vector3.zero;

    private void Start()
    {
        transform.position = new Vector3(Random.Range
                                             (bounds.Min.x + 0.00001f, bounds.Max.x),
                                         bounds.Max.y - 0.00001f, 0f);
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.Translate(m_moveDirection * (movementSpeed * Time.deltaTime));
        CheckBounds();
    }

    private void CheckBounds()
    {
        m_position = transform.position;

        if (!(m_position.x < bounds.Min.x) && !(m_position.x > bounds.Max.x) &&
            !(m_position.y < bounds.Min.y) && !(m_position.y > bounds.Max.y)) return;
        
        // if triple shot power up
        // activate the triple shot on the player
        
        // else if speed power up
        // activate the speed boost on the player
        
        // else if shield power up
        // activate the shields on the player
        
        // else
        // log a warning that this is not a selectable option
        
        Destroy((gameObject));
    }
}
