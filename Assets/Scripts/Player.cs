using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Tooltip("The speed the player should move in game.")]
    [Range(1, 20)]
    [SerializeField]
    private float speed = 5.0f;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        
    }

    /// <summary>
    ///  Update is called once per frame.
    /// </summary>
    void Update()
    {
        Move();
    }

    // Move the player
    private void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * (Time.deltaTime * speed));
    }
}
