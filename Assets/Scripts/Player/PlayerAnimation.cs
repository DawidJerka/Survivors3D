using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    private static readonly int SpeedHash =
        Animator.StringToHash("Speed");

    private static readonly int RunSpeedMultiplierHash =
        Animator.StringToHash("RunSpeedMultiplier");

    [SerializeField] private float movementThreshold = 0.1f;

    [Header("Animation")]
    [SerializeField] private float referenceRunSpeed = 5f;
    [SerializeField] private float minRunAnimationSpeed = 0.6f;
    [SerializeField] private float maxRunAnimationSpeed = 1.8f;

    private Animator animator;
    private Rigidbody playerRigidbody;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerRigidbody = GetComponentInParent<Rigidbody>();

        if (playerRigidbody == null)
        {
            Debug.LogError(
                $"{nameof(PlayerAnimation)} could not find Player Rigidbody.",
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

        float speed = velocity.magnitude;

        if (speed < movementThreshold)
            speed = 0f;

        animator.SetFloat(SpeedHash, speed);

        float runSpeedMultiplier =
            speed > 0f
                ? speed / referenceRunSpeed
                : 1f;

        runSpeedMultiplier = Mathf.Clamp(
            runSpeedMultiplier,
            minRunAnimationSpeed,
            maxRunAnimationSpeed
        );

        animator.SetFloat(
            RunSpeedMultiplierHash,
            runSpeedMultiplier
        );
    }
}