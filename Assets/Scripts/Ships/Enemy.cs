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
        transform.position = PositionHelper.GetRandomPosition(gameMoveDirectionVariable.Value, bounds.Value);
    }
}
