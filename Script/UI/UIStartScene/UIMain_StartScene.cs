using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Server;
using Google.Protobuf.WellKnownTypes;

public class UIMain_StartScene : MonoSingleton<UIMain_StartScene>
{
    public Button Button_Continue;
    public Button Button_NewGame;
    public Button Button_Setting;
    public Button Button_Exit;

    public GameObject StartTitlePannel;
    public GameObject MenuPannle;
    public GameObject ButtonPannelMask;
    public GameObject SettingPannel;
    public GameObject CharacterSelectCanves;
    public Text PressKeyText;

    private IEnumerator StartAnim;
    // Start is called before the first frame update
    protected override void OnStart()
    {
#if UNITY_EDITOR        
        this.Button_Continue.gameObject.SetActive(true);
#else
        this.Button_Continue.gameObject.SetActive(PlayerPrefs.HasKey("FirstPlay"));
#endif
        this.Button_NewGame.gameObject.SetActive(true);
        this.Button_Setting.gameObject.SetActive(true);
        this.Button_Exit.gameObject.SetActive(true);

        StartTitlePannel.SetActive(true);
        ButtonPannelMask.SetActive(false);
        SettingPannel.SetActive(false);
        CharacterSelectCanves.SetActive(false);
        MenuPannle.SetActive(false);

        this.StartAnim = this.StartTitle();
        this.StartCoroutine(StartAnim);

        GameManager.Instance.Init();
        EnemyManager.Instance.Init();
    }

    public void OnClickContinue()
    {
        SoundManager.Instance.PlaySound(SoundDataDefine.UIButtonSwitch_01);
    }
    public void OnClickNewGame()
    {
        SoundManager.Instance.PlaySound(SoundDataDefine.UIButtonSwitch_01);
        //this.CharacterSelectCanves.SetActive(true);
        UISelectCharacter uiSelect = CharacterSelectCanves.GetComponent<UISelectCharacter>();
        UIInput.Instance.SetSelectedUI(CharacterSelectCanves, uiSelect.ClickExit);
    }

    public void OnClickeLogin()
    {
        SoundManager.Instance.PlaySound(SoundDataDefine.UIButtonSwitch_01);
        UINet.Instance.OnStartConnect();
    }

    public void OnClickSetting()
    {
        SoundManager.Instance.PlaySound(SoundDataDefine.UIButtonSwitch_01);
        //ButtonPannelMask.SetActive(true); //把setActive拿到UIInput里去
        UISettingPannel_StartScene pannel = SettingPannel.GetComponent<UISettingPannel_StartScene>();
        UIInput.Instance.SetSelectedUI(SettingPannel, pannel.ExitClick);
    }


    public void OnClickExit()
    {
        SoundManager.Instance.PlaySound(SoundDataDefine.UIButtonSwitch_01);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    } 

    public void OnClickTestHeart()
    {
        UserService.Instance.SendHeartBeat();
    }

    public void OnClickTestUI()
    {
        UIManager.Instance.ShowPopUpWindow<UINotice>(PopUpWindowType.Error);
    }
    private IEnumerator StartTitle()
    {
        this.MenuPannle.SetActive(false);
        this.StartTitlePannel.SetActive(true); 
        Color changeColor = PressKeyText.color;
        float fillValue = 0;
        bool add = true;
        while (true)
        {
            if (add)
            {
                if (fillValue > 1)
                    add = false;
                fillValue += 0.004f;
            }
            else
            {
                if (fillValue < 0)
                    add = true;
                fillValue -= 0.004f;
            }
            changeColor.a = fillValue > 0 ? fillValue : 0;
            PressKeyText.color = changeColor;

            yield return null;
        }
    }
    public void StopShowStartTitle()
    {
        this.StopCoroutine(StartAnim);
        this.MenuPannle.SetActive(true);
        this.StartTitlePannel.SetActive(false);
    }
}
