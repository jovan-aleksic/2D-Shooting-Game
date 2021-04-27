using UnityEngine;
using UnityEngine.UI;

public class CoolDownTimerDisplay : MonoBehaviour
{
    private Slider m_slider;

    public FloatReference coolDownTimer;

    private void Start()
    {
        m_slider = GetComponent<Slider>();

        if (m_slider == null || coolDownTimer == null)
        {
            gameObject.SetActive(false);
            return;
        }

        coolDownTimer.Value = 0;
    }

    private void Update()
    {
        m_slider.value = coolDownTimer.Value;
    }
}
