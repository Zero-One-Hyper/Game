using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterController
{
    public void SetVelocity(Vector3 animVelocity, bool hasYSpeed);    
    public void SetInputRotate(Vector2 vec);
    public void ToggleLockEnemy(string val);
    public void PressLock();
    public void DoAttack();
    public void DoRoll();
    public void DoRun(bool isRunning);
    public void DoUseShield(bool ShileldUp);
    public void DoHit();
    public void DoDead();

    public AnimatorController animController { get; set; }
}
