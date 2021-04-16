using UnityEngine;

public class Enemy : Ship
{
    [Space(5)]
    [Header("Enemy")]
    [Space(3)]
    [SerializeField]
    protected GameMoveDirectionVariable gameMoveDirectionVariable;

    private Vector3Reference m_moveDirection;

    private float m_amplitude = 1f;
    private float m_frequency = 1f;

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

    private void Start()
    {
        RandomizeMoveDirection();
    }

    private void RandomizeMoveDirection()
    {
        Vector2 randomValue = Random.insideUnitCircle;
        m_amplitude = Mathf.Abs(randomValue.x);
        m_frequency = Mathf.Abs(randomValue.y);
    }

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

        RandomizeMoveDirection();
    }

    /// <summary>
    /// Set the move direction so the ship moves.
    /// </summary>
    private void SetMoveDirection()
    {
        float x = m_moveDirection.Value.x;
        float y = m_moveDirection.Value.y;
        float z = m_moveDirection.Value.z;
        float t = Time.time * m_frequency;

        switch (gameMoveDirectionVariable.Value)
        {
            case GameMoveDirectionEnum.TopToBottom:
            case GameMoveDirectionEnum.BottomToTop:
                x = Mathf.Cos(t) * m_amplitude;
                break;
            case GameMoveDirectionEnum.LeftToRight:
            case GameMoveDirectionEnum.RightToLeft:
                y = Mathf.Sin(t) * m_amplitude;
                break;
        }


        m_moveDirection.Value = new Vector3(x, y, z);
    }
}
