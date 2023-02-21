using Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateGuard : IEnemyStates
{
    private Transform GuardPoint;
    private EnemyStateMechine Owner;
    private Action<AnimActionType> GuardAction
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
            return Owner.NavMeshAgent;
        }
    }
    public StateGuard(Transform guardPositon, EnemyStateMechine owner)
    {
        this.GuardPoint = guardPositon;
        this.Owner = owner;
    }

    public void OnEnterState()
    {
        //Debug.Log("Enter Guard State");
        if(this.GuardPoint!= null)
        {
            this.Owner.AgentStopDistance = 0.0f;
            this.NavMeshAgent.destination = this.GuardPoint.transform.position;
            this.Owner.transform.forward = this.GuardPoint.transform.forward;    
        }
        if(Vector3.Distance(this.Owner.transform.position, this.GuardPoint.transform.position) > 1.0f)
        {
            this.GuardAction?.Invoke(AnimActionType.Walk);
        }
        else
        {
            this.GuardAction?.Invoke(AnimActionType.Idle);
        }
    }

    public void OnExitState()
    {
        Debug.Log("Exit Guard State");
        this.Owner.AgentStopDistance = 3.0f;
    }

    public void OnUpdateState(float deltaTime)
    {
        //Debug.Log(this.NavMeshAgent.speed);

        Vector3.Lerp(this.Owner.transform.forward, GuardPoint.transform.forward, 1.0f * Time.deltaTime);
        
        if (Vector3.Distance(this.Owner.transform.position, this.GuardPoint.transform.position) > 1.0f)
        {
            this.GuardAction?.Invoke(AnimActionType.Walk);
        }
        else
        {
            this.GuardAction?.Invoke(AnimActionType.Idle);
        }
    }
    public void OnFixedUpdateState(float fixedDeltaTime)
    {
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
}
