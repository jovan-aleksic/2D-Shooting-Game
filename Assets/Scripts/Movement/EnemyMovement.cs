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
        if (gameMoveDirection == null) return;

        float x = moveDirection.x;
        float y = moveDirection.y;
        float z = moveDirection.z;
        float t = Time.time * m_frequency;

        switch (gameMoveDirection.Value)
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
        if (gameMoveDirection == null || bounds == null) return;

        Vector3 position = transform.position;

        switch (gameMoveDirection.Value)
        {
            case GameMoveDirectionEnum.TopToBottom:
            {
                if (position.y < bounds.Min.y)
                    transform.position =
                        PositionHelper.GetRandomPosition(gameMoveDirection.Value, bounds.Value);
                break;
            }
            case GameMoveDirectionEnum.BottomToTop:
            {
                if (position.y > bounds.Max.y)
                    transform.position =
                        PositionHelper.GetRandomPosition(gameMoveDirection.Value, bounds.Value);
                break;
            }
            case GameMoveDirectionEnum.LeftToRight:
            {
                if (position.x > bounds.Max.x)
                    transform.position =
                        PositionHelper.GetRandomPosition(gameMoveDirection.Value, bounds.Value);
                break;
            }
            case GameMoveDirectionEnum.RightToLeft:
            {
                if (position.x < bounds.Min.x)
                    transform.position =
                        PositionHelper.GetRandomPosition(gameMoveDirection.Value, bounds.Value);
                break;
            }
        }

        RandomizeMoveDirection();
    }

    #endregion
}
