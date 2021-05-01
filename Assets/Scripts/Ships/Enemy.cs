using Debug = UnityEngine.Debug;

public class Enemy : Ship
{
    #region Overrides of Ship

    /// <inheritdoc />
    protected override void Awake()
    {
        base.Awake();

        Debug.Assert(fireDelayTimer != null, nameof(fireDelayTimer) + " != null");
        StartCoroutine(fireDelayTimer.CoolDown());
    }

    /// <inheritdoc />
    protected override bool ShouldFireLaser()
    {
        // If the fire cool down timer is not active return true.
        Debug.Assert(fireDelayTimer != null, nameof(fireDelayTimer) + " != null");
        return !fireDelayTimer.IsActive;
    }

    #endregion
}
