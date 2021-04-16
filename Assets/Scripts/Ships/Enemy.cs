using System;
using UnityEngine;

public class Enemy : Ship
{
    [Space(5)]
    [Header("Enemy")]
    [Space(3)]
    [SerializeField]
    protected GameMoveDirectionVariable gameMoveDirectionVariable;

    private Vector3Reference m_moveDirection;

    [Range(0, 1)]
    [SerializeField]
    private float moveDirectionAmplitude = 1f;

    [Range(0, 1)]
    [SerializeField]
    private float moveDirectionFrequency = 1f;

    #region Overrides of Ship

    /// <inheritdoc />
    protected override void Awake()
    {
        base.Awake();

        StartCoroutine(fireDelayTimer.CoolDown());

        m_moveDirection = GetComponent<Moveable>().moveDirection;
    }

    /// <inheritdoc />
    protected override void Update()
    {
        base.Update();

        SetMoveDirection();
    }

    /// <inheritdoc />
    protected override bool ShouldFireLaser()
    {
        // If the fire cool down timer is not active return true.
        return !fireDelayTimer.IsActive;
    }

    #endregion

    public void OutOfBoundsAction()
    {
        Vector3 position = transform.position;

        switch (gameMoveDirectionVariable.Value)
        {
            case GameMoveDirectionEnum.TopToBottom:
            {
                if (position.y < bounds.Min.y)
                    transform.position =
                        PositionHelper.GetRandomPosition(gameMoveDirectionVariable.Value, bounds.Value);
                break;
            }
            case GameMoveDirectionEnum.BottomToTop:
            {
                if (position.y > bounds.Max.y)
                    transform.position =
                        PositionHelper.GetRandomPosition(gameMoveDirectionVariable.Value, bounds.Value);
                break;
            }
            case GameMoveDirectionEnum.LeftToRight:
            {
                if (position.x > bounds.Max.x)
                    transform.position =
                        PositionHelper.GetRandomPosition(gameMoveDirectionVariable.Value, bounds.Value);
                break;
            }
            case GameMoveDirectionEnum.RightToLeft:
            {
                if (position.x < bounds.Min.x)
                    transform.position =
                        PositionHelper.GetRandomPosition(gameMoveDirectionVariable.Value, bounds.Value);
                break;
            }
        }
    }

    /// <summary>
    /// Set the move direction so the ship moves.
    /// </summary>
    private void SetMoveDirection()
    {
        float x = m_moveDirection.Value.x;
        float y = m_moveDirection.Value.y;
        float z = m_moveDirection.Value.z;
        float t = Time.time * moveDirectionFrequency;

        switch (gameMoveDirectionVariable.Value)
        {
            case GameMoveDirectionEnum.TopToBottom:
            case GameMoveDirectionEnum.BottomToTop:
                x = Mathf.Cos(t) * moveDirectionAmplitude;
                break;
            case GameMoveDirectionEnum.LeftToRight:
            case GameMoveDirectionEnum.RightToLeft:
                y = Mathf.Sin(t) * moveDirectionAmplitude;
                break;
        }


        m_moveDirection.Value = new Vector3(x, y, z);
    }
}
