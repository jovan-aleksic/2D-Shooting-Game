using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public static class PhysicsHelper
{
    private static readonly ContactFilter2D ContactFilter2DTrigger = new ContactFilter2D {useTriggers = true};

    public static int BoxCast2D([NotNull] Transform transform, Vector2 offset, Vector2 size,
                                int maxHits, out RaycastHit2D[] hits)
    {
        Vector2 position = transform.position;
        position += offset;
        transform.rotation.ToAngleAxis(out float angle, out Vector3 _);
        hits = new RaycastHit2D[maxHits];
        return Physics2D.BoxCast(position, size, angle, Vector2.zero, ContactFilter2DTrigger, hits);
    }

    public static void DrawBoxCast([NotNull] Transform transform, Vector3 offset, Vector3 size,
                                   int maxHits, string[] compareTags,
                                   bool logMessage = true, bool drawHitRay = true)
    {
        Box box = new Box(transform.position, offset, size, transform.rotation);

        Debug.DrawLine(box.TopLeft, box.TopRight, Gizmos.color);
        Debug.DrawLine(box.TopRight, box.BottomRight, Gizmos.color);
        Debug.DrawLine(box.BottomRight, box.BottomLeft, Gizmos.color);
        Debug.DrawLine(box.BottomLeft, box.TopLeft, Gizmos.color);

        if (compareTags == null) return;

        int count = BoxCast2D(transform, offset, size, maxHits, out RaycastHit2D[] hits);

        if (!(count > 0) || hits == null) return;

        Vector2 position = transform.position;
        for (int i = 0; i < count; i++)
        {
            RaycastHit2D hit = hits[i];

            if (hit.collider == null) continue;

            foreach (string compareTag in compareTags)
            {
                if (string.IsNullOrEmpty(compareTag) || !hit.collider.CompareTag(compareTag)) continue;
                if (logMessage)
                    Debug.Log($"Hitting: {hit.collider.name} {hit.collider.tag}");
                if (drawHitRay)
                    Debug.DrawRay(position, -(position - hit.point), Color.red);
            }
        }
    }

    public static Transform GetFirstTargetHit([NotNull] Transform transform, Vector2 offset, Vector2 size,
                                              int maxHits, string[] compareTags)
    {
        if (compareTags == null) return null;

        int count = BoxCast2D(transform, offset, size, maxHits, out RaycastHit2D[] hits);
        if (!(count > 0) || hits == null) return null;

        for (int i = 0; i < count; i++)
        {
            RaycastHit2D hit = hits[i];

            if (hit.collider == null) continue;

            if (compareTags.Where(compareTag => !string.IsNullOrEmpty(compareTag))
                .Any(compareTag => hit.collider.CompareTag(compareTag)))
            {
                return hit.transform;
            }
        }

        return null;
    }

    private readonly struct Box
    {
        public Vector3 TopLeft { get; }
        public Vector3 TopRight { get; }
        public Vector3 BottomLeft { get; }
        public Vector3 BottomRight { get; }

        public Box(Vector3 position, Vector3 offset, Vector3 size, Quaternion rotation)
        {
            if (size.z == 0)
                size.z = 1f;
            Vector3 halfSize = size / 2f;
            Vector3 offsetPosition = position + offset;

            TopLeft = new Vector3(offsetPosition.x - halfSize.x, offsetPosition.y + halfSize.y, offsetPosition.z);
            TopRight = new Vector3(offsetPosition.x + halfSize.x, offsetPosition.y + halfSize.y, offsetPosition.z);
            BottomLeft = new Vector3(offsetPosition.x - halfSize.x, offsetPosition.y - halfSize.y, offsetPosition.z);
            BottomRight = new Vector3(offsetPosition.x + halfSize.x, offsetPosition.y - halfSize.y, offsetPosition.z);

            TopLeft = Rotate(TopLeft, rotation, position);
            TopRight = Rotate(TopRight, rotation, position);
            BottomLeft = Rotate(BottomLeft, rotation, position);
            BottomRight = Rotate(BottomRight, rotation, position);
        }

        private static Vector3 Rotate(Vector3 vector, Quaternion rotation, Vector3 pivot)
        {
            return rotation * (vector - pivot) + pivot;
        }
    }
}
