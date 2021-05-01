using UnityEngine;

public class Player : Ship
{
    [Header("")] [SerializeField] private StatVariable ammoCount;
    [SerializeField] private CodedGameEventListener ammoPowerUp;

    [Header("Player", 3f)]
    [SerializeField]
    private Vector3Variable movementDirection;

    [InputAxis] [SerializeField] private string horizontalInput = "Horizontal";
    [InputAxis] [SerializeField] private string verticalInput = "Vertical";
    [InputAxis] [SerializeField] private string fireLaserInput = "Fire1";
    [InputAxis] [SerializeField] private string thrusterInput = "Fire3";

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

    [Header("Speed Boost")]
    [SerializeField]
    [Range(1, 10)]
    private float boostSpeedAmount = 2.0f;

    [SerializeField] private CoolDownTimer speedBoostActiveTimer;

    [SerializeField] private CodedGameEventListener speedBoostPowerUp;

    private float m_boostSpeed;

    [Header("Thrusters")]
    [SerializeField]
    private Vector2 thrusterSpeedAmount = new Vector2(0, 0.5f);

    private Vector2 m_thrusterSpeed = Vector2.zero;
    [SerializeField] private GameObject thrustersVisual;

    private bool m_hasThrustersVisual;

    [SerializeField] private DischargeRechargeTimer thrusterUsageTimer;

    #region Unity Methods

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    private void Start()
    {
        transform.position = new Vector3(bounds.Value.center.x, bounds.Min.y + 0.1f);

        thrusterUsageTimer.InitTimer(this);

        ammoCount.ResetStat();

        shield.Remove(shield.Max);

        m_hasThrustersVisual = thrustersVisual != null;
        if (m_hasThrustersVisual) thrustersVisual.SetActive(false);

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
        speedBoostPowerUp.OnDisable();
    }

    private void OnEnable()
    {
        ammoPowerUp.OnEnable(AmmoCollected);
        lifePowerUp.OnEnable(LifeCollected);
        shieldPowerUp.OnEnable(ActivateShields);
        tripleShotPowerUp.OnEnable(ActivateTripleShot);
        homingLaserPowerUp.OnEnable(ActivateHomingLaser);
        speedBoostPowerUp.OnEnable(ActivateSpeedBoost);
    }

    #endregion

    #region Overrides of Ship

    /// <inheritdoc />
    protected override void Update()
    {
        SetMoveDirection();
        base.Update();
    }

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

    /// <summary>
    /// Set the move direction so the ship moves.
    /// </summary>
    private void SetMoveDirection()
    {
        // If the Fire 3 button was pressed this frame
        if (Input.GetButtonDown(thrusterInput) && thrusterUsageTimer.CanDischarge)
        {
            thrusterUsageTimer.StartDischarging();
            if (m_hasThrustersVisual) thrustersVisual.SetActive(true);
            m_thrusterSpeed = thrusterSpeedAmount;
        }

        // If the Fire 3 Button is being held down this frame
        if (Input.GetButton(thrusterInput))
        {
            if (!thrusterUsageTimer.CanDischarge)
            {
                if (m_hasThrustersVisual) thrustersVisual.SetActive(false);
                m_thrusterSpeed = Vector2.zero;
                thrusterUsageTimer.StartRecharging();
            }
        }

        // If the Fire 3 Button was released this frame
        if (Input.GetButtonUp(thrusterInput))
        {
            thrusterUsageTimer.StartRecharging();
            if (m_hasThrustersVisual) thrustersVisual.SetActive(false);
            m_thrusterSpeed = Vector2.zero;
        }

        m_boostSpeed = speedBoostActiveTimer.IsActive ? boostSpeedAmount : 1;
        movementDirection.SetValue(Input.GetAxis(horizontalInput) * (m_boostSpeed + m_thrusterSpeed.x),
                                   Input.GetAxis(verticalInput) * (m_boostSpeed + m_thrusterSpeed.y));
    }

    public void OutOfBoundsAction()
    {
        Vector3 position = transform.position;

        position.y = Mathf.Clamp(position.y, bounds.Min.y + 0.0001f, bounds.Max.y - 0.0001f);

        if (position.x > bounds.Max.x)
            position.x = bounds.Min.x + 0.1f;

        if (position.x < bounds.Min.x)
            position.x = bounds.Max.x - 0.1f;

        transform.position = position;
    }

    #region Power Up Activation

    private void ActivateTripleShot()
    {
        StartCoroutine(tripleShotActiveTimer.CoolDown());
    }

    private void ActivateHomingLaser()
    {
        StartCoroutine(homingLaserActiveTimer.CoolDown());
    }

    private void ActivateSpeedBoost()
    {
        StartCoroutine(speedBoostActiveTimer.CoolDown());
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
