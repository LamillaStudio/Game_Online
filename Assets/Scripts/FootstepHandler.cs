using UnityEngine;

public class FootstepHandler : MonoBehaviour
{
    [SerializeField] private AudioClip[] footstepSounds;
    [SerializeField] private AudioSource audioSource;

    void OnFootstep()
    {
        if (footstepSounds.Length == 0 || audioSource == null) return;

        AudioClip clip = footstepSounds[Random.Range(0, footstepSounds.Length)];
        audioSource.PlayOneShot(clip);
    }
}