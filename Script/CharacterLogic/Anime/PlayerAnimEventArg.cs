using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[SerializeField]
public class PlayerAnimEventArg : EventArgs
{
    public bool isLocked;

    public bool isRunning;

    public bool isUseShield;

    public float speedX = 0;

    public float speedY = 0;
       

}
