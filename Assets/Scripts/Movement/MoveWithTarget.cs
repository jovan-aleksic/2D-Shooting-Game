using UnityEngine;

public class MoveWithTarget : MonoBehaviour
{
    [SerializeField] private Transform m_target;

    private Vector3 m_offset = Vector3.zero;

    private void Start()
    {
        if (m_target != null) return;
        enabled = false;
    }

    public void SetTarget(Transform target)
    {
        m_target = target;
        Debug.Assert(m_target != null, nameof(m_target) + " != null");
        m_offset = m_target.position - transform.position;
        enabled = true;
    }

    private void Update()
    {
        Debug.Assert(m_target != null, nameof(m_target) + " != null");
        transform.position = m_target.position + m_offset;
    }
}
