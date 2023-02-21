using Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{   
    public Animator Animator { get; set; }
    private ControlType controlType;
    public ControlType ControlType
    {
        get
        {
            return this.controlType;
        }
        set
        {
            switch(value)
            {
                case ControlType.None:
                    break;
                case ControlType.LOCAL:
                    this.EnemyStateMechine.enabled = true;
                    break;
                case ControlType.NET:
                    this.EnemyStateMechine.enabled = false;
                    break;
            }
            this.controlType = value;
        }
    }
    public EnemyStateMechine EnemyStateMechine;

    private AnimActionType currentActionType;

    private void Awake()
    {
        this.Animator = this.GetComponentInChildren<Animator>();
        this.EnemyStateMechine = this.GetComponent<EnemyStateMechine>();
        this.EnemyStateMechine.StateMechineAction += this.PlayAnim;
        this.ControlType = ControlType.LOCAL;
    }
    public void EnemyAttack()
    {
        DoAttack();
    }

    private void DoAttack()
    {
        this.PlayAnim(AnimActionType.Attack);
    }
    internal void DoHit()
    {
        this.PlayAnim(AnimActionType.Hurt);
    }
    public void PlayAnim(AnimActionType actionType)
    {
        switch(actionType)
        {
            case AnimActionType.Idle:
                this.Animator.SetBool("Run", false);
                this.Animator.SetBool("Walk", false);
                this.Animator.SetBool("Stop", true);
                break;
            case AnimActionType.Walk:

                this.Animator.SetBool("Stop", false);
                this.Animator.SetBool("Run", false);
                this.Animator.SetBool("Walk", true);
                break;
            case AnimActionType.Run:
                this.Animator.SetBool("Stop", false);
                this.Animator.SetBool("Walk", false);
                this.Animator.SetBool("Run", true);
                break;
            case AnimActionType.Attack:
                Debug.Log("Attack");
                this.Animator.SetTrigger("Attack");
                if(this.controlType == ControlType.LOCAL) 
                    EnemyManager.Instance.SendEnemyAttack(this.EnemyStateMechine.EnemyBase.EnemyID);
                break;
            case AnimActionType.Hurt:
                Debug.Log("Play Hurt");
                this.Animator.SetTrigger("Hurt");
                break;
            case AnimActionType.Dead:
                Debug.Log("Play Dead");
                //this.Animator.SetBool("Dead", true); 
                this.Animator.SetTrigger("Dead"); 
                break;
        }
        this.currentActionType = actionType;
    }
    
    public EnemyAnimeEvent GetAnimEvent()
    {
        return new EnemyAnimeEvent()
        {
            ActionType = this.currentActionType,
        };
    }
    public void SetAnim(EnemyAnimeEvent anim)
    {
        this.PlayAnim(anim.ActionType);
    }

}
