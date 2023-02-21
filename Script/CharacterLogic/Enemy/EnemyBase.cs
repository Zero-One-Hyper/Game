using Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    public int EnemyID
    {
        get
        {
            return this.enemy.EnemyID;
        }
        private set
        {
            this.enemy.EnemyID = value;
        }
    }
    [SerializeField]
    private int enemyid;
    public Enemy enemy;
    public bool NetControl;

    public Transform EnemyAimPoint;
    public GameObject EnemyModle;
    public Collider Collider;
    public EnemyController EnemyController;
    public EnemyStateMechine EnemyStateMechine;

    public Transform UIHealthBarPoint;
    public UIHealthBar UIHealthBar;

    public Func<EnemyAnimeEvent> OnAnimPlay;
    private EnemyAnimeEvent eventArg;

    public WeaponHandler WeaponHandler;

    private void Awake()
    {
        this.EnemyController = this.GetComponent<EnemyController>();
        this.EnemyStateMechine = this.GetComponent<EnemyStateMechine>();
        this.Collider = this.GetComponent<Collider>();

        if (this.enemy == null)
            this.enemy = new Enemy(this, this.enemyid);

        EnemyManager.Instance.AddEnemyEntity(this.enemy);

        this.OnAnimPlay += this.EnemyController.GetAnimEvent;
        EnemyManager.Instance.SwitchEnemyControlType += this.SwitchControlType;
    }
    private void Start()
    {
        this.SwitchControlType(ControlType.LOCAL);
        GameObject go = Resources.Load<GameObject>("UIPrefab/HealthBar");
        go = Instantiate(go);
        this.UIHealthBar = go.GetComponent<UIHealthBar>();                      
        this.UIHealthBar.Init(this.enemy, UIHealthBarPoint);
    }
    private void FixedUpdate()
    {
        this.UpdateEnemyTransform();
        this.UpdateEnemyAnim();
    }

    public void Set(Vector3 postion, Vector3 forward)
    {        
        this.transform.position = postion;
        this.EnemyModle.transform.forward = forward;
        this.SwitchControlType(ControlType.NET);
    }
    public void SwitchControlType(ControlType type)
    {
        this.EnemyController.ControlType = type;
        this.NetControl = type == ControlType.NET;
    }
    int testUpdate = 2;
    private void UpdateEnemyTransform()
    {
        if(this.NetControl)
        {
            this.transform.position = this.enemy.Position;
            this.transform.forward = this.enemy.Direction;
        }
        else
        {
            if (testUpdate % 2 != 0) return;
            testUpdate++;
            if (testUpdate > 100)
                testUpdate = 1;
            this.enemy.EntityUpdate(this.transform.position, this.transform.forward);
            //Ð´Íê·¢ËÍ
            if (this.enemy.CanPosDirUpdate())
            {                
                this.eventArg = this.OnAnimPlay?.Invoke();
                
                EnemyService.Instance.SendEnemyEntitySync(new EnemyEntityRequest()
                {
                    EnemyId = this.enemy.EnemyID,
                    NPosition = this.enemy.NPosition,
                    NDirection = this.enemy.NDirection,

                    ActionType = this.eventArg.ActionType,
                });
                
            }
        }
    }
    private void UpdateEnemyAnim()
    {
        if(this.NetControl)
        {
            if(this.enemy.AnimEventArg != null)
                this.EnemyController.SetAnim((EnemyAnimeEvent)this.enemy.AnimEventArg);
        }
    }

    internal void AttackCheseTarget(bool isAttack)
    {
        this.WeaponHandler.SwitchAttack(isAttack);
        /*
        GameObject AttackTarget = this.EnemyStateMechine.ChaseTarget;
        CharacterBase characterBase = AttackTarget.GetComponent<CharacterBase>();
        characterBase.GetHit(this.enemy.AttackDamage);
        */
    }
    public void GetHit(float Damage)
    {
        this.enemy.EnemyState.CurrentHealth -= Damage;
        this.EnemyController.DoHit();

        if (this.enemy.EnemyState.CurrentHealth <= 0)
        {
            GameManager.Instance.OnEnemyDead?.Invoke(this.enemy.EnemyID);
            this.EnemyStateMechine.SwitchStates(EnemyStates.DEAD);
        }
    }
    public void OnEnemyDaed()
    {
        this.Collider.enabled = false; 
        this.UIHealthBar.gameObject.SetActive(false);
        this.WeaponHandler.OnEnemyDead();
        EnemyManager.Instance.TriggerEvent("EnemyDead", this, this.enemy.EnemyEventArg);
    }
}
