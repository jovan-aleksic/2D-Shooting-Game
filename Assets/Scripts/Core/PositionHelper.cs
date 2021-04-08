using System;
using UnityEngine;
using Random = UnityEngine.Random;

public static class PositionHelper
{
    public static Vector3 GetRandomPosition(GameMoveDirectionEnum gameMoveDirection, Bounds screenBounds)
    {
        return gameMoveDirection switch
        {
            GameMoveDirectionEnum.TopToBottom => new Vector3(
                Random.Range(screenBounds.min.x + 0.00001f, screenBounds.max.x),
                screenBounds.max.y - 0.0001f,
                0),
            GameMoveDirectionEnum.BottomToTop => new Vector3(
                Random.Range(screenBounds.min.x + 0.00001f, screenBounds.max.x),
                screenBounds.min.y + 0.0001f,
                0),
            GameMoveDirectionEnum.LeftToRight => new Vector3(screenBounds.min.x + 0.0001f, Random.Range(
                                                                 screenBounds.min.y + 0.00001f, screenBounds.max.y),
                                                             0),
            GameMoveDirectionEnum.RightToLeft => new Vector3(screenBounds.max.x - 0.0001f, Random.Range(
                                                                 screenBounds.min.y + 0.00001f, screenBounds.max.y),
                                                             0),
            _ => screenBounds.center
        };
    }

    public static Vector3 GetDirection(GameMoveDirectionEnum gameMoveDirection)
    {
        return gameMoveDirection switch
        {
            GameMoveDirectionEnum.TopToBottom => Vector3.down,
            GameMoveDirectionEnum.BottomToTop => Vector3.up,
            GameMoveDirectionEnum.LeftToRight => Vector3.right,
            GameMoveDirectionEnum.RightToLeft => Vector3.left,
            GameMoveDirectionEnum.InputSystem => Vector3.zero,
            _ => throw new ArgumentOutOfRangeException(nameof(gameMoveDirection), gameMoveDirection,
                                                       gameMoveDirection + " is not accepted.")
        };
    }
}