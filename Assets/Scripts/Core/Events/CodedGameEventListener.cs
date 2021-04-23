using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[System.Serializable]
public class CodedGameEventListener
{
   [SerializeField] private GameEvent @event;

   private GameEventListener m_gameEventListener;

   private bool m_isEnabled;


   public void Init(GameObject gameObject, UnityAction call)
   {
      m_gameEventListener = gameObject.AddComponent<GameEventListener>();
      m_gameEventListener.response = new UnityEvent();
      m_gameEventListener.response.AddListener(call);

      if (@event == null) return;
      m_gameEventListener.@event = @event;
      m_gameEventListener.@event.RegisterListener(m_gameEventListener);
      m_isEnabled = true;
   }

   public void OnDisable()
   {
      if(!m_isEnabled) return;
      if (@event != null && m_gameEventListener != null) @event.UnregisterListener(m_gameEventListener);
      m_isEnabled = false;
   }

   public void OnEnable()
   {
      if (m_isEnabled) return;
      if (@event != null && m_gameEventListener != null) @event.RegisterListener(m_gameEventListener);
      m_isEnabled = true;
   }
}
