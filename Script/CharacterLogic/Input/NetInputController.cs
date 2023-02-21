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

    //����InputSystem��Process�����ӵ�Stick Deadzone ������ң��Ư��    
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
        Debug.Log("�����ɫ����������");
        CurrentCharacter?.DoAttack();
    }

    public void DoRoll()
    {
        Debug.Log("�����ɫ��������������");
        CurrentCharacter?.DoRoll();
    }
    public void DoRun(bool isRunning)
    {
        if(isRunning)
            Debug.Log("�����ɫRun");
        CurrentCharacter?.DoRun(isRunning);
    }
    public void UseShield(bool isShield)
    {
        if(isShield)
            Debug.Log("�����ɫʹ�ö���");
        CurrentCharacter?.DoUseShield(isShield);    
    }    
}
