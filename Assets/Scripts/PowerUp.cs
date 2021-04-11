using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PowerUp
{ 
   [SerializeField] private GameEvent powerUpCollectedEvent;
   private GameEventListener m_gameEventListener;

   public void Init(GameObject gameObject, UnityAction call)
   {
      m_gameEventListener = gameObject.AddComponent<GameEventListener>();
      m_gameEventListener.response = new UnityEvent();
      m_gameEventListener.response.AddListener(call);
      m_gameEventListener.@event = powerUpCollectedEvent;
      m_gameEventListener.@event.RegisterListener(m_gameEventListener);
   }
}
