using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopUpWindow : MonoBehaviour
{
    public Button CloseButton;
    public Action OnCloseWindow;
    public virtual void PopWindowCloseUI()
    {
        SoundManager.Instance.PlayButtonSwitch01Sound();

        OnCloseWindow?.Invoke();

        Destroy(this.gameObject);
    }
}
