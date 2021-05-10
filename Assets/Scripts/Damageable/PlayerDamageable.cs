using UnityEngine;
using Debug = System.Diagnostics.Debug;

public class PlayerDamageable : Damageable
{
    [Header("Player Damageable", 2f, "orange")]
    // ReSharper disable once MissingLinebreak
    [SerializeField] private GameObject rightEngine;

    [SerializeField] private GameObject leftEngine;

    private bool m_rightEngineEnabled, m_leftEngineEnabled, m_hasEngines;

    [Header("Heal Power Up")] [SerializeField]
    private CodedGameEventListener lifePowerUp;

    [Header("Shields")] [SerializeField] private CodedGameEventListener shieldPowerUp;

    private void OnDisable()
    {
        if (lifePowerUp != null) lifePowerUp.OnDisable();
        if (shieldPowerUp != null) shieldPowerUp.OnDisable();
    }

    private void OnEnable()
    {
        if (lifePowerUp != null) lifePowerUp.OnEnable(LifeCollected);
        if (shieldPowerUp != null) shieldPowerUp.OnEnable(ActivateShields);
    }


    #region Overrides of Damageable

    /// <inheritdoc />
    protected override void Start()
    {
        base.Start();

        Debug.Assert(shieldHealth != null, nameof(shieldHealth) + " != null");
        shieldHealth.Remove(shieldHealth.Max);

        if (rightEngine == null || leftEngine == null)
            return;

        m_hasEngines = true;

        if (leftEngine != null)
            DisableLeftEngine();
        if (rightEngine != null)
            DisableRightEngine();
    }

    /// <inheritdoc />
    protected override void Damage()
    {
        base.Damage();

        ReceivedDamage();
    }

    #endregion

    #region Power Up Activation

    private void ActivateShields()
    {
        Debug.Assert(shieldHealth != null, nameof(shieldHealth) + " != null");
        shieldHealth.ResetStat();
    }

    private void LifeCollected()
    {
        Debug.Assert(m_lives != null, nameof(m_lives) + " != null");
        m_lives.Add(1);

        if (!m_hasEngines) return;

        switch (m_lives.Value)
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

    private void ReceivedDamage()
    {
        if (!m_hasEngines) return;

        Debug.Assert(shieldHealth != null, nameof(shieldHealth) + " != null");
        if (shieldHealth.Value > 0) return;

        Debug.Assert(m_lives != null, nameof(m_lives) + " != null");
        switch (m_lives.Value)
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
        Debug.Assert(leftEngine != null, nameof(leftEngine) + " != null");
        leftEngine.SetActive(true);
        m_leftEngineEnabled = true;
    }

    private void EnableRightEngine()
    {
        Debug.Assert(rightEngine != null, nameof(rightEngine) + " != null");
        rightEngine.SetActive(true);
        m_rightEngineEnabled = true;
    }

    private void DisableLeftEngine()
    {
        Debug.Assert(leftEngine != null, nameof(leftEngine) + " != null");
        leftEngine.SetActive(false);
        m_leftEngineEnabled = false;
    }

    private void DisableRightEngine()
    {
        Debug.Assert(rightEngine != null, nameof(rightEngine) + " != null");
        rightEngine.SetActive(false);
        m_rightEngineEnabled = false;
    }

    #endregion
}
