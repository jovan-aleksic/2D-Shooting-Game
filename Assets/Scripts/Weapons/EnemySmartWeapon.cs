using UnityEngine;
using UnityEngine.Serialization;

public class EnemySmartWeapon : Weapon
{
    [FormerlySerializedAs("offset")] [SerializeField]
    private Vector3 seekOffset = Vector3.zero;

    [SerializeField] private Vector3 seekSize = Vector3.one;

    [Tag] [SerializeField] private string[] seekTargetTag = new[] {"Player"};

    [SerializeField] private int maxTargets = 10;

    [FormerlySerializedAs("rearLaserProjectilePrefab")] [SerializeField] private GameObject secondaryLaserProjectilePrefab;

    [FormerlySerializedAs("rearLaserOffset")] [SerializeField] private Vector3 secondaryLaserOffset = Vector3.up;
    [SerializeField] protected CoolDownTimer rearFireDelayTimer;
    private bool m_hasSecondaryLaserPrefab;

    #region Overrides of Weapon

    /// <inheritdoc />
    protected override void Start()
    {
        base.Start();

        if (secondaryLaserProjectilePrefab != null)
            m_hasSecondaryLaserPrefab = true;
    }

    #endregion

    private void FixedUpdate()
    {
        Debug.Assert(rearFireDelayTimer != null, nameof(rearFireDelayTimer) + " != null");
        if (rearFireDelayTimer.IsActive) return;
        if (PhysicsHelper.GetFirstTargetHit(transform, seekOffset, seekSize, maxTargets, seekTargetTag) == null) return;
        if (!m_hasSecondaryLaserPrefab) return;

        StartCoroutine(rearFireDelayTimer.CoolDown());
        Instantiate(secondaryLaserProjectilePrefab, transform.position + secondaryLaserOffset, Quaternion.identity);

        if (!hasLaserSoundEffect) return;
        Debug.Assert(laserSoundEffect != null, nameof(laserSoundEffect) + " != null");
        laserSoundEffect.Play();
    }

    private void OnDrawGizmosSelected()
    {
        Color color = Gizmos.color;
        Gizmos.color = Color.yellow;
        PhysicsHelper.DrawBoxCast(transform, seekOffset, seekSize, maxTargets, seekTargetTag, false);
        Gizmos.color = color;
    }
}
