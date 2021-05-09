using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject laserProjectilePrefab;

    [SerializeField] protected Vector3 laserOffset = Vector3.up;

    [SerializeField] protected CoolDownTimer fireDelayTimer;

    [SerializeField] protected SoundEffect laserSoundEffect;
    protected bool hasLaserSoundEffect;

    #region Unity Methods

    protected virtual void OnEnable()
    {
        Debug.Assert(fireDelayTimer != null, nameof(fireDelayTimer) + " != null");
        StartCoroutine(fireDelayTimer.CoolDown());
    }

    protected virtual void OnDisable()
    {
        Debug.Assert(fireDelayTimer != null, nameof(fireDelayTimer) + " != null");
        fireDelayTimer.OnDisabled();
    }

    protected virtual void Start()
    {
        if (laserProjectilePrefab == null)
        {
            Debug.LogError("Prefab for the laser to fire is required.", gameObject);
            return;
        }

        if (laserSoundEffect != null)
            hasLaserSoundEffect = true;
    }

    /// <summary>
    ///  Update is called once per frame.
    /// </summary>
    protected virtual void Update()
    {
        if (ShouldFireLaser())
            FireLaser();
    }

    #endregion

    protected virtual void FireLaser()
    {
        if (laserProjectilePrefab == null) return;
        Debug.Assert(fireDelayTimer != null, nameof(fireDelayTimer) + " != null");
        if (fireDelayTimer.IsActive) return;
        StartCoroutine(fireDelayTimer.CoolDown());

        Instantiate(laserProjectilePrefab, transform.position + laserOffset, Quaternion.identity);

        if (!hasLaserSoundEffect) return;
        Debug.Assert(laserSoundEffect != null, nameof(laserSoundEffect) + " != null");
        laserSoundEffect.Play();
    }

    /// <summary>
    /// Should the ship fire the laser
    /// </summary>
    /// <returns>true if the ship should fire the laser</returns>
    protected virtual bool ShouldFireLaser()
    {
        Debug.Assert(fireDelayTimer != null, nameof(fireDelayTimer) + " != null");
        return !fireDelayTimer.IsActive;
    }
}
