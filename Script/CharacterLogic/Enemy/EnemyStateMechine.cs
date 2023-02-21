using Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyStates
{
    GUARD = 0,
    PATROL = 1,
    CHASE = 2,
    DEAD = 3,
}
public class EnemyStateMechine : MonoBehaviour
{
    public Action<AnimActionType> StateMechineAction;
    public NavMeshAgent NavMeshAgent;
    public EnemyBase EnemyBase;
    private float agentStopDistance = 3.0f;
    public float AgentStopDistance
    {
        set
        {
            agentStopDistance = value;
            this.NavMeshAgent.stoppingDistance = value;
        }
    }

    [Header("敌人状态")]
    public EnemyStates defaultStates;
    [SerializeField]
    private EnemyStates currentStates;

    [Header("待机参数")]
    public Transform GuardPosition;
    public float sightRadius;//视线范围

    [Header("巡逻参数")]
    public Transform[] PatrolPosition;

    [Header("追踪参数")]
    public GameObject ChaseTarget;
    public float chaseRange;//追踪范围
    public float attackRange;

    //private Stack<IEnemyStates> StateStack = new Stack<IEnemyStates>();
    private IEnemyStates[] AllState = new IEnemyStates[4];
    private IEnemyStates enemyState;


    private void Awake()
    {
        this.NavMeshAgent = this.GetComponent<NavMeshAgent>();
        this.NavMeshAgent.stoppingDistance = this.agentStopDistance;
        this.EnemyBase = this.GetComponent<EnemyBase>();

        this.SwitchStates(this.defaultStates);
    }

    // Update is called once per frame
    void Update()
    {
        this.enemyState.OnUpdateState(Time.deltaTime);
    }
    private void FixedUpdate()
    {
        this.enemyState.OnFixedUpdateState(Time.fixedDeltaTime);       
    }
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position + Vector3.up * 1.5f, this.sightRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position + Vector3.up * 1.5f, this.chaseRange);
    }
#endif

    public void SwitchTarget(GameObject Target)
    {
        if (Target == null) return;
        this.ChaseTarget = Target;
    }

    public void SwitchStates(EnemyStates states, GameObject ChaseTarget = null)
    {
        this.enemyState?.OnExitState();
        switch(states)
        {
            case EnemyStates.GUARD:
                if (this.AllState[(int)EnemyStates.GUARD] == null)
                    this.enemyState = new StateGuard(GuardPosition, this);
                else
                {
                    this.enemyState = AllState[(int)EnemyStates.GUARD];
                }
                break;
            case EnemyStates.PATROL:
                if (this.AllState[(int)EnemyStates.PATROL] == null)
                    this.enemyState = new StatePatrol(PatrolPosition, this);
                else
                {
                    this.enemyState = AllState[(int)EnemyStates.PATROL];
                }
                break;
            case EnemyStates.CHASE:
                if (this.AllState[(int)EnemyStates.CHASE] == null)
                    this.enemyState = new StateChase(this.chaseRange, this.attackRange, this.ChaseTarget, this);
                else
                {
                    this.enemyState = AllState[(int)EnemyStates.CHASE];
                }
                ((StateChase)this.enemyState).ChangeCheseTarget(this.ChaseTarget);
                break;
            case EnemyStates.DEAD:
                if (this.AllState[(int)EnemyStates.DEAD] == null)
                    this.enemyState = new StateDead(this);
                else
                {
                    this.enemyState = AllState[(int)EnemyStates.DEAD];
                }
                break;
        }
        this.currentStates = states;
        //this.StateStack.Push(this.enemyState);
        if (this.AllState[(int)states] == null)
            this.AllState[(int)states] = this.enemyState;
        this.enemyState.OnEnterState();
    }
}
