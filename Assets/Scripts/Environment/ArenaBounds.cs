using UnityEngine;

public class ArenaBounds : MonoBehaviour
{
    [SerializeField] private Vector2 size =
        new Vector2(80f, 80f);

    [SerializeField] private float cornerRadius = 8f;

    [SerializeField] private float spawnMargin = 1.5f;

    public bool Contains(Vector3 worldPosition)
    {
        Vector3 local =
            transform.InverseTransformPoint(worldPosition);

        float halfWidth =
            size.x * 0.5f - spawnMargin;

        float halfDepth =
            size.y * 0.5f - spawnMargin;

        float radius =
            Mathf.Max(
                0f,
                cornerRadius - spawnMargin
            );

        float straightHalfWidth =
            halfWidth - radius;

        float straightHalfDepth =
            halfDepth - radius;

        float dx = Mathf.Max(
            Mathf.Abs(local.x) - straightHalfWidth,
            0f
        );

        float dz = Mathf.Max(
            Mathf.Abs(local.z) - straightHalfDepth,
            0f
        );

        return
            dx * dx + dz * dz <= radius * radius;
    }
}