using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DisplayIntVariable : MonoBehaviour
{
    private Text m_text;

    [SerializeField] private string m_message = "Score: ";
    [SerializeField] private IntReference m_intReference;

    private void Start() => m_text = GetComponent<Text>();

    private void Update() => m_text.text = m_message + m_intReference.Value;
}