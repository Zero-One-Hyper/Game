using Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateChase : IEnemyStates
{
    private GameObject ChaseTarget;
    private EnemyStateMechine Owner;
    private Action<AnimActionType> ChaseAction
    {
        get
        {
            return this.Owner.StateMechineAction;
        }
    }

    private NavMeshAgent NavMeshAgent
    {
        get
        {
            return this.Owner.NavMeshAgent;
        }
    }
    private float AttackRange;
    private float ChaseRange;

    private bool isChase = true;

    private float AttackFrequncy = 3.0f;
    private float calcAttackFrequncy = 3.0f;

    private bool lostPlayer = false;
    private bool isWaiting = false;
    private float calcTime = 0;
    private float WaitTime;

    public StateChase(float chaseRange, float attackRange, GameObject chasePosition, EnemyStateMechine owner)
    {
        this.ChaseTarget = chasePosition;  
        this.Owner = owner;
        this.AttackRange = attackRange;
        this.ChaseRange = chaseRange;
    }
    public void ChangeCheseTarget(GameObject chaseTarget)
    {
        this.ChaseTarget = chaseTarget;
    }
    public void OnEnterState()
    {
        Debug.Log("Enter Chase State");
        this.isChase = true;
        this.NavMeshAgent.stoppingDistance = this.AttackRange;
        if(this.ChaseTarget!= null)
        {
            this.NavMeshAgent.destination = this.ChaseTarget.transform.position;
            this.ChaseAction?.Invoke(AnimActionType.Run);
        }
        else
        {
            this.isChase = false;
            this.lostPlayer = true;
        }
    }

    public void OnExitState()
    {
        Debug.Log("Exit Chase State");
    }

    public void OnUpdateState(float deltaTime)
    {
           
        if(OutOfChaseRange())
        {
            this.isChase = false;
            if (!isWaiting)
            {
                this.WaitForScend();
                this.NavMeshAgent.destination = this.Owner.transform.position;
            }
        }
        if (isChase)
        {
            this.ChaseAction?.Invoke(AnimActionType.Run);
        }
        else
        {
            this.ChaseAction?.Invoke(AnimActionType.Idle);
        }
        if (isWaiting)
        {
            calcTime += deltaTime;
            if (calcTime < WaitTime)
            {
                return;
            }
            this.isWaiting = false;
            if (lostPlayer)
            {
                //this.ChaseAction?.Invoke(AnimActionType.Idle);
                this.Owner.SwitchStates(Owner.defaultStates);
                return;
            }
        }
        this.NavMeshAgent.destination = this.ChaseTarget.transform.position;       
    }
    public void OnFixedUpdateState(float fixedDeltaTime)
    {
        //Debug.Log("FixedUpdate Chase State");
        if (TargetInAttackRange())
        {
            this.isChase = false;
            this.AttackTarget(fixedDeltaTime);
        }
        else
        {
            this.isChase = true;
            this.calcAttackFrequncy = 3.0f;
            this.FaceToChaseTarget(fixedDeltaTime);
        }
    }

    private bool OutOfChaseRange()
    {
        float distance = Vector3.Distance(this.Owner.transform.position, this.ChaseTarget.transform.position); ;
        //Debug.Log(distance);
        if (distance >= this.ChaseRange)
        {
            this.lostPlayer = true;
            return true;
        }
        this.lostPlayer = false;
        this.isWaiting = false; 
        this.NavMeshAgent.destination = this.Owner.transform.position;
        return false;
    }
    private bool TargetInAttackRange()
    {
        float distance = Vector3.Distance(this.Owner.transform.position, this.ChaseTarget.transform.position);
        //Debug.Log(distance);
        if (distance <= this.AttackRange)
        {
            return true;
        }
        return false;
    }
    private void AttackTarget(float deltatime)
    {
        this.calcAttackFrequncy += deltatime;
        if(this.calcAttackFrequncy < this.AttackFrequncy)
        {
            return;
        }
        ChaseAction?.Invoke(AnimActionType.Attack);
        this.calcAttackFrequncy = 0;
    }
    private void WaitForScend()
    {
        this.isWaiting = true;
        this.isChase = false;
        this.calcTime = 0;
        this.WaitTime = UnityEngine.Random.Range(3.0f, 7.0f);
        this.ChaseAction?.Invoke(AnimActionType.Idle);
    }

    private void FaceToChaseTarget(float fixedDeltaTime)
    {
        Vector3 temp = Vector3.ProjectOnPlane(this.ChaseTarget.transform.position - this.Owner.transform.position, Vector3.up);
        temp = new Vector3(temp.x, this.Owner.transform.forward.y, temp.z);
        this.Owner.transform.forward = Vector3.Slerp(this.Owner.transform.forward, temp, 35.0f * fixedDeltaTime);
    }
}
