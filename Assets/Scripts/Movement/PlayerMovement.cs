using UnityEngine;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

public class PlayerMovement : Moveable
{
    [Header("Inputs")] [InputAxis] [SerializeField]
    private string m_horizontalInput = "Horizontal";

    [InputAxis] [SerializeField] private string m_verticalInput = "Vertical";
    [InputAxis] [SerializeField] private string m_thrusterInput = "Fire3";

    [Header("Speed Boost")] [SerializeField] [Range(1, 10)]
    private float m_boostSpeedAmount = 2.0f;

    [SerializeField] private CoolDownTimer m_speedBoostActiveTimer;

    [SerializeField] private CodedGameEventListener m_speedBoostPowerUp;

    private float m_boostSpeed;

    [Header("Thrusters")] [SerializeField] private Vector2 m_thrusterSpeedAmount = new Vector2(0, 0.5f);

    private Vector2 m_thrusterSpeed = Vector2.zero;
    [SerializeField] private GameObject m_thrustersVisual;

    private bool m_hasThrustersVisual;

    [SerializeField] private DischargeRechargeTimer m_thrusterUsageTimer;

    private void Start()
    {
        if (m_bounds is { }) transform.position = new Vector3(m_bounds.Value.center.x, m_bounds.Min.y + 0.1f);

        Debug.Assert(m_thrusterUsageTimer != null, nameof(m_thrusterUsageTimer) + " != null");
        m_thrusterUsageTimer.InitTimer(this);

        m_hasThrustersVisual = m_thrustersVisual != null;
        if (!m_hasThrustersVisual) return;
        Debug.Assert(m_thrustersVisual != null, nameof(m_thrustersVisual) + " != null");
        m_thrustersVisual.SetActive(false);
    }

    private void OnDisable()
    {
        Debug.Assert(m_speedBoostPowerUp != null, nameof(m_speedBoostPowerUp) + " != null");
        m_speedBoostPowerUp.OnDisable();
        m_speedBoostActiveTimer.OnDisabled();
    }

    private void OnEnable()
    {
        Debug.Assert(m_speedBoostPowerUp != null, nameof(m_speedBoostPowerUp) + " != null");
        m_speedBoostPowerUp.OnEnable(ActivateSpeedBoost);
    }

    #region Overrides of Moveable

    /// <inheritdoc />
    protected override void SetMoveDirection()
    {
        // If the Fire 3 button was pressed this frame
        if (Input.GetButtonDown(m_thrusterInput) && m_thrusterUsageTimer is {CanDischarge: true})
        {
            m_thrusterUsageTimer.StartDischarging();
            if (m_hasThrustersVisual)
            {
                Debug.Assert(m_thrustersVisual != null, nameof(m_thrustersVisual) + " != null");
                m_thrustersVisual.SetActive(true);
            }

            m_thrusterSpeed = m_thrusterSpeedAmount;
        }

        // If the Fire 3 Button is being held down this frame
        if (Input.GetButton(m_thrusterInput))
        {
            if (m_thrusterUsageTimer is {CanDischarge: false})
            {
                if (m_hasThrustersVisual)
                {
                    Debug.Assert(m_thrustersVisual != null, nameof(m_thrustersVisual) + " != null");
                    m_thrustersVisual.SetActive(false);
                }

                m_thrusterSpeed = Vector2.zero;
                m_thrusterUsageTimer.StartRecharging();
            }
        }

        // If the Fire 3 Button was released this frame
        if (Input.GetButtonUp(m_thrusterInput))
        {
            Debug.Assert(m_thrusterUsageTimer != null, nameof(m_thrusterUsageTimer) + " != null");
            m_thrusterUsageTimer.StartRecharging();
            if (m_hasThrustersVisual)
            {
                Debug.Assert(m_thrustersVisual != null, nameof(m_thrustersVisual) + " != null");
                m_thrustersVisual.SetActive(false);
            }

            m_thrusterSpeed = Vector2.zero;
        }

        m_boostSpeed = m_speedBoostActiveTimer is {IsActive: true} ? m_boostSpeedAmount : 1;
        moveDirection = new Vector3(Input.GetAxis(m_horizontalInput) * (m_boostSpeed + m_thrusterSpeed.x),
                                    Input.GetAxis(m_verticalInput) * (m_boostSpeed + m_thrusterSpeed.y));
    }

    /// <inheritdoc />
    protected override void CheckBounds()
    {
        Vector3 position = transform.position;

        if (m_bounds is { })
        {
            position.y = Mathf.Clamp(position.y, m_bounds.Min.y + 0.0001f, m_bounds.Max.y - 0.0001f);

            if (position.x > m_bounds.Max.x)
                position.x = m_bounds.Min.x + 0.1f;

            if (position.x < m_bounds.Min.x)
                position.x = m_bounds.Max.x - 0.1f;
        }

        transform.position = position;
    }

    #endregion

    private void ActivateSpeedBoost()
    {
        Debug.Assert(m_speedBoostActiveTimer != null, nameof(m_speedBoostActiveTimer) + " != null");
        StartCoroutine(m_speedBoostActiveTimer.CoolDown());
    }
}
