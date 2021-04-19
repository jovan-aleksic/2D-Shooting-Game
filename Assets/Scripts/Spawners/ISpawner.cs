using UnityEngine;

public interface ISpawner
{
    void Init(Transform container, MonoBehaviour monoBehaviour);

    void Start();

    void Stop();

    void DestroyAllSpawnedObjects();
}
