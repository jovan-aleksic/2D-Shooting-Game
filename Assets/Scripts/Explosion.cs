using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float destroyDelayTime = 2.8f;

    private void Start()
    {
        Destroy(gameObject, destroyDelayTime);
    }
}

