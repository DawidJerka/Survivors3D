using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHitFeedback : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Image hitOverlay;
    [SerializeField] private CameraFollow cameraFollow;

    [Header("Flash")]
    [SerializeField] private float maxAlpha = 0.22f;
    [SerializeField] private float fadeDuration = 0.18f;

    private Coroutine flashCoroutine;

    private void OnEnable()
    {
        if (playerHealth != null)
            playerHealth.OnDamaged += HandleDamaged;
    }

    private void OnDisable()
    {
        if (playerHealth != null)
            playerHealth.OnDamaged -= HandleDamaged;
    }

    private void Start()
    {
        SetAlpha(0f);
    }

    private void HandleDamaged(float damage)
    {
        cameraFollow?.Shake();

        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);

        flashCoroutine =
            StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        SetAlpha(maxAlpha);

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(
                elapsed / fadeDuration
            );

            SetAlpha(
                Mathf.Lerp(maxAlpha, 0f, t)
            );

            yield return null;
        }

        SetAlpha(0f);
        flashCoroutine = null;
    }

    private void SetAlpha(float alpha)
    {
        if (hitOverlay == null)
            return;

        Color color = hitOverlay.color;
        color.a = alpha;
        hitOverlay.color = color;
    }
}