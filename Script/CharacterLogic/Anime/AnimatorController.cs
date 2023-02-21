using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum AnimMoveStatus
{
    NONE,
    UNLOCK,
    LOCK,
}

public class AnimatorController : MonoBehaviour
{
    public Animator anim;
    public ICharacterController characterController;
    public CharacterBase characterBase;

    public AnimMoveStatus animStatus = AnimMoveStatus.UNLOCK;

    public float AnimMaxSpeed = 4.740124f;
    public float AnimSprintSpeed = 6.083995f;

    private float horizon = 0;
    private float vertical = 0;
    private Vector3 AnimVelocity;

    private float InputModule;

    private Vector2 inputSpeed = new Vector2(0, 0);
    private Vector2 currentAnimSpeed = new Vector2(0, 0);
    private Vector2 animSpeed = new Vector2(0, 0);

    private PlayerAnimEventArg animEventArg = new PlayerAnimEventArg();

    bool canAttack = true;
    bool canRoll = true;

    bool rollEnd = true;

    [SerializeField]
    private bool isRunning = false;
    [SerializeField]
    private bool isUseShield = false;
    [SerializeField]
    private bool canUseShield = true;

    public bool UseAnimYSpeed
    {
        get
        {
            //翻滚过程中需要使用动画的Y轴速度
            return !rollEnd;
        }
    }

    private void Awake()
    {
        this.characterBase = this.GetComponentInParent<CharacterBase>();
        this.characterController = this.GetComponentInParent<ICharacterController>();
        this.anim = this.GetComponent<Animator>();
        characterBase.OnAnimPlay += this.AnimPlaying;
    }

    private PlayerAnimEventArg AnimPlaying()
    {
        this.animEventArg.isLocked = this.animStatus == AnimMoveStatus.LOCK;
        this.animEventArg.speedX = this.inputSpeed.x;
        this.animEventArg.speedY = this.inputSpeed.y;

        this.animEventArg.isRunning = this.isRunning;
        this.animEventArg.isUseShield = this.isUseShield;

        return this.animEventArg;
    }

    // Start is called before the first frame update
    void Start()
    {
        AnimEventCenter.Instance.AddListener("ReUseShield", this.FreeShield);
    }


    // Update is called once per frame
    void Update()
    {
        this.AnimMove();
        this.ShieldAnim();
    }

    private void OnAnimatorMove()
    {
        AnimVelocity = anim.velocity;
        characterController.SetVelocity(AnimVelocity, this.UseAnimYSpeed);
    }
    public void AnimMove()
    {
        //currentAnimSpeed = Vector2.LerpUnclamped(currentAnimSpeed, animSpeed, 0.2f);
        //Debug.Log(animSpeed);
        if (this.animStatus == AnimMoveStatus.LOCK && !this.isRunning)
        {
            this.anim.SetFloat("X_Speed", animSpeed.x, 0.2f, Time.deltaTime);
            this.anim.SetFloat("Y_Speed", animSpeed.y, 0.2f, Time.deltaTime);

        }
        else
        {
            //Debug.Log(currentAnimSpeed.magnitude);
            if(isRunning)
            {
                this.anim.SetFloat("Y_Speed", animSpeed.magnitude * (this.AnimSprintSpeed / this.AnimMaxSpeed), 0.2f, Time.deltaTime);
            }
            else
            {
                this.anim.SetFloat("Y_Speed", animSpeed.magnitude, 0.2f, Time.deltaTime);
            }
        }

    }

    //X是横向速度
    //Y是纵向速度
    internal void CharacterMove(Vector2 vec, Vector3 playerForward)
    {
        inputSpeed.x = vec.x;
        inputSpeed.y = vec.y;

        animSpeed = CalcSpeed(vec, playerForward);

        currentAnimSpeed = new Vector2(this.anim.GetFloat("X_Speed"), this.anim.GetFloat("Y_Speed"));
    }

