using UnityEngine;

public class Player : Ship
{
    [Header("Player", 3f)]

    // ReSharper disable once RedundantBlankLines
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

    #region Unity Methods

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    private void Start()
    {
        shield.Remove(shield.Max);

        if (rightEngine == null || leftEngine == null)
            return;

        m_hasEngines = true;
        DisableLeftEngine();
        DisableRightEngine();
    }

    private void OnDisable()
    {
        lifePowerUp.OnDisable();
        shieldPowerUp.OnDisable();
    }

    private void OnEnable()
    {
        lifePowerUp.OnEnable(LifeCollected);
        shieldPowerUp.OnEnable(ActivateShields);
    }

    #endregion

    #region Power Up Activation

    private void ActivateShields()
    {
        shield.ResetStat();
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
