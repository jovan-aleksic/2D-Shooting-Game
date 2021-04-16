using System;
using UnityEngine;

public class Enemy : Ship
{
    [Space(5)]
    [Header("Enemy")]
    [Space(3)]
    [SerializeField]
    protected GameMoveDirectionVariable gameMoveDirectionVariable;

    #region Overrides of Ship

    /// <inheritdoc />
    protected override void Awake()
    {
        base.Awake();

        StartCoroutine(fireDelayTimer.CoolDown());
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
                if (position.y < bounds.Min.y)
                    transform.position =
                        PositionHelper.GetRandomPosition(gameMoveDirectionVariable.Value, bounds.Value);
                break;
            case GameMoveDirectionEnum.BottomToTop:
                if (position.y > bounds.Max.y)
                    transform.position =
                        PositionHelper.GetRandomPosition(gameMoveDirectionVariable.Value, bounds.Value);
                break;
            case GameMoveDirectionEnum.LeftToRight:
                if (position.x > bounds.Max.x)
                    transform.position =
                        PositionHelper.GetRandomPosition(gameMoveDirectionVariable.Value, bounds.Value);
                break;
            case GameMoveDirectionEnum.RightToLeft:
                if (position.x < bounds.Min.x)
                    transform.position =
                        PositionHelper.GetRandomPosition(gameMoveDirectionVariable.Value, bounds.Value);
                break;
        }
    }
}
