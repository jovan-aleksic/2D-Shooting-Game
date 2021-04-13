using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class FloatSliderDisplay : MonoBehaviour
{
    private Slider m_slider;

    [SerializeField] private FloatReference floatReference;

    private void Awake()
    {
        m_slider = GetComponent<Slider>();
    }
    
    private void Update()
    {
        m_slider.value = floatReference.Value;
    }
}
