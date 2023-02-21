using Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateDead : IEnemyStates
{
    private EnemyStateMechine Owner;
    private Action<AnimActionType> PlayDead
    {
        get
        {
            return this.Owner.StateMechineAction;
        }
    }
    public StateDead(EnemyStateMechine Owner)
    { 
        this.Owner = Owner; 
    }
    public void OnEnterState()
    {
        //Debug.Log("Enter Dead State");
        this.PlayDead?.Invoke(AnimActionType.Dead);
        this.Owner.EnemyBase.OnEnemyDaed();
        //this.Owner.EnemyBase.Collider.enabled = false;
    }

    public void OnExitState()
    {
        //Debug.Log("Exit Dead State");
        //this.PlayDead?.Invoke(AnimActionType.None);
        //this.Owner.EnemyBase.Collider.enabled = true;
    }

    public void OnUpdateState(float deltaTime)
    {
        //Debug.Log("Update Dead State");
    }
    public void OnFixedUpdateState(float fixedDeltaTime)
    { 
        //Debug.Log("FixedUpdate Dead State");
    }

}
