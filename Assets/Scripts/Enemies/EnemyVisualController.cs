using UnityEngine;

public class EnemyVisualController : MonoBehaviour
{
    private static readonly int SpeedHash =
        Animator.StringToHash("Speed");

    private static readonly int MoveSpeedMultiplierHash =
        Animator.StringToHash("RunSpeedMultiplier");

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private float referenceMoveSpeed = 3f;
    [SerializeField] private float movementThreshold = 0.1f;
    [SerializeField] private float animationDampTime = 0.1f;

    [SerializeField] private float minAnimationSpeed = 0.5f;
    [SerializeField] private float maxAnimationSpeed = 2f;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 720f;
    [SerializeField] private float rotationOffsetY = 0f;

    private Rigidbody enemyRigidbody;

    private void Awake()
    {
        enemyRigidbody = GetComponentInParent<Rigidbody>();

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        if (enemyRigidbody == null)
        {
            Debug.LogError(
                $"{nameof(EnemyVisualController)} could not find Rigidbody.",
                this
            );
        }

        if (animator == null)
        {
            Debug.LogError(
                $"{nameof(EnemyVisualController)} could not find Animator.",
                this
            );
        }
    }

    private void Update()
    {
        UpdateAnimation();
    }

    private void LateUpdate()
    {
        UpdateRotation();
    }

    private void UpdateAnimation()
    {
        if (enemyRigidbody == null || animator == null)
            return;

        Vector3 velocity = enemyRigidbody.linearVelocity;
        velocity.y = 0f;

        float speed = velocity.magnitude;

        if (speed < movementThreshold)
            speed = 0f;

        animator.SetFloat(
            SpeedHash,
            speed,
            animationDampTime,
            Time.deltaTime
        );

        float speedMultiplier =
            speed > 0f && referenceMoveSpeed > 0f
                ? speed / referenceMoveSpeed
                : 1f;

        speedMultiplier = Mathf.Clamp(
            speedMultiplier,
            minAnimationSpeed,
            maxAnimationSpeed
        );

        animator.SetFloat(
            MoveSpeedMultiplierHash,
            speedMultiplier
        );
    }

    private void UpdateRotation()
    {
        if (enemyRigidbody == null)
            return;

        Vector3 velocity = enemyRigidbody.linearVelocity;
        velocity.y = 0f;

        if (velocity.sqrMagnitude <
            movementThreshold * movementThreshold)
        {
            return;
        }

        Quaternion targetRotation =
            Quaternion.LookRotation(
                velocity.normalized,
                Vector3.up
            )
            * Quaternion.Euler(
                0f,
                rotationOffsetY,
                0f
            );

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}