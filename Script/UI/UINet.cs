using System.Text;
using System.Net.Mail;
using System.Linq;
using System.Net.Mime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINet : MonoSingleton<UINet>
{
    private const string serverIP = "192.168.0.104";
    private const int prot = 8848;

    public Image loadingImg;
    public Text loadingText;

    IEnumerator LoadingUI;

    Action<bool, string> UInetAction;

    protected override void OnStart()
    {
        UInetAction += this.ConnectCallBack;
    }



    // Start is called before the first frame update
    void Start()
    {

    }
    public void OnStartConnect()
    {
        this.ConnectToServer();
    }

    private void ConnectToServer()
    {
        LoadingUI = UIConnecting();
        StartCoroutine(LoadingUI);
        NetClient.Instance.Init(serverIP, prot);

        NetClient.Instance.DoConnect(ConnectCallBack);
    }
    IEnumerator UIConnecting()
    {
        float fill = 0;
        bool clock = true;
        string tempText = "Login";
        int c = 0;
        while (true)
        {
            fill += clock ? 0.115f : (-0.115f);
            if (fill >= 1 || fill <= 0)
            {
                clock = !clock;
                loadingImg.fillClockwise = clock;
            }
            loadingImg.fillAmount = fill;
            if (c > 3)
            {
                tempText = "Login";
                c = 0;
            }
            else
            {
                c++;
                tempText += ".";
            }
            loadingText.text = tempText;


            yield return new WaitForSeconds(0.3f);
        }
    }

    private void ConnectCallBack(bool result, string erromsg)
    {
        StopCoroutine(LoadingUI);
        UINotice uI = UIManager.Instance.ShowPopUpWindow<UINotice>(PopUpWindowType.Error);
        uI.Set(erromsg);
        if (!result)
        {
            uI.OnCloseWindow += () =>
            {
                this.transform.gameObject.SetActive(false);
            };
            return;
        }

        this.transform.gameObject.SetActive(false);

    }

}
