using UnityEngine;

public class AnimationFootstepEvents : MonoBehaviour
{
    [Header("Footsteps")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] footstepClips;
    [Range(0f, 1f)]
    [SerializeField] private float footstepVolume = 0.5f;

    public void OnFootstep(AnimationEvent animationEvent)
    {
        // Event może odpalać się podczas blendowania dwóch animacji.
        if (animationEvent.animatorClipInfo.weight < 0.5f)
            return;

        if (audioSource == null ||
            footstepClips == null ||
            footstepClips.Length == 0)
        {
            return;
        }

        AudioClip clip =
            footstepClips[Random.Range(0, footstepClips.Length)];

        audioSource.PlayOneShot(
            clip,
            footstepVolume
        );
    }

    public void OnLand(AnimationEvent animationEvent)
    {
        // Na razie nic.
        // Zostawiamy, bo część animacji Kyle'a może mieć również event OnLand.
    }
}