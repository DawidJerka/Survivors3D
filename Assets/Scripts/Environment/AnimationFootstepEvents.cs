using UnityEngine;

public class AnimationFootstepEvents : MonoBehaviour
{
    [SerializeField] private AudioSource footstepSource;
    [SerializeField] private AudioSource landingSource;

    public void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight < 0.5f)
            return;

        if (footstepSource != null &&
            footstepSource.isActiveAndEnabled)
        {
            footstepSource.Play();
        }
    }

    public void OnLand(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight < 0.5f)
            return;

        if (landingSource != null &&
            landingSource.isActiveAndEnabled)
        {
            landingSource.Play();
        }
    }
}