using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class FloatTextDisplay : MonoBehaviour
{
    private Text m_text;

    [SerializeField] private string message = "";
    [SerializeField] private FloatReference floatReference;

    private void Start() => m_text = GetComponent<Text>();

    private void Update() => m_text.text = $"{message} {floatReference.Value:00.00}";
}
