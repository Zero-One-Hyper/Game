using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class NetCharacterController : MonoBehaviour, ICharacterController
{
    public Rigidbody rigid;
    public AnimatorController animController { get; set; }
    public IInputController inputController;
    public Transform PlayerModel;


    private Vector2 inputVec = new Vector2(0, 0);
    private Vector3 InputForword = new Vector3(0, 0, 0);
    private Vector3 CameraForword = new Vector3(0, 0, 0);


    private void Awake()
    {
        this.inputController = this.transform.GetComponent<IInputController>();
        this.rigid = this.transform.GetComponent<Rigidbody>();
        this.animController = this.transform.GetComponentInChildren<AnimatorController>();
        this.PlayerModel = this.animController.transform;

    }

    public void SetVelocity(Vector3 animVelocity, bool hasYSpeed)
    {        
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

    }
    public void PressLock()
    {
        animController.SwitchLock();
    }

    public void DoAttack()
    {
        Debug.Log("ÍøÂç½ÇÉ«¹¥»÷");
        animController.DoAttack();
    }

    public void DoRoll()
    {
        animController.DoRoll();
    }

    public void DoRun(bool isRunning)
    {
        animController.DoRun(isRunning);
    }
    public void DoUseShield(bool ShieldUp)
    {
        animController.DoUseShield(ShieldUp);
    }
    public void DoHit()
    {
        this.animController.DoHit();
    }

    public void DoDead()
    {
        this.animController.CharacterDead();
    }
}
