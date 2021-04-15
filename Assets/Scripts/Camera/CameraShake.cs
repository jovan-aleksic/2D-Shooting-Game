using System.Collections;
using UnityEngine;

/// <summary>
/// Makes the Camera Shake when a game event happens.
/// </summary>
public class CameraShake : MonoBehaviour
{
    [SerializeField] private CodedGameEventListener cameraShakeEvent;
    [SerializeField] private CoolDownTimer shakeTimer;
    private Coroutine m_cameraShakeRoutine;

    private Vector3 m_originalPosition;

    /// <summary>
    /// The amplitude(strength) of the shake.
    /// </summary>
    [SerializeField] private float amplitude;

    /// <summary>
    /// The frequency(Speed) of the shake.
    /// </summary>
    [SerializeField] private float frequency;

    float m_tick = 0;

    Vector3 amt;

    private void Awake()
    {
        cameraShakeEvent.Init(gameObject, Response);
    }

    private void Start()
    {
        m_tick = Random.Range(-100, 100);
    }

    private void Response()
    {
        if (m_cameraShakeRoutine != null)
        {
            StopCoroutine(ShakeCamera());
            transform.localPosition = m_originalPosition;
        }

        m_cameraShakeRoutine = StartCoroutine(ShakeCamera());
    }

    private IEnumerator ShakeCamera()
    {
        m_originalPosition = transform.localPosition;

        StartCoroutine(shakeTimer.CoolDown());

        while (shakeTimer.IsActive)
        {
            amt.x = Mathf.PerlinNoise(m_tick, 0) - 0.5f;
            amt.y = Mathf.PerlinNoise(0, m_tick) - 0.5f;
            amt.z = Mathf.PerlinNoise(m_tick, m_tick) - 0.5f;

            m_tick += Time.deltaTime * frequency;

            amt = amt * amplitude;

            //transform.localPosition = m_originalPosition + Random.insideUnitSphere * amplitude;
            transform.localPosition = m_originalPosition + amt;

            yield return null;
        }

        transform.localPosition = m_originalPosition;

        yield return null;
    }
}
