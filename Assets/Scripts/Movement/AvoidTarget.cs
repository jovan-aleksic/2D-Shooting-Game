using UnityEngine;
using Debug = System.Diagnostics.Debug;

public class AvoidTarget : EnemyMovement
{
    [SerializeField] private Vector3 m_detectorOffset = Vector3.zero;
    [SerializeField] private Vector3 m_detectorSize = Vector3.one;
    [SerializeField] private int m_maxTargets = 10;
    [Tag] [SerializeField] private string[] m_targetTag = new[] {"Player Laser"};

    private Transform m_target;
    private bool m_targetDetected;

    private void FixedUpdate()
    {
        m_target = PhysicsHelper.GetFirstTargetHit(transform, m_detectorOffset, m_detectorSize, m_maxTargets, m_targetTag);
        m_targetDetected = m_target != null;

        if (m_target == null) return;

        Debug.Assert(m_gameMoveDirection != null, nameof(m_gameMoveDirection) + " != null");
        switch (m_gameMoveDirection.Value)
        {
            case GameMoveDirectionEnum.TopToBottom:
            case GameMoveDirectionEnum.BottomToTop:
                // dodge on the x axis
                moveDirection.x = Dodge(transform.position.x, m_target.position.x);
                break;
            case GameMoveDirectionEnum.LeftToRight:
            case GameMoveDirectionEnum.RightToLeft:
                // dodge on the y axis
                moveDirection.y = Dodge(transform.position.y, m_target.position.y);
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Color color = Gizmos.color;
        Gizmos.color = Color.magenta;
        PhysicsHelper.DrawBoxCast(transform, m_detectorOffset, m_detectorSize, m_maxTargets, m_targetTag, false);
        Gizmos.color = color;
    }

    #region Overrides of EnemyMovement

    /// <inheritdoc />
    protected override void SetMoveDirection()
    {
        if (m_gameMoveDirection == null) return;

        if (m_targetDetected) return;
        base.SetMoveDirection();
    }

    #endregion

    private float Dodge(float thisAxis, float targetAxis)
    {
        // The target is to the left or below
        if (thisAxis > targetAxis)
        {
            // dodge right or up
            return 1;
        }

        // The target is to the right or above
        if (thisAxis < targetAxis)
        {
            // doge left or down
            return -1;
        }

        // target is dead center so dodge in a random direction
        return Random.value < 0.5f ? -1 : 1;
    }
}
