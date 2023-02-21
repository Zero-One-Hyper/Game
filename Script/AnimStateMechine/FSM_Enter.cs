using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Enter : StateMachineBehaviour
{
    [Header("��SendMessage����ʽ����")]
    public string[] messageEnter;
    public string[] messageString;
    [Header("���¼�����ʽ������Ϣ")]
    public string[] eventEnter;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Character character = null;
        character = animator.gameObject.GetComponentInParent<CharacterBase>() != null ? animator.gameObject.GetComponentInParent<CharacterBase>().character : null;
        Enemy enemy = null;
        enemy = animator.gameObject.GetComponentInParent<EnemyBase>() != null ? animator.gameObject.GetComponentInParent<EnemyBase>().enemy : null;

        for (int i = 0; i < messageEnter.Length; i++)
        {
            animator.gameObject.SendMessage(messageEnter[i], messageString[i]);
        }
        foreach (var ev in eventEnter)
        {
            if (character != null)
                AnimEventCenter.Instance.TriggerEvent(ev, character);
            if (enemy != null)
                AnimEventCenter.Instance.TriggerEvent(ev, enemy);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
