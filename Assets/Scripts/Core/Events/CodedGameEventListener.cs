using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[System.Serializable]
public class CodedGameEventListener
{
   [FormerlySerializedAs("powerUpCollectedEvent")] [SerializeField]
   private GameEvent @event;

   private GameEventListener m_gameEventListener;

   public void Init(GameObject gameObject, UnityAction call)
   {
      m_gameEventListener = gameObject.AddComponent<GameEventListener>();
      m_gameEventListener.response = new UnityEvent();
      m_gameEventListener.response.AddListener(call);

      m_gameEventListener.@event = @event;
      m_gameEventListener.@event.RegisterListener(m_gameEventListener);
   }
}
