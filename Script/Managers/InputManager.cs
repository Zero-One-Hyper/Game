using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum InputType
{
    None = 0,
    Player = 1,
    UI = 2,
}
public class InputManager : MonoSingleton<InputManager>
{
    private InputSetting InputSetting;
    
    public void Init()
    {
        if(InputSetting == null)
            this.InputSetting = new InputSetting();
    }

    public void EnableInputSetting(InputType type)
    {
        switch (type)
        {
            case InputType.Player:
                this.InputSetting.Player.Enable();
                break;
            case InputType.UI:
                this.InputSetting.UI.Enable();
                break;
        }
    }
    public void DisAbleInputSetting(InputType type)
    {
        switch (type)
        {
            case InputType.Player:
                this.InputSetting.Player.Disable();
                break;
            case InputType.UI:
                this.InputSetting.UI.Disable();
                break;
        }
    }
    public void SetCallBack(object obj, InputType type)
    {
        switch (type)
        {
            case InputType.Player:
                this.InputSetting.Player.SetCallbacks((InputSetting.IPlayerActions)obj);                
                break;
            case InputType.UI:
                this.InputSetting.UI.SetCallbacks((InputSetting.IUIActions)obj);
                break;
        }
    }
}
