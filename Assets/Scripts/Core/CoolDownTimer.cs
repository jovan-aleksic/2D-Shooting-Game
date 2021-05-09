using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// A simple cool down timer.
/// </summary>
[System.Serializable]
public class CoolDownTimer
{
    private float m_coolDownTime;

    public float minCoolDownTime = 0.5f;

    public float maxCoolDownTime = 0.5f;

    public bool IsActive { get; private set; }

    public FloatReference percentageCompleted;

    public FloatReference coolDownTimeLeft;

    private float m_completeTime;

    private float m_startTime;

    public void OnDisabled()
    {
        IsActive = false;
    }

    public IEnumerator CoolDown()
    {
        IsActive = true;
        m_startTime = Time.time;
        m_coolDownTime = Random.Range(minCoolDownTime, maxCoolDownTime);
        m_completeTime = m_startTime + m_coolDownTime;

        while (Time.time < m_completeTime)
        {
            // percentage completed 0 - 1 = current / maximum
            percentageCompleted.Value = math.min((Time.time - m_startTime) / m_coolDownTime, 1f);
            coolDownTimeLeft.Value = math.max(m_completeTime - Time.time, 0f);
            yield return null;
        }

        percentageCompleted.Value = 1;

        IsActive = false;
    }
}
