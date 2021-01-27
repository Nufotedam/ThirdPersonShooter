using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLogic : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ActiveLayerWeapon(animator);
    }   

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{

    //}

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ActiveLayerWeapon(animator);
        animator.GetComponent<PlayerWeapon>().isChanging = false;
    }

    private static void ActiveLayerWeapon(Animator animator)
    {
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
