using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class DischargeRechargeTimer
{
   [SerializeField] private float timeCanUse = 5f;

   [SerializeField] private FloatReference percentageLeft;

   private float m_usageTime;

   private Coroutine m_dischargeRoutine;

   private Coroutine m_rechargeRoutine;

   public bool CanDischarge => m_usageTime > 0;

   private bool m_hasBeenInit;

   private MonoBehaviour m_behaviour;

   public void InitTimer(MonoBehaviour behaviour)
   {
      if (m_hasBeenInit) return;

      m_behaviour = behaviour;
      m_usageTime = timeCanUse;
      percentageLeft.Value = 1;

      m_hasBeenInit = true;
   }

   private IEnumerator DischargeRoutine()
   {
      while (CanDischarge)
      {
         m_usageTime = Math.Max(m_usageTime - Time.deltaTime, 0f);
         percentageLeft.Value = Math.Max(m_usageTime / timeCanUse, 0f);
         yield return null;
      }

      percentageLeft.Value = 0;

      yield return null;
   }

   private IEnumerator RechargeRoutine()
   {
      m_usageTime += 0.00000000001f;
      while (m_usageTime < timeCanUse)
      {
         yield return null;
         m_usageTime = Math.Min(m_usageTime + Time.deltaTime, timeCanUse);
         percentageLeft.Value = Math.Min(m_usageTime / timeCanUse, 1f);
      }

      yield return null;
   }

   public void StartDischarging()
   {
      if (!m_hasBeenInit) return;
      if (m_dischargeRoutine != null)
         m_behaviour.StopCoroutine(m_dischargeRoutine);
      if (m_rechargeRoutine != null)
         m_behaviour.StopCoroutine(m_rechargeRoutine);

      m_dischargeRoutine = m_behaviour.StartCoroutine(DischargeRoutine());
   }

   public void StartRecharging()
   {
      if (!m_hasBeenInit) return;
      if (m_dischargeRoutine != null)
         m_behaviour.StopCoroutine(m_dischargeRoutine);
      if (m_rechargeRoutine != null)
         m_behaviour.StopCoroutine(m_rechargeRoutine);
      m_rechargeRoutine = m_behaviour.StartCoroutine(RechargeRoutine());
   }
}
