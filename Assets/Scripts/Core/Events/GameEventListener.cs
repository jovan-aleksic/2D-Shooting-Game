#region Description

// ----------------------------------------------------------------------------
// Came from
// https://github.com/roboryantron/Unite2017
//
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

#endregion

using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [Tooltip("Event to register with.")] public GameEvent @event;

    [Tooltip("Response to invoke when Event is raised.")]
    public UnityEvent response;

    private void OnEnable()
    {
        if (@event != null) @event.RegisterListener(this);
    }

    private void OnDisable()
    {
        @event.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        response?.Invoke();
    }
}
