using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Follow")]
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0f, 10f, -8f);
    [SerializeField] private float smoothSpeed = 5f;

    private Vector3 followPosition;

    [Header("Camera Shake")]
    [SerializeField] private float defaultShakeDuration = 0.1f;
    [SerializeField] private float defaultShakeStrength = 0.12f;

    private float shakeTimeRemaining;
    private float shakeDuration;
    private float shakeStrength;

    private void Start()
    {
        if (target != null)
        {
            followPosition = target.position + offset;
            transform.position = followPosition;
        }
        else
        {
            followPosition = transform.position;
        }
    }

    private void LateUpdate()
    {
        UpdateFollow();
        UpdateShake();
    }

    private void UpdateFollow()
    {
        if (target == null)
            return;

        Vector3 desiredPosition =
            target.position + offset;

        followPosition = Vector3.Lerp(
            followPosition,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );
    }

    private void UpdateShake()
    {
        if (shakeTimeRemaining <= 0f)
        {
            transform.position = followPosition;
            return;
        }

        float progress =
            shakeTimeRemaining / shakeDuration;

        float currentStrength =
            shakeStrength * progress;

        Vector2 randomOffset =
            Random.insideUnitCircle * currentStrength;

        // Shake w płaszczyźnie ekranu.
        Vector3 shakeOffset =
            transform.right * randomOffset.x +
            transform.up * randomOffset.y;

        transform.position =
            followPosition + shakeOffset;

        shakeTimeRemaining -=
            Time.unscaledDeltaTime;
    }

    public void Shake()
    {
        Shake(
            defaultShakeDuration,
            defaultShakeStrength
        );
    }

    public void Shake(
        float duration,
        float strength)
    {
        if (duration <= 0f ||
            strength <= 0f)
        {
            return;
        }

        shakeDuration = duration;
        shakeTimeRemaining = duration;
        shakeStrength = strength;
    }
}