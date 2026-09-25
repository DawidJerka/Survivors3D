using System.Collections;
using UnityEngine;

public class EnemyHitFeedback : MonoBehaviour
{
    [SerializeField] private Transform visualRoot;

    [Header("Recoil")]
    [SerializeField] private float recoilDistance = 0.12f;
    [SerializeField] private float recoilDuration = 0.05f;
    [SerializeField] private float returnDuration = 0.08f;

    private Vector3 originalLocalPosition;
    private Coroutine recoilCoroutine;

    private void Awake()
    {
        if (visualRoot != null)
            originalLocalPosition = visualRoot.localPosition;
    }

    public void PlayHit(Vector3 hitDirection)
    {
        if (visualRoot == null)
            return;

        if (recoilCoroutine != null)
            StopCoroutine(recoilCoroutine);

        recoilCoroutine = StartCoroutine(
            RecoilRoutine(hitDirection)
        );
    }

    private IEnumerator RecoilRoutine(Vector3 hitDirection)
    {
        Vector3 direction = hitDirection;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            yield break;

        direction.Normalize();

        Vector3 worldTarget =
            visualRoot.position +
            direction * recoilDistance;

        float elapsed = 0f;

        while (elapsed < recoilDuration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(
                elapsed / recoilDuration
            );

            visualRoot.position = Vector3.Lerp(
                transform.TransformPoint(originalLocalPosition),
                worldTarget,
                t
            );

            yield return null;
        }

        Vector3 recoilPosition =
            visualRoot.localPosition;

        elapsed = 0f;

        while (elapsed < returnDuration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(
                elapsed / returnDuration
            );

            visualRoot.localPosition = Vector3.Lerp(
                recoilPosition,
                originalLocalPosition,
                t
            );

            yield return null;
        }

        visualRoot.localPosition =
            originalLocalPosition;

        recoilCoroutine = null;
    }

    private void OnDisable()
    {
        if (recoilCoroutine != null)
            StopCoroutine(recoilCoroutine);

        if (visualRoot != null)
            visualRoot.localPosition =
                originalLocalPosition;
    }
}