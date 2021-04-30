using UnityEngine;

/// <summary>
/// A ship game object. All ships are moveable and damageable so it requires these 2 components to be attached.
/// </summary>
[RequireComponent(typeof(Damageable), typeof(Moveable), typeof(Rigidbody2D))]
public abstract class Ship : MonoBehaviour
{
    [Header("Ship", 3f)]
    [SerializeField]
    protected BoundsVariable bounds;

    [Header("Laser", 2f)]
    [SerializeField]
    private GameObject laserProjectilePrefab;

    [SerializeField] protected Vector3 laserOffset = Vector3.up;

    protected Transform projectileContainer;

    [SerializeField] private StringVariable projectileContainerName;

    [SerializeField] protected CoolDownTimer fireDelayTimer;

    [SerializeField] private SoundEffect laserSoundEffect;
    private bool m_hasLaserSoundEffect;

    #region Unity Methods

    protected virtual void Awake()
    {
        if (projectileContainer != null) return;
        if (laserProjectilePrefab == null) return;
        GameObject laserContainer = GameObject.Find(projectileContainerName.Value);

        if (laserContainer == null)
        {
            Debug.LogError("Could not find a Game Object in the scene with a name of " +
                           projectileContainerName + "\nPlease make sure that it is in the scene",
                           this);
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }

        projectileContainer = laserContainer.transform;

        if (laserSoundEffect != null)
            m_hasLaserSoundEffect = true;
    }

    /// <summary>
    ///  Update is called once per frame.
    /// </summary>
    protected virtual void Update()
    {
        if (ShouldFireLaser())
            FireLaser();
    }

    /// <summary>
    /// Implement OnDrawGizmosSelected to draw a gizmo if the object is selected.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (bounds == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(bounds.Value.center, bounds.Value.size);
    }

    #endregion

    protected virtual void FireLaser()
    {
        if (laserProjectilePrefab == null) return;
        if (fireDelayTimer.IsActive) return;
        StartCoroutine(fireDelayTimer.CoolDown());

        Instantiate(laserProjectilePrefab, transform.position + laserOffset, Quaternion.identity,
                    projectileContainer);

        if (m_hasLaserSoundEffect)
            laserSoundEffect.Play();
    }

    /// <summary>
    /// Should the ship fire the laser
    /// </summary>
    /// <returns>true if the ship should fire the laser</returns>
    protected abstract bool ShouldFireLaser();
}
