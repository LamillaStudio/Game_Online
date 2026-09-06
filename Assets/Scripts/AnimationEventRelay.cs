using UnityEngine;

public class AnimationEventRelay : MonoBehaviour
{
    private PlayerMovement playerMovement;

    void Awake()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    // Este método es el que el Animation Event va a llamar
    public void ApplyJumpForce()
    {
        playerMovement.ApplyJumpForce();
    }
}
