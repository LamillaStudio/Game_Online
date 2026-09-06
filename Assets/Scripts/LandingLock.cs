using UnityEngine;

public class LandingLock : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerMovement pm = animator.GetComponentInParent<PlayerMovement>();
        if (pm != null) pm.SetMovementLocked(true);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerMovement pm = animator.GetComponentInParent<PlayerMovement>();
        if (pm != null) pm.SetMovementLocked(false);
    }
}
