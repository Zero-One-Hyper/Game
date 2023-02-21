using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;

public class NetInputController : MonoBehaviour, IInputController
{
    public CharacterBase characterBase { get; set; }
    public ICharacterController CurrentCharacter;

    private bool LockEnemy = false;
    // Start is called before the first frame update
    void Awake()
    {
        this.characterBase = this.transform.GetComponent<CharacterBase>();
        this.CurrentCharacter = this.transform.GetComponent<ICharacterController>();

    }

    //利用InputSystem的Process新增加的Stick Deadzone 来避免遥感漂移    
    public void OnMove(Vector2 vec)
    {
        CurrentCharacter?.SetInputRotate(vec);
    }
    public void OnLockEnemy()
    {
        CurrentCharacter?.PressLock();

    }
    public void SetAnim(PlayerAnimEventArg animEventArg)
    {
        if (animEventArg == null)
            return;
        if (animEventArg.isLocked != this.LockEnemy)
        {
            this.OnLockEnemy();
            this.LockEnemy = animEventArg.isLocked;
        }
        this.OnMove(new Vector2(animEventArg.speedX, animEventArg.speedY));
        this.DoRun(animEventArg.isRunning);
        this.UseShield(animEventArg.isUseShield);
        
    }

    public void DoAttack()
    {       
        Debug.Log("网络角色攻击！！！");
        CurrentCharacter?.DoAttack();
    }

    public void DoRoll()
    {
        Debug.Log("网络角色翻滚！！！！！");
        CurrentCharacter?.DoRoll();
    }
    public void DoRun(bool isRunning)
    {
        if(isRunning)
            Debug.Log("网络角色Run");
        CurrentCharacter?.DoRun(isRunning);
    }
    public void UseShield(bool isShield)
    {
        if(isShield)
            Debug.Log("网络角色使用盾牌");
        CurrentCharacter?.DoUseShield(isShield);    
    }    
}
