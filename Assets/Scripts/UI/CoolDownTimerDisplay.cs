using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolDownTimerDisplay : MonoBehaviour
{
    private Slider m_slider;

    public FloatReference coolDownTimer;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        m_slider = GetComponent<Slider>();

        if (m_slider == null || coolDownTimer == null)
        {
            gameObject.SetActive(false);
            return;
        }

        coolDownTimer.Value = 0;
    }

    /// <summary>
    ///  Update is called once per frame.
    /// </summary>
    void Update()
    {
        m_slider.value = coolDownTimer.Value;
    }
}
