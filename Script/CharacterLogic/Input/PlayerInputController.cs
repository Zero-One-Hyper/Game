using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;


//只管输入 也就是可以放在最开始的场景中
public class PlayerInputController : MonoBehaviour, InputSetting.IPlayerActions, IInputController
{
    public ICharacterController CurrentCharacter;

    public CharacterBase characterBase { get; set; }

    private void Awake()
    {
        this.characterBase = this.transform.GetComponent<CharacterBase>();
        //currentCharacter要从PlayerManager里拿
        this.CurrentCharacter = this.transform.GetComponent<ICharacterController>();
        InitInputSystem();
        //playerInput.
    }

    //绑定事件
    private void InitInputSystem()
    {
        InputManager.Instance.Init();
        InputManager.Instance.EnableInputSetting(InputType.Player);
        InputManager.Instance.SetCallBack(this, InputType.Player);
    }

    //利用InputSystem的Process新增加的Stick Deadzone 来避免遥感漂移    
    public void OnMove(InputAction.CallbackContext context)
    {
        //Vector2 vec = context.ReadValue<Vector2>();
        //Debug.Log(vec);

        CurrentCharacter?.SetInputRotate(context.ReadValue<Vector2>());
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        //Vector2 vec = context.ReadValue<Vector2>();
        //Debug.Log(vec);

        CurrentCharacter?.animController.CharacterViewRotate(context.ReadValue<Vector2>());
    }

    public void OnLockEnemy(InputAction.CallbackContext context)
    {
        //bool val = context.control.IsPressed();

        //Debug.Log(val);
        if (context.control.IsPressed())
        {
            CurrentCharacter?.PressLock();
        }
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        //bool val = context.performed;
        //Debug.Log(val);
        if (context.performed)
        {
            if (this.characterBase.character.characterState.CurrentEnergy >= CharacterDataDefine.AttackCost)
            {
                this.DoAttack();
            }
        }
    }

    public void OnRoll_Run(InputAction.CallbackContext context)
    {
        if (context.interaction is HoldInteraction)
        {
            if (context.control.IsPressed())
            {
                if (context.performed)
                {
                    //Debug.Log("Run Start");
                    this.Run(true);
                }
            }
            else
            {
                //Debug.Log("Run End");
                this.Run(false);
            }
        }
        else if (context.performed)
        {
            if (this.characterBase.character.characterState.CurrentEnergy >= CharacterDataDefine.RollCost)
            {
                //Debug.Log(this.characterBase.character.characterState.CurrentEnergy);
                this.DoRoll();
            }            
        }
    }
    public void OnUseShield(InputAction.CallbackContext context)
    {
        //Debug.Log(context.duration);
        if(context.control.IsPressed())
        {
            if (context.performed)
            {
                //Debug.Log("Shield Up");
                this.UseShield(true);
            }
        }
        else
        {
            this.UseShield(false);
        }
        //if(context.duration )
    }
    public void OnToggleLockEnemy(InputAction.CallbackContext context)
    {

        if (context.performed)
        {
            //Debug.Log("切换锁定敌人");
            //Debug.Log(context.control.name);
            CurrentCharacter?.ToggleLockEnemy(context.control.name);
        }
    }

    public void DoAttack()
    {
        CurrentCharacter?.DoAttack();
    }
    public void DoRoll()
    {
        CurrentCharacter?.DoRoll();
    }
    public void SetAnim(PlayerAnimEventArg animEventArg)
    {

    }
    private void Run(bool isRunning)
    {
        CurrentCharacter?.DoRun(isRunning);
    }
    private void UseShield(bool ShieldUp)
    {
        CurrentCharacter?.DoUseShield(ShieldUp);
    }

}
