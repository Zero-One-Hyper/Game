using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum LockType
{
    None,
    Left,
    Right
}

public class CharacterController : MonoBehaviour, ICharacterController
{
    [Header("组件")]
    public Rigidbody rigid;
    public CapsuleCollider capsule;
    public AnimatorController animController { get; set; }
    public IInputController inputController;

    public Transform PlayerModel;

    public Camera cameraMain;
        
    private Vector2 inputVec = new Vector2(0, 0);
    private Vector3 InputForward = new Vector3(0, 0, 0);
    private Vector3 CameraForward = new Vector3(0, 0, 0);

    //private Vector2 rollForward = new Vector2(0, 0);
    private bool LockForward = false;
    [Header("地面检测")]
    [SerializeField]
    private bool IsGround = true;
    private Collider[] GroundsCollider;
    //[SerializeField]
    private float Up_Offset = 1.47f;
    //[SerializeField]
    private float Down_Offset= 1.64f;
    //[SerializeField]
    private float ColliderRound = 0.33f;
        
#if UNITY_EDITOR
    [Header("TestLockBox")]
    public Mesh mesh;
#endif

    [Header("锁定Box")]
    [SerializeField]
    private GameObject LockTarget;
    [SerializeField]
    private Vector3 LockTargetVector;
    [SerializeField]
    private float MaxLockDistance = 30.0f;

    private Enemy LockEnemy;


    //与敌人的障碍物检测射线信息
    private RaycastHit ObstructionsHit;
    //[SerializeField]
    private float Haf_LockCheckBoxLength = 10.0f;
    //[SerializeField]
    private Vector3 LockCheckBoxScale = new Vector3(5.0f, 5.0f, 10.0f);
    private Collider[] EnemyColliders;
        
    private Vector3 enemyPosition;

    private float EnemyDistance;
    private float MinEnemyDistance;
    private Vector3 WordToViewPoint;
    public LayerMask mask;

    private bool isRunning = false;
    private Character character;
    private void Awake()
    {
        this.inputController = this.transform.GetComponent<IInputController>();
        this.rigid = this.transform.GetComponent<Rigidbody>();
        this.animController = this.transform.GetComponentInChildren<AnimatorController>();

        PlayerModel = animController.transform;
        cameraMain = Camera.main;
        CameraForward = Vector3.ProjectOnPlane(cameraMain.transform.forward, Vector3.up).normalized;
        capsule = this.transform.GetComponent<CapsuleCollider>();
    }
    void Start()
    {
        this.character = User.Instance.UserCharacter;
        AnimEventCenter.Instance.AddListener("RollRotate", this.DoRollRotate);
        AnimEventCenter.Instance.AddListener("EndRoll", this.EndRoll);
        AnimEventCenter.Instance.AddListener("Attack", this.AttackEvent);
        AnimEventCenter.Instance.AddListener("Roll", this.RollEvent);

        EnemyManager.Instance.AddEventListener("EnemyDead", this.OnEnemyDead);
    }


    // Update is called once per frame
    void Update()
    {
        if(LockTarget != null 
            && Vector3.Distance(LockTarget.transform.position, this.transform.position) > MaxLockDistance)
        {
            PressLock();
        }

        if (LockForward)
        {
            if (InputForward.magnitude > 0.02f)
                PlayerModel.forward = Vector3.Slerp(PlayerModel.forward, InputForward, 55.0f * Time.deltaTime);

            return;
        }
        //animController.AnimMove();
        if(this.animController.animStatus == AnimMoveStatus.LOCK && !this.isRunning)
        {
            //设计一直朝向敌人
            enemyPosition = this.LockTarget.transform.position;
            this.PlayerModel.forward = Vector3.Slerp(
                PlayerModel.forward, 
                Vector3.ProjectOnPlane((enemyPosition - PlayerModel.position), Vector3.up).normalized, 
                40.0f * Time.deltaTime);
        }
        else if (this.animController.animStatus == AnimMoveStatus.UNLOCK || this.isRunning)
        {
            CameraForward = Vector3.ProjectOnPlane(cameraMain.transform.forward, Vector3.up).normalized;

            InputForward = CameraForward * inputVec.y + cameraMain.transform.right * inputVec.x;

            if (InputForward.magnitude > 0.02f)
                PlayerModel.forward = Vector3.Slerp(PlayerModel.forward, InputForward, 10.0f * Time.deltaTime);
        }
        if(isRunning)
        {
            if (character.characterState.CurrentEnergy <= 0)
            {
                animController.DoRun(false);
                this.isRunning = false;
            }
            else
            {
                if (this.character.CharacterId == User.Instance.PlayerId)
                    UImainCanvas.Instance.UICharacterState.EditEnergy(1.0f / 30.0f);
                this.character.characterState.CurrentEnergy -= (1.0f / 30.0f);
            }
        }
    }
    private void FixedUpdate()
    {
        OnGround();
        
        CheckForObstructions();
    }
  
    bool OnGround()
    {
        /*
        Vector3 point1 = this.transform.position + transform.up * Up_Offset;
        Vector3 point2 = this.transform.position + transform.up * capsule.height - transform.up * Down_Offset;
        */

        GroundsCollider = Physics.OverlapCapsule(this.transform.position + transform.up * Up_Offset,
            this.transform.position + transform.up * capsule.height - transform.up * Down_Offset, 
            ColliderRound, 
            LayerMask.GetMask("Ground"));

        if (GroundsCollider.Length > 0)
        {
            this.IsGround = true;
            return true;
        }
        else
        {
            this.IsGround = false;
            return false;
        }
    }
    private void CheckForObstructions()
    {
        if (this.LockTarget == null) 
            return;
        if (Physics.Raycast(this.transform.position + Vector3.up * this.capsule.height / 1.5f,
            LockTarget.transform.position - this.transform.position + Vector3.up * this.capsule.height / 1.5f,
            out ObstructionsHit,
            Vector3.Distance(this.transform.position, LockTarget.transform.position),
            mask))
        {
#if UNITY_EDITOR
            Debug.DrawLine(this.transform.position + Vector3.up * this.capsule.height / 1.5f,
                ObstructionsHit.transform.position,
                Color.red);
#endif
            if(ObstructionsHit.collider != null)
            {
                Debug.Log(ObstructionsHit.collider.gameObject.name);
                this.PressLock();
            }
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position + transform.up * Up_Offset, ColliderRound);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position + transform.up * capsule.height - transform.up * Down_Offset, ColliderRound);

        if (UnityEditor.EditorApplication.isPlaying)
        {
            Gizmos.color = Color.green;

            /*Gizmos.DrawWireMesh(mesh,
                this.transform.position + Vector3.up * 0.8f + cameraMain.transform.forward * Haf_LockCheckBoxLength,
                cameraMain.transform.rotation,
                //Quaternion.LookRotation(CameraForward, Vector3.up),
                LockCheckBoxScale * 2.0f);*/
            if (LockTarget != null)
            {
                Gizmos.color = Color.red;/*
                Gizmos.DrawLine(this.transform.position + Vector3.up * this.capsule.height / 1.5f,
                    LockTarget.transform.position + Vector3.up * 0.9f);*/
            }
        }
    }
