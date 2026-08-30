using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerStats))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float baseMoveSpeed = 5f;

    private Rigidbody rb;
    private PlayerStats playerStats;
    private Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerStats = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        moveInput = Keyboard.current != null
            ? new Vector2( // TODO: Use Input System for movement input
                (Keyboard.current.dKey.isPressed ? 1f : 0f) -
                (Keyboard.current.aKey.isPressed ? 1f : 0f),

                (Keyboard.current.wKey.isPressed ? 1f : 0f) -
                (Keyboard.current.sKey.isPressed ? 1f : 0f)
            )
            : Vector2.zero;

        moveInput = moveInput.normalized;
    }

    private void FixedUpdate()
    {
        Vector3 movement =
            new Vector3(moveInput.x, 0f, moveInput.y);

        float currentMoveSpeed =
            baseMoveSpeed *
            playerStats.MoveSpeedMultiplier;

        rb.linearVelocity = new Vector3(
            movement.x * currentMoveSpeed,
            rb.linearVelocity.y,
            movement.z * currentMoveSpeed
        );
    }
}