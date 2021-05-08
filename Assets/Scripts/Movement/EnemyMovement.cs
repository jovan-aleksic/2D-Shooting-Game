using UnityEngine;

public class EnemyMovement : Moveable
{
    private float m_amplitude = 1f;
    private float m_frequency = 1f;

    protected virtual void Start()
    {
        RandomizeMoveDirection();
    }

    private void RandomizeMoveDirection()
    {
        Vector2 randomValue = Random.insideUnitCircle;
        m_amplitude = Mathf.Abs(randomValue.x);
        m_frequency = Mathf.Abs(randomValue.y);
    }

    #region Overrides of Moveable

    /// <inheritdoc />
    protected override void SetMoveDirection()
    {
        if (m_gameMoveDirection == null) return;

        float x = moveDirection.x;
        float y = moveDirection.y;
        float z = moveDirection.z;
        float t = Time.time * m_frequency;

        switch (m_gameMoveDirection.Value)
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


        moveDirection = new Vector3(x, y, z);
    }

    /// <inheritdoc />
    protected override void CheckBounds()
    {
        if (m_gameMoveDirection == null || m_bounds == null) return;

        Vector3 position = transform.position;

        switch (m_gameMoveDirection.Value)
        {
            case GameMoveDirectionEnum.TopToBottom:
            {
                if (position.y < m_bounds.Min.y)
                    transform.position =
                        PositionHelper.GetRandomPosition(m_gameMoveDirection.Value, m_bounds.Value);
                break;
            }
            case GameMoveDirectionEnum.BottomToTop:
            {
                if (position.y > m_bounds.Max.y)
                    transform.position =
                        PositionHelper.GetRandomPosition(m_gameMoveDirection.Value, m_bounds.Value);
                break;
            }
            case GameMoveDirectionEnum.LeftToRight:
            {
                if (position.x > m_bounds.Max.x)
                    transform.position =
                        PositionHelper.GetRandomPosition(m_gameMoveDirection.Value, m_bounds.Value);
                break;
            }
            case GameMoveDirectionEnum.RightToLeft:
            {
                if (position.x < m_bounds.Min.x)
                    transform.position =
                        PositionHelper.GetRandomPosition(m_gameMoveDirection.Value, m_bounds.Value);
                break;
            }
        }

        RandomizeMoveDirection();
    }

    #endregion
}