#endif

    public void SetVelocity(Vector3 animVelocity, bool UseAnimYSpeed)
    {
        //不需要使用动画Y轴速度 或者离开地面
        if(!UseAnimYSpeed || !IsGround)
            animVelocity.y = rigid.velocity.y;
        //Debug.Log(animVelocity);
        rigid.velocity = animVelocity;
    }
    public void SetInputRotate(Vector2 vec)
    {
        this.inputVec = vec;
        this.animController.CharacterMove(vec, this.transform.forward);
    }
    public void ToggleLockEnemy(string val)
    {                                                                       
        if(LockTarget != null)
        {
            //Debug.Log(val);
            if(val == "left" || val == "q")
            {
                //在左边
                //Debug.Log("左");
                var temp = CheckTarget(LockType.Left);
                LockTarget = temp == null ? LockTarget : temp;
            }
            else if(val == "right" || val == "e")
            {
                //Debug.Log("右");
                var temp = CheckTarget(LockType.Right);
                LockTarget = temp == null ? LockTarget : temp;
            }
        }
        
    }
    public void PressLock()
    {
        if (this.LockTarget == null)
        {
            LockTarget = CheckTarget();            
        }
        else
        {
            this.LockTarget = null;
            //设置玩家取消对准Enemy
            animController.SwitchLock();
            UIBattle.Instance.UnLockTarget();
        }        
    }
    private GameObject CheckTarget(LockType type = LockType.None)
    {
        GameObject res = null;
        EnemyColliders = Physics.OverlapBox(this.transform.position + Vector3.up * 0.8f + cameraMain.transform.forward * Haf_LockCheckBoxLength,
                                                      LockCheckBoxScale,
                                                      cameraMain.transform.rotation,
                                                      //Quaternion.LookRotation(CameraForward, Vector3.up), 
                                                      LayerMask.GetMask("Enemy"));
        MinEnemyDistance = 999.99f;
        
        if (LockTarget != null)
            LockTargetVector = Vector3.ProjectOnPlane((LockTarget.transform.position - this.transform.position), Vector3.up);

        foreach (var col in EnemyColliders)
        {
            if (type != LockType.None && LockTarget != null)
            {
                if (LockTarget != null && col.gameObject == this.LockTarget)
                    continue;
                //Vector3 ttt = Vector3.ProjectOnPlane((col.transform.position - this.transform.position), Vector3.up);
                //unity的cross左手规则 
                float temp = Vector3.Cross(LockTargetVector,
                    Vector3.ProjectOnPlane((col.transform.position - this.transform.position), Vector3.up)).y;
                
                if ((type == LockType.Left && temp > 0) || (type == LockType.Right && temp < 0))
                    continue;
                else
                {
                   // Debug.Log(col.gameObject.name);
                }
            }
            //判断频幕内
            WordToViewPoint = cameraMain.WorldToViewportPoint(col.transform.position);
            if (WordToViewPoint.x > 0 && WordToViewPoint.x < 1 && WordToViewPoint.y > 0 && WordToViewPoint.y < 1)
            {
                //Debug.Log(col.gameObject);                
                EnemyDistance = Vector3.Distance(col.transform.position, 
                    type == LockType.None? this.transform.position : LockTarget.transform.position);
                if (EnemyDistance < MinEnemyDistance)
                    res = col.gameObject;
            }
            //Debug.Log(Vector3.Distance(this.transform.position, LockTarget.transform.position));
        }
       
        if (res != null)
        {
            //设置玩家forwoed对准Enemy
            var temp = res.GetComponent<EnemyBase>();
            this.LockEnemy = temp.enemy;
            CameraManager.Instance.SetLockTarget(temp.EnemyAimPoint);
            if (type == LockType.None)
                animController.SwitchLock();
        }
        return res;
    }

    #region 事件
    private void DoRollRotate(object sender, EventArgs e)
    {
        //计算按下翻滚时遥感角度
        /*this.rollForward = this.inputVec;*/
        if(cameraMain == null) return;
        CameraForward = Vector3.ProjectOnPlane(cameraMain.transform.forward, Vector3.up).normalized;
        InputForward = CameraForward * inputVec.y + cameraMain.transform.right * inputVec.x;
        //Debug.Log(rollForward);
        this.LockForward = true;

        //解除forword角度锁定 结束后再判断要不要回来
    }

    public void EndRoll(object sender, EventArgs e)
    {
        //Debug.Log("EndRoll");
        this.LockForward = false;
    }
    private void RollEvent(object sender, EventArgs e)
    {
        //Debug.Log("Roll Cast");
        this.character.characterState.CurrentEnergy -= CharacterDataDefine.RollCost;
        if (this.character.characterState.CurrentEnergy < 0)
            this.character.characterState.CurrentEnergy = 0;
        if (this.character.characterState.CurrentEnergy > this.character.characterState.MaxEnergy)
            this.character.characterState.CurrentEnergy = this.character.characterState.MaxEnergy;
        if ((Character)sender == User.Instance.UserCharacter)
        {
            UImainCanvas.Instance.UICharacterState.EditEnergy(CharacterDataDefine.RollCost);
            BattleService.Instance.CharacterRoll();
        }
    }

    private void AttackEvent(object sender, EventArgs e)
    {
        //Debug.Log("Attack Cost");
        this.character.characterState.CurrentEnergy -= CharacterDataDefine.AttackCost;
        if (this.character.characterState.CurrentEnergy < 0)
            this.character.characterState.CurrentEnergy = 0;
        if (this.character.characterState.CurrentEnergy > this.character.characterState.MaxEnergy)
            this.character.characterState.CurrentEnergy = this.character.characterState.MaxEnergy;
        if ((Character)sender == User.Instance.UserCharacter)
        {
            UImainCanvas.Instance.UICharacterState.EditEnergy(CharacterDataDefine.AttackCost);
            BattleService.Instance.CharacterAttack();
        }
    }

    #endregion
    public void DoAttack()
    {
        //做碰撞判断  
        Debug.Log("本地角色攻击");
        animController.DoAttack();
    }
    public void DoRoll()
    {
        animController.DoRoll();
    }
    public void DoRun(bool isRunning)
    {
        if (character.characterState.CurrentEnergy > 0)
        {
            animController.DoRun(isRunning);
            this.isRunning = isRunning;
        }
    }

    public void DoUseShield(bool ShieldUp)
    {
        animController.DoUseShield(ShieldUp);          
    }

    public void DoHit()
    {
        this.animController.DoHit();
    }
    private void OnEnemyDead(object sender, EventArgs e)
    {
        EnemyEventArg eventarg = (EnemyEventArg)e;
        if(this.LockTarget != null && eventarg.EnemyID == this.LockEnemy.EnemyID)
        {
            this.PressLock();
        }
    }

    public void DoDead()
    {
        this.animController.CharacterDead();
    }
}
