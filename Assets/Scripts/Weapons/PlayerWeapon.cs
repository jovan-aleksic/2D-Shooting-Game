using UnityEngine;

public class PlayerWeapon : Weapon
{
    [Header("Player Weapon", 2f, "orange")] [SerializeField]
    private StatVariable ammoCount;

    [SerializeField] private CodedGameEventListener ammoPowerUp;

    // ReSharper disable once RedundantBlankLines
    [InputAxis] [SerializeField] private string fireLaserInput = "Fire1";

    [Header("Triple Shot")] [SerializeField]
    private GameObject tripleShotPrefab;

    private bool m_hasTripleShotPrefab;

    [SerializeField] private CoolDownTimer tripleShotActiveTimer;
    [SerializeField] private CodedGameEventListener tripleShotPowerUp;

    [SerializeField] private SoundEffect tripleShotFireSoundEffect;
    private bool m_hasTripleShotFireSoundEffect;

    [Header("Homing Laser")] [SerializeField]
    private GameObject homingLaserPrefab;

    [SerializeField] private CoolDownTimer homingLaserActiveTimer;

    private bool m_hasHomingLaserPrefab;
    [SerializeField] private CodedGameEventListener homingLaserPowerUp;

    [SerializeField] private SoundEffect homingLaserFireSoundEffect;
    private bool m_hasHomingLaserFireSoundEffect;

    #region Overrides of Weapon

    /// <inheritdoc />
    protected override void Start()
    {
        if (ammoCount != null) ammoCount.ResetStat();

        m_hasTripleShotFireSoundEffect = tripleShotFireSoundEffect != null;
        m_hasTripleShotPrefab = tripleShotPrefab != null;
        m_hasHomingLaserFireSoundEffect = homingLaserFireSoundEffect != null;
        m_hasHomingLaserPrefab = homingLaserPrefab != null;
    }

    /// <inheritdoc />
    protected override bool ShouldFireLaser()
    {
        // The Player Can only fire if they are pressing the Fire Button and They have Ammo to Fire.
        return Input.GetButton(fireLaserInput) && ammoCount != null && ammoCount.Value > 0;
    }

    /// <inheritdoc />
    protected override void FireLaser()
    {
        Debug.Assert(fireDelayTimer != null, nameof(fireDelayTimer) + " != null");
        if (fireDelayTimer.IsActive) return;

        if (ammoCount != null) ammoCount.Remove(1);

        Debug.Assert(tripleShotActiveTimer != null, nameof(tripleShotActiveTimer) + " != null");
        Debug.Assert(homingLaserActiveTimer != null, nameof(homingLaserActiveTimer) + " != null");
        if (!tripleShotActiveTimer.IsActive && !homingLaserActiveTimer.IsActive)
        {
            base.FireLaser();
            return;
        }

        if (tripleShotActiveTimer.IsActive)
        {
            if (m_hasTripleShotPrefab)
            {
                StartCoroutine(fireDelayTimer.CoolDown());
                Instantiate(tripleShotPrefab, transform.position + laserOffset, Quaternion.identity);
                if (m_hasTripleShotFireSoundEffect)
                {
                    Debug.Assert(tripleShotFireSoundEffect != null, nameof(tripleShotFireSoundEffect) + " != null");
                    tripleShotFireSoundEffect.Play();
                }
            }
            else if (!homingLaserActiveTimer.IsActive)
            {
                base.FireLaser();
                return;
            }
        }

        Debug.Assert(homingLaserActiveTimer != null, nameof(homingLaserActiveTimer) + " != null");
        if (homingLaserActiveTimer.IsActive)
        {
            if (m_hasHomingLaserPrefab)
            {
                Debug.Assert(fireDelayTimer != null, nameof(fireDelayTimer) + " != null");
                StartCoroutine(fireDelayTimer.CoolDown());
                Instantiate(homingLaserPrefab, transform.position + laserOffset, Quaternion.identity);
                if (!m_hasHomingLaserFireSoundEffect) return;
                Debug.Assert(homingLaserFireSoundEffect != null, nameof(homingLaserFireSoundEffect) + " != null");
                homingLaserFireSoundEffect.Play();
            }
            else
            {
                Debug.Assert(tripleShotActiveTimer != null, nameof(tripleShotActiveTimer) + " != null");
                if (!tripleShotActiveTimer.IsActive)
                {
                    base.FireLaser();
                }
            }
        }
    }

    #endregion

    private void OnDisable()
    {
        if (ammoPowerUp != null) ammoPowerUp.OnDisable();
        if (tripleShotPowerUp != null) tripleShotPowerUp.OnDisable();
        if (homingLaserPowerUp != null) homingLaserPowerUp.OnDisable();
    }

    private void OnEnable()
    {
        if (ammoPowerUp != null) ammoPowerUp.OnEnable(AmmoCollected);
        if (tripleShotPowerUp != null) tripleShotPowerUp.OnEnable(ActivateTripleShot);
        if (homingLaserPowerUp != null) homingLaserPowerUp.OnEnable(ActivateHomingLaser);
    }

    #region Power Up Activation

    private void AmmoCollected()
    {
        if (ammoCount != null) ammoCount.ResetStat();
    }

    private void ActivateTripleShot()
    {
        Debug.Log("Triple Shoot Activated");
        Debug.Assert(tripleShotActiveTimer != null, nameof(tripleShotActiveTimer) + " != null");
        StartCoroutine(tripleShotActiveTimer.CoolDown());
    }

    private void ActivateHomingLaser()
    {
        Debug.Assert(homingLaserActiveTimer != null, nameof(homingLaserActiveTimer) + " != null");
        StartCoroutine(homingLaserActiveTimer.CoolDown());
    }

    #endregion
}
