using UnityEngine;

public class Player : Ship
{
    [Header("")] [SerializeField] private StatVariable ammoCount;
    [SerializeField] private CodedGameEventListener ammoPowerUp;

    [Header("Player", 3f)]

    // ReSharper disable once RedundantBlankLines
    [InputAxis]
    [SerializeField]
    private string fireLaserInput = "Fire1";

    [Header("Receive Damage")]
    [SerializeField]
    private StatVariable lives;

    private int m_lives;

    [SerializeField] private GameObject rightEngine;
    [SerializeField] private GameObject leftEngine;

    private bool m_rightEngineEnabled, m_leftEngineEnabled, m_hasEngines;

    [Header("Heal Power Up")] [SerializeField]
    private CodedGameEventListener lifePowerUp;

    [Header("Shields")] [SerializeField] private CodedGameEventListener shieldPowerUp;

    [SerializeField] private StatVariable shield;

    [Header("Triple Shot")]
    [SerializeField]
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

    #region Unity Methods

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    private void Start()
    {
        ammoCount.ResetStat();

        shield.Remove(shield.Max);

        m_hasTripleShotFireSoundEffect = tripleShotFireSoundEffect != null;
        m_hasTripleShotPrefab = tripleShotPrefab != null;
        m_hasHomingLaserFireSoundEffect = homingLaserFireSoundEffect != null;
        m_hasHomingLaserPrefab = homingLaserPrefab != null;

        if (rightEngine == null || leftEngine == null)
            return;

        m_hasEngines = true;
        DisableLeftEngine();
        DisableRightEngine();
    }

    private void OnDisable()
    {
        ammoPowerUp.OnDisable();
        lifePowerUp.OnDisable();
        shieldPowerUp.OnDisable();
        tripleShotPowerUp.OnDisable();
        homingLaserPowerUp.OnDisable();
    }

    private void OnEnable()
    {
        ammoPowerUp.OnEnable(AmmoCollected);
        lifePowerUp.OnEnable(LifeCollected);
        shieldPowerUp.OnEnable(ActivateShields);
        tripleShotPowerUp.OnEnable(ActivateTripleShot);
        homingLaserPowerUp.OnEnable(ActivateHomingLaser);
    }

    #endregion

    #region Overrides of Ship

    /// <inheritdoc />
    protected override bool ShouldFireLaser()
    {
        // The Player Can only fire if they are pressing the Fire 1 Button and They have Ammo to Fire.
        return Input.GetButton(fireLaserInput) && ammoCount.Value > 0;
    }

    /// <inheritdoc />
    protected override void FireLaser()
    {
        if (fireDelayTimer.IsActive) return;

        ammoCount.Remove(1);

        if (!tripleShotActiveTimer.IsActive && !homingLaserActiveTimer.IsActive ||
            !m_hasTripleShotPrefab && !m_hasHomingLaserPrefab)
        {
            base.FireLaser();
            return;
        }

        if (tripleShotActiveTimer.IsActive)
        {
            if (m_hasTripleShotPrefab)
            {
                StartCoroutine(fireDelayTimer.CoolDown());
                Instantiate(tripleShotPrefab, transform.position + laserOffset, Quaternion.identity,
                            projectileContainer);
                if (m_hasTripleShotFireSoundEffect)
                    tripleShotFireSoundEffect.Play();
            }
            else if (!homingLaserActiveTimer.IsActive)
            {
                base.FireLaser();
                return;
            }
        }

        if (homingLaserActiveTimer.IsActive)
        {
            if (m_hasHomingLaserPrefab)
            {
                StartCoroutine(fireDelayTimer.CoolDown());
                Instantiate(homingLaserPrefab, transform.position + laserOffset, Quaternion.identity,
                            projectileContainer);
                if (m_hasHomingLaserFireSoundEffect)
                    homingLaserFireSoundEffect.Play();
            }
            else if (!tripleShotActiveTimer.IsActive)
            {
                base.FireLaser();
            }
        }
    }

    #endregion

    #region Power Up Activation

    private void ActivateTripleShot()
    {
        StartCoroutine(tripleShotActiveTimer.CoolDown());
    }

    private void ActivateHomingLaser()
    {
        StartCoroutine(homingLaserActiveTimer.CoolDown());
    }

    private void ActivateShields()
    {
        shield.ResetStat();
    }

    private void AmmoCollected()
    {
        ammoCount.ResetStat();
    }

    private void LifeCollected()
    {
        lives.Add(1);

        if (!m_hasEngines) return;

        m_lives = (int) lives.Value;

        switch (m_lives)
        {
            case 2:
                int selectedEngine = Random.Range(0, 100);
                if (selectedEngine < 50)
                    DisableLeftEngine();
                else
                    DisableRightEngine();
                break;
            case 3 when m_leftEngineEnabled:
                DisableLeftEngine();
                break;
            case 3:
                DisableRightEngine();
                break;
        }
    }

    #endregion

    # region Damage

    public void ReceivedDamage()
    {
        if (!m_hasEngines) return;
        m_lives = (int) lives.Value;

        switch (m_lives)
        {
            case 2:
                int selectedEngine = Random.Range(0, 100);
                if (selectedEngine < 50)
                    EnableLeftEngine();
                else
                    EnableRightEngine();
                break;
            case 1 when m_rightEngineEnabled:
                EnableLeftEngine();
                break;
            case 1:
                EnableRightEngine();
                break;
        }
    }

    private void EnableLeftEngine()
    {
        leftEngine.SetActive(true);
        m_leftEngineEnabled = true;
    }

    private void EnableRightEngine()
    {
        rightEngine.SetActive(true);
        m_rightEngineEnabled = true;
    }

    private void DisableLeftEngine()
    {
        leftEngine.SetActive(false);
        m_leftEngineEnabled = false;
    }

    private void DisableRightEngine()
    {
        rightEngine.SetActive(false);
        m_rightEngineEnabled = false;
    }

    #endregion
}
