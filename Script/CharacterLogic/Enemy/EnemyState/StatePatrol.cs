using Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StatePatrol : IEnemyStates
{
    private Vector3 CurrentPoint = Vector3.zero;
    private Vector3 ArrivePoint = Vector3.zero;
    private Transform[] PatrolPosition;

    private bool isPatrolTo = true;
    private Stack<Vector3> patrolPointStack = new Stack<Vector3>();
    private EnemyStateMechine Owner;
    private Action<AnimActionType> PatrolAction
    {
        get
        {
            return this.Owner.StateMechineAction;
        }
    }

    private bool isWait = false;
    private float calcTime = 0;
    private float WaitTime;

    private NavMeshAgent NavMeshAgent
    {
        get 
        { 
            return this.Owner.NavMeshAgent;
        }
    }
    public StatePatrol(Transform[] patrolPosition, EnemyStateMechine owner)
    {
        this.PatrolPosition = patrolPosition;
        this.Owner = owner;
    }
    public void OnEnterState()
    {
        //Debug.Log("Enter Patrol State");
        if(this.CurrentPoint == Vector3.zero)
        {
            this.CurrentPoint = this.PatrolPosition[0].position;
            patrolPointStack.Push(this.CurrentPoint);
        }
        this.PatrolAction?.Invoke(AnimActionType.Walk);
        this.NavMeshAgent.stoppingDistance = 0.5f;
        this.NavMeshAgent.destination = this.CurrentPoint;
        this.isWait = false;
    }

    public void OnExitState()
    {
        //Debug.Log("Exit Patrol State");
    }
    
    public void OnUpdateState(float deltaTime)
    {
        //Debug.Log("Update Patrol State");
        if (isWait)
        {
            calcTime += deltaTime;
            if (calcTime < WaitTime)
            {
                this.PatrolAction?.Invoke(AnimActionType.Idle);
                return;            
            }
            else
            {
                isWait = false;
                this.PatrolAction?.Invoke(AnimActionType.Walk);
                this.NavMeshAgent.destination = this.CurrentPoint;
            }
        }
        if (Vector3.Distance(this.Owner.transform.position, this.CurrentPoint) <= NavMeshAgent.stoppingDistance)
        {
            //到达目标点
            if (CurrentPoint == this.PatrolPosition[0].position || CurrentPoint == this.PatrolPosition[this.PatrolPosition.Length - 1].position)
            {
                this.WaitForScend();
            }

            if (isPatrolTo)
            {
                this.patrolPointStack.Push(this.PatrolPosition[this.patrolPointStack.Count].position);
                this.CurrentPoint = patrolPointStack.Peek();
                if (this.patrolPointStack.Count == this.PatrolPosition.Length)
                {
                    isPatrolTo = !isPatrolTo;
                }
            }
            else
            {
                this.CurrentPoint = patrolPointStack.Pop();
                if (this.patrolPointStack.Count == 0)
                {
                    isPatrolTo = !isPatrolTo;
                }
            }
            if(!isWait)
                this.NavMeshAgent.destination = this.CurrentPoint;
        }
    }
    public void OnFixedUpdateState(float fixedDeltaTime)
    {
        //Debug.Log("FixedUpdate Patrol State");
        if(this.FindPlayer())
        {
            Debug.Log(this.Owner.ChaseTarget.gameObject.name);
            this.Owner.SwitchStates(EnemyStates.CHASE);
        }
    }
    private GameObject go;
    private float distance;
    private Collider[] colliders;
    private float distacneTemp;
    private bool FindPlayer()
    {
        //if (currentStates >= EnemyStates.CHASE) return false;
        go = null;
        distance = float.MaxValue;
        colliders = Physics.OverlapSphere(this.Owner.transform.position + Vector3.up, this.Owner.sightRadius);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                if (go != null)
                {
                    CharacterBase cb = go.GetComponent<CharacterBase>();
                    if (cb.character.characterState.CurrentHealth <= 0)
                        continue;
                    distacneTemp = Vector3.Distance(this.Owner.transform.position, collider.transform.position);
                    if (distacneTemp < distance)
                    {
                        go = collider.gameObject;
                    }
                }
                else
                {
                    go = collider.gameObject;
                    CharacterBase cb = go.GetComponent<CharacterBase>();
                    if (cb.character.characterState.CurrentHealth <= 0)
                    {
                        continue;
                        go = null;
                    }
                }
            }
        }
        if (go != null)
            this.Owner.ChaseTarget = go;

        return go != null;
    }
    private void WaitForScend()
    {
        this.isWait = true;
        this.calcTime = 0;
        this.WaitTime = UnityEngine.Random.Range(3.0f, 7.0f);
    }
}
