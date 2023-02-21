using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Exit : StateMachineBehaviour
{
    [Header("以SendMessageUpwards的形式触发")]
    public string[] SendUpwards;
    [Header("以SendMessage的形式触发")]
    public string[] messageExit;
    public string[] messageString;
    [Header("以事件的形式触发消息")]
    public string[] eventExit;

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Character character = null; 
        character = animator.gameObject.GetComponentInParent<CharacterBase>() != null? animator.gameObject.GetComponentInParent<CharacterBase>().character : null;
        Enemy enemy = null;
        enemy = animator.gameObject.GetComponentInParent<EnemyBase>() != null? animator.gameObject.GetComponentInParent<EnemyBase>().enemy : null;
        foreach(var msg in SendUpwards)
        {
            animator.gameObject.SendMessageUpwards(msg);
        }
        for(int i = 0; i < messageExit.Length; i++)
        {
            animator.gameObject.SendMessage(messageExit[i], messageString[i]);
        }
        foreach (var ev in eventExit)
        {
            if(character != null)
                AnimEventCenter.Instance.TriggerEvent(ev, character);
            if(enemy != null)
                AnimEventCenter.Instance.TriggerEvent(ev, enemy);
        }        
    }

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
