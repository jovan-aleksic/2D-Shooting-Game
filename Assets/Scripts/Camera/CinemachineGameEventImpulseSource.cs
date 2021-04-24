using Cinemachine;
using UnityEngine;

public class CinemachineGameEventImpulseSource : CinemachineImpulseSource
{
    [SerializeField] private CodedGameEventListener cameraShakeEvent;

    private void OnDisable() => cameraShakeEvent.OnDisable();
    private void OnEnable() => cameraShakeEvent.OnEnable(GenerateImpulse);
}