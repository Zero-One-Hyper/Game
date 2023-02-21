using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PopUpWindowType
{
    None,
    Error,
    Notice,
}
//用于所有弹出UI
public class UIManager : Singleton<UIManager>
{
    public UIManager()
    {

    }

    const string UIResource = "UIPopWindow/";

    public T ShowPopUpWindow<T>(PopUpWindowType windowType) where T : UIPopUpWindow
    {
        switch (windowType)
        {
            case PopUpWindowType.Error:
                SoundManager.Instance.PlayErroSound();
                break;
            case PopUpWindowType.Notice:
                break;
        }
        Type type = typeof(T);
        //Debug.Log("实例化UI:    " + type.Name);
        GameObject prefab = Resources.Load<GameObject>(UIResource + type.Name);

        GameObject go = UnityEngine.Object.Instantiate(prefab);

        T resWindow = go.GetComponent<T>();
        
        return resWindow;
    }
}
