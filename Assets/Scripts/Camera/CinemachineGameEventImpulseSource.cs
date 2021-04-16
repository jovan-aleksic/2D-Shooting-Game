using Cinemachine;
using UnityEngine;

public class CinemachineGameEventImpulseSource : CinemachineImpulseSource
{
    [SerializeField] private CodedGameEventListener cameraShakeEvent;

    private void Awake()
    {
        cameraShakeEvent.Init(gameObject, GenerateImpulse);
    }
}