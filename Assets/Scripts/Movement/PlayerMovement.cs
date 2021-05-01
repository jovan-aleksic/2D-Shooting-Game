using UnityEngine;
using Debug = UnityEngine.Debug;

public class PlayerMovement : Moveable
{
    [Header("Inputs")] [InputAxis] [SerializeField]
    private string horizontalInput = "Horizontal";

    [InputAxis] [SerializeField] private string verticalInput = "Vertical";
    [InputAxis] [SerializeField] private string thrusterInput = "Fire3";

    [Header("Speed Boost")] [SerializeField] [Range(1, 10)]
    private float boostSpeedAmount = 2.0f;

    [SerializeField] private CoolDownTimer speedBoostActiveTimer;

    [SerializeField] private CodedGameEventListener speedBoostPowerUp;

    private float m_boostSpeed;

    [Header("Thrusters")] [SerializeField] private Vector2 thrusterSpeedAmount = new Vector2(0, 0.5f);

    private Vector2 m_thrusterSpeed = Vector2.zero;
    [SerializeField] private GameObject thrustersVisual;

    private bool m_hasThrustersVisual;

    [SerializeField] private DischargeRechargeTimer thrusterUsageTimer;

    private void Start()
    {
        if (bounds is { }) transform.position = new Vector3(bounds.Value.center.x, bounds.Min.y + 0.1f);

        Debug.Assert(thrusterUsageTimer != null, nameof(thrusterUsageTimer) + " != null");
        thrusterUsageTimer.InitTimer(this);

        m_hasThrustersVisual = thrustersVisual != null;
        if (!m_hasThrustersVisual) return;
        Debug.Assert(thrustersVisual != null, nameof(thrustersVisual) + " != null");
        thrustersVisual.SetActive(false);
    }

    private void OnDisable()
    {
        Debug.Assert(speedBoostPowerUp != null, nameof(speedBoostPowerUp) + " != null");
        speedBoostPowerUp.OnDisable();
    }

    private void OnEnable()
    {
        Debug.Assert(speedBoostPowerUp != null, nameof(speedBoostPowerUp) + " != null");
        speedBoostPowerUp.OnEnable(ActivateSpeedBoost);
    }

    #region Overrides of Moveable

    /// <inheritdoc />
    protected override void SetMoveDirection()
    {
        // If the Fire 3 button was pressed this frame
        if (Input.GetButtonDown(thrusterInput) && thrusterUsageTimer is {CanDischarge: true})
        {
            thrusterUsageTimer.StartDischarging();
            if (m_hasThrustersVisual)
            {
                Debug.Assert(thrustersVisual != null, nameof(thrustersVisual) + " != null");
                thrustersVisual.SetActive(true);
            }

            m_thrusterSpeed = thrusterSpeedAmount;
        }

        // If the Fire 3 Button is being held down this frame
        if (Input.GetButton(thrusterInput))
        {
            if (thrusterUsageTimer is {CanDischarge: false})
            {
                if (m_hasThrustersVisual)
                {
                    Debug.Assert(thrustersVisual != null, nameof(thrustersVisual) + " != null");
                    thrustersVisual.SetActive(false);
                }

                m_thrusterSpeed = Vector2.zero;
                thrusterUsageTimer.StartRecharging();
            }
        }

        // If the Fire 3 Button was released this frame
        if (Input.GetButtonUp(thrusterInput))
        {
            Debug.Assert(thrusterUsageTimer != null, nameof(thrusterUsageTimer) + " != null");
            thrusterUsageTimer.StartRecharging();
            if (m_hasThrustersVisual)
            {
                Debug.Assert(thrustersVisual != null, nameof(thrustersVisual) + " != null");
                thrustersVisual.SetActive(false);
            }

            m_thrusterSpeed = Vector2.zero;
        }

        m_boostSpeed = speedBoostActiveTimer is {IsActive: true} ? boostSpeedAmount : 1;
        moveDirection = new Vector3(Input.GetAxis(horizontalInput) * (m_boostSpeed + m_thrusterSpeed.x),
                                    Input.GetAxis(verticalInput) * (m_boostSpeed + m_thrusterSpeed.y));
    }

    /// <inheritdoc />
    protected override void CheckBounds()
    {
        Vector3 position = transform.position;

        if (bounds is { })
        {
            position.y = Mathf.Clamp(position.y, bounds.Min.y + 0.0001f, bounds.Max.y - 0.0001f);

            if (position.x > bounds.Max.x)
                position.x = bounds.Min.x + 0.1f;

            if (position.x < bounds.Min.x)
                position.x = bounds.Max.x - 0.1f;
        }

        transform.position = position;
    }

    #endregion

    private void ActivateSpeedBoost()
    {
        Debug.Assert(speedBoostActiveTimer != null, nameof(speedBoostActiveTimer) + " != null");
        StartCoroutine(speedBoostActiveTimer.CoolDown());
    }
}
