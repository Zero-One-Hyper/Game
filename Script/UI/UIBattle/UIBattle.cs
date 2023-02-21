using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBattle : MonoSingleton<UIBattle>
{
    public UILockCenter UILockCenter;
    public void LockTarget(Transform go)
    {
        //Debug.Log("Lock Target" + go.name);
        UILockCenter.SetLockTarget(go);
    }
    public void UnLockTarget()
    {
        UILockCenter.UnLockTarget();
    }
}
