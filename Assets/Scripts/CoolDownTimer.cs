using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// A simple cool down timer.
/// </summary>
[System.Serializable]
public class CoolDownTimer
{
    //public float coolDownTime = 0.5f;
    private float m_coolDownTime;

    public float minCoolDownTime = 0.5f;

    public float maxCoolDownTime = 0.5f;

    public bool IsActive { get; private set; }

    public FloatReference percentageCompleted;

    private float m_completeTime;

    private float m_startTime;

    public IEnumerator CoolDown()
    {
        IsActive = true;
        m_startTime = Time.time;
        m_coolDownTime = Random.Range(minCoolDownTime, maxCoolDownTime);
        m_completeTime = m_startTime + m_coolDownTime;

        while (Time.time < m_completeTime)
        {
            // percentage completed 0 - 1 = current / maximum
            percentageCompleted.Value = Math.Min((Time.time - m_startTime) / m_coolDownTime, 1f);
            yield return null;
        }

        percentageCompleted.Value = 1;

        IsActive = false;
    }
}
