using UnityEngine;

public class PlayerVisualRotator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 720f;
    [SerializeField] private float movementThreshold = 0.1f;

    [Tooltip("Użyj np. 180, jeśli model jest domyślnie zwrócony tyłem.")]
    [SerializeField] private float rotationOffsetY = 0f;

    private Rigidbody playerRigidbody;

    private void Awake()
    {
        playerRigidbody = GetComponentInParent<Rigidbody>();

        if (playerRigidbody == null)
        {
            Debug.LogError(
                $"{nameof(PlayerVisualRotator)} could not find Player Rigidbody.",
                this
            );
        }
    }

    private void Update()
    {
        if (playerRigidbody == null)
            return;

        Vector3 velocity = playerRigidbody.linearVelocity;
        velocity.y = 0f;

        if (velocity.sqrMagnitude <
            movementThreshold * movementThreshold)
        {
            return;
        }

        Vector3 direction = velocity.normalized;

        Quaternion targetRotation =
            Quaternion.LookRotation(direction, Vector3.up) *
            Quaternion.Euler(0f, rotationOffsetY, 0f);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}