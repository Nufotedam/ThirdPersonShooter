using UnityEngine;

public class WeaponLogic : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ActiveLayerWeapon(animator);
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ActiveLayerWeapon(animator);
        //  Changes the weapon change status :/
        animator.GetComponent<PlayerWeapon>().isChanging = false;
    }

    private static void ActiveLayerWeapon(Animator animator)
    {
        //  Check wheter or not the player has an equipped weapon
        if (!animator.GetBool("AK") && !animator.GetBool("Pistol") && !animator.GetBool("Shotgun"))
        {
            animator.SetLayerWeight(1, 0);
        }
        else
        {
            animator.SetLayerWeight(1, 1);
        }
    }
}
