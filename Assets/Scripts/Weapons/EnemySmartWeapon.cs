using UnityEngine;
using UnityEngine.Serialization;

public class EnemySmartWeapon : Weapon
{
    [SerializeField] private bool primaryWeaponEnabled = true;

    [FormerlySerializedAs("offset")] [SerializeField]
    private Vector3 seekOffset = Vector3.zero;

    [SerializeField] private Vector3 seekSize = Vector3.one;

    [Tag] [SerializeField] private string[] seekTargetTag = new[] {"Player"};

    [SerializeField] private int maxTargets = 10;

    [FormerlySerializedAs("rearLaserProjectilePrefab")] [SerializeField] private GameObject secondaryLaserProjectilePrefab;

    [FormerlySerializedAs("rearLaserOffset")] [SerializeField] private Vector3 secondaryLaserOffset = Vector3.up;
    [FormerlySerializedAs("rearFireDelayTimer")] [SerializeField] protected CoolDownTimer secondaryFireDelayTimer;
    private bool m_hasSecondaryLaserPrefab;

    #region Overrides of Weapon

    /// <inheritdoc />
    protected override void Start()
    {
        if (primaryWeaponEnabled)
            base.Start();

        if (secondaryLaserProjectilePrefab != null)
            m_hasSecondaryLaserPrefab = true;
    }

    /// <inheritdoc />
    protected override bool ShouldFireLaser()
    {
        return primaryWeaponEnabled && base.ShouldFireLaser();
    }

    #endregion

    private void FixedUpdate()
    {
        Debug.Assert(secondaryFireDelayTimer != null, nameof(secondaryFireDelayTimer) + " != null");
        if (secondaryFireDelayTimer.IsActive) return;
        if (PhysicsHelper.GetFirstTargetHit(transform, seekOffset, seekSize, maxTargets, seekTargetTag) == null) return;
        if (!m_hasSecondaryLaserPrefab) return;

        StartCoroutine(secondaryFireDelayTimer.CoolDown());
        Instantiate(secondaryLaserProjectilePrefab, transform.position + secondaryLaserOffset, Quaternion.identity);

        if (!hasLaserSoundEffect) return;
        Debug.Assert(laserSoundEffect != null, nameof(laserSoundEffect) + " != null");
        laserSoundEffect.Play();
    }

    private void OnDrawGizmosSelected()
    {
        Color color = Gizmos.color;
        Gizmos.color = Color.cyan;
        PhysicsHelper.DrawBoxCast(transform, seekOffset, seekSize, maxTargets, seekTargetTag, false);
        Gizmos.color = color;
    }
}
