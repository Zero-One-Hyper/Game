using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UINotice : UIPopUpWindow
{
    public Text erromsg;

    

    public void Set(string NoticeMsg)
    {
        erromsg.text = NoticeMsg;
        UIInput.Instance.SetSelectedPopUI(this.gameObject, this.PopWindowCloseUI);
        UIInput.Instance.SelectObject = this.CloseButton.gameObject;
    }

    public void OnClickClose()
    {
        UIInput.Instance.CloseUI();
    }
    public override void PopWindowCloseUI()
    {
        base.PopWindowCloseUI();
    }
}
