using UnityEngine;

public class ReloadReset : StateMachineBehaviour
{
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //  When the player is reloading, he will not be able to perform any other action until the animation is finished.
        animator.GetComponent<PlayerWeapon>().isReloading = false;
        //  Update the ammo info of the weapon when the animation finish
        animator.GetComponent<PlayerWeapon>().AmmoInfo();
    }
}
