using UnityEngine;
using UnityEngine.Events;

public class Player : Ship
{
    [Header("Player")]
    [Space(10)]
    [SerializeField]
    private Vector3Variable movementDirection;

    [Header("Receive Damage")]
    [SerializeField] private IntReference lives;

    [SerializeField] private GameObject rightEngine;
    [SerializeField] private GameObject leftEngine;

    private bool m_rightEngineEnabled, m_leftEngineEnabled, m_hasEngines;

    [Header("Triple Shot")]
    [SerializeField]
    private GameObject tripleShotPrefab;

    [SerializeField] private CoolDownTimer tripleShotActiveTimer;

    [SerializeField] GameEvent tripleShotPowerUpCollected;
    private GameEventListener m_tripleShotEventListener;

    [SerializeField] private SoundEffect tripleShotFireSoundEffect;
    private bool m_hasTripleShotFireSoundEffect;

    [Header("Speed Boost")]
    [SerializeField]
    [Range(1, 10)]
    private float boostSpeedAmount = 2.0f;

    [SerializeField] private CoolDownTimer speedBoostActiveTimer;

    [SerializeField] private GameEvent speedBoostPowerUpCollected;
    private GameEventListener m_speedBoostEventListener;

    [Header("Shields")]
    [SerializeField]
    private GameEvent shieldPowerUpCollected;

    private GameEventListener m_shieldEventListener;

    [SerializeField] private GameObject shield;
    private bool m_hasShields;


    #region Unity Methods

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    private void Start()
    {
        transform.position = new Vector3(bounds.Value.center.x, bounds.Min.y + 0.1f);

        if (shield != null)
        {
            m_hasShields = true;
            shield.SetActive(false);
        }

        m_hasTripleShotFireSoundEffect = tripleShotFireSoundEffect != null;

        if (rightEngine == null || leftEngine == null)
            return;

        m_hasEngines = true;
        DisableLeftEngine();
        DisableRightEngine();
    }

    #endregion

    #region Overrides of Ship

    /// <inheritdoc />
    protected override void Awake()
    {
        base.Awake();

        m_tripleShotEventListener = gameObject.AddComponent<GameEventListener>();
        m_tripleShotEventListener.response = new UnityEvent();
        m_tripleShotEventListener.response.AddListener(ActivateTripleShot);
        m_tripleShotEventListener.@event = tripleShotPowerUpCollected;
        m_tripleShotEventListener.@event.RegisterListener(m_tripleShotEventListener);

        m_speedBoostEventListener = gameObject.AddComponent<GameEventListener>();
        m_speedBoostEventListener.response = new UnityEvent();
        m_speedBoostEventListener.response.AddListener(ActivateSpeedBoost);
        m_speedBoostEventListener.@event = speedBoostPowerUpCollected;
        m_speedBoostEventListener.@event.RegisterListener(m_speedBoostEventListener);

        m_shieldEventListener = gameObject.AddComponent<GameEventListener>();
        m_shieldEventListener.response = new UnityEvent();
        m_shieldEventListener.response.AddListener(ActivateShields);
        m_shieldEventListener.@event = shieldPowerUpCollected;
        m_shieldEventListener.@event.RegisterListener(m_shieldEventListener);
    }

    /// <inheritdoc />
    protected override void Update()
    {
        SetMoveDirection();
        base.Update();
    }

    /// <inheritdoc />
    protected override bool ShouldFireLaser()
    {
        return Input.GetButton("Fire1");
    }

    /// <inheritdoc />
    protected override void FireLaser()
    {
        if (fireDelayTimer.IsActive) return;

        if (!tripleShotActiveTimer.IsActive || tripleShotPrefab == null)
        {
            base.FireLaser();
            return;
        }

        StartCoroutine(fireDelayTimer.CoolDown());
        Instantiate(tripleShotPrefab, transform.position + laserOffset, Quaternion.identity, projectileContainer);

        if (m_hasTripleShotFireSoundEffect)
            tripleShotFireSoundEffect.Play();
    }

    /// <summary>
    /// Set the move direction so the ship moves.
    /// </summary>
    private void SetMoveDirection()
    {
        float speed = speedBoostActiveTimer.IsActive ? boostSpeedAmount : 1;
        movementDirection.SetValue(Input.GetAxis("Horizontal") * speed, Input.GetAxis("Vertical") * speed);
    }

    #endregion

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

    private void ActivateTripleShot()
    {
        StartCoroutine(tripleShotActiveTimer.CoolDown());
    }

    private void ActivateSpeedBoost()
    {
        StartCoroutine(speedBoostActiveTimer.CoolDown());
    }

    private void ActivateShields()
    {
        if (m_hasShields) shield.SetActive(true);
    }

    public void ReceivedDamage()
    {
        if (!m_hasEngines) return;

        if (lives.Value == 2)
        {
            int selectedEngine = Random.Range(0, 100);
            if (selectedEngine < 50)
                EnableRightEngine();
            else
                EnableLeftEngine();
        }
        else if (lives == 1)
        {
            if (m_rightEngineEnabled)
                EnableLeftEngine();
            else
                EnableRightEngine();
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
}