    //圆锥曲线 把InputSystem 得到的模长为1的输入 放到模长为sqr(X_Speed^2 + Y_Speed^2)的圆上    
    Vector2 CalcSpeed(Vector2 speed, Vector3 playerForward)
    {
        /*
        //kX = Y  计算斜率
        float k;
        if (speed.y <= 0.1f || speed.x < 0.1f)
            k = 0;
        else
            k = speed.y / speed.x;
        //计算横轴X
        float x = Mathf.Pow(AnimMaxSpeed, 2) / (1 + (k * k));
        float y = Mathf.Sqrt((Mathf.Pow(AnimMaxSpeed, 2) - (Mathf.Pow(x, 2))));
        */
        //计算个p的斜率 直接极坐标
        //InputSystem 模长为1；但还是算一算
        InputModule = Mathf.Sqrt(Mathf.Pow(speed.x, 2) + Mathf.Pow(speed.y, 2));
        if (InputModule < 0.001f) return new Vector2(0, 0);
        /*
        float cos = speed.x / module;
        float sin = speed.y / module;

        //Debug.Log(module);
        float animSpeed = (module / 1) * AnimMaxSpeed;
        //Debug.Log(animSpeed);
        float x = animSpeed * cos;
        float y = animSpeed * sin;
        return new Vector2(x, y);
        */
        return new Vector2((InputModule / 1) * AnimMaxSpeed * speed.x / InputModule,
            (InputModule / 1) * AnimMaxSpeed * speed.y / InputModule);
    }
    public void CharacterViewRotate(Vector2 vec)
    {
        horizon = vec.x;
        vertical = vec.y;
    }

    internal void SwitchLock()
    {
        if (this.animStatus == AnimMoveStatus.UNLOCK)
        {
            this.animStatus = AnimMoveStatus.LOCK;
            this.anim.SetBool("Locked", true);
        }
        else
        {
            this.animStatus = AnimMoveStatus.UNLOCK;
            this.anim.SetBool("Locked", false);
        }
    }

    internal void DoAttack()
    {
        if (this.canAttack)
        {
            this.canUseShield = false;
            this.anim.SetTrigger("Attack");
            this.anim.SetLayerWeight(this.anim.GetLayerIndex("Shield"), 0);
        }
    }

    internal void DoRoll()
    {
        if(canRoll)
        {
            this.canUseShield = false;
            this.anim.SetTrigger("Roll");
            this.rollEnd = false;
            this.anim.SetLayerWeight(this.anim.GetLayerIndex("Shield"), 0);
        }        
    }

    public void OnFreeTrigger(string trigger)
    {
        //Debug.Log("Free " + trigger);
        switch(trigger)
        {
            case "Attack":
                this.canAttack = true;
                //this.canUseShield = true;
                this.anim.ResetTrigger("Attack");
                break;
            case "Roll":
                this.canRoll = true;
                
                this.anim.ResetTrigger("Roll");
                break;  
        }
    }
    public void OnLockTrigger(string trigger)
    {
        //Debug.Log("Lock " + trigger);
        switch (trigger)
        {
            case "Attack":
                this.canAttack = false;
                this.anim.ResetTrigger("Attack");
                break;
            case "Roll":
                this.canRoll = false;   
                this.anim.ResetTrigger("Roll");
                break;
        }
    }
    public void EndRoll()
    {
        AnimEventCenter.Instance.TriggerEvent("EndRoll", null);
    }

    internal void DoRun(bool isRunning)
    {
        this.isRunning = isRunning;
    }
    private void ShieldAnim()
    {
        if(!this.canUseShield) return;
        this.anim.SetFloat("ShieldWeight", isUseShield ? 1 : 0, 0.03f, Time.deltaTime);
        this.anim.SetLayerWeight(this.anim.GetLayerIndex("Shield"), this.anim.GetFloat("ShieldWeight"));
    }
    internal void DoUseShield(bool shieldUp)
    {        
        if(!this.canUseShield) return;
        this.isUseShield = shieldUp;
        //this.anim.SetLayerWeight(this.anim.GetLayerIndex("Shield"), shieldUp? 1 : 0);
        this.anim.SetBool("ShieldUp", shieldUp);
        this.characterBase.Block(this.isUseShield);
    }
    private void FreeShield(object sender, EventArgs e)
    {
        this.canUseShield = true;
    }
    internal void DoHit()
    {
        if (this.characterBase.character.characterState.CurrentEnergy < 1.0f / 30.0f && this.isUseShield)
        {
            this.anim.SetTrigger("OverBlock");
        }
        this.anim.SetTrigger("Hit");

    }
    public void AttackStart()
    {
        //Debug.Log("AttackStart");
        this.characterBase.WeaponHandler.SwitchAttack(true);
    }
    public void AttackEnd()
    {
        //Debug.Log("AttackEnd");
        this.characterBase.WeaponHandler.SwitchAttack(false);
    }
    public void CharacterDead()
    {
        this.anim.SetFloat("ShieldWeight", 0);
        this.anim.SetLayerWeight(this.anim.GetLayerIndex("Shield"), 0);
        this.anim.SetTrigger("Dead");
    }
}
