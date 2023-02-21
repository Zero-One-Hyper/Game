using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UImainCanvas : MonoSingleton<UImainCanvas>
{
    public UIMenu UIMenu;
    public UICharacterState UICharacterState;
    public GameObject PannelMenuButton;

    public Text TextStartGame;
    public Text TextWin;
    public Text TextFail;
    public IEnumerator GameStart;

    private bool isOpen = false;
    public void Start()
    {
        this.UIMenu.gameObject.SetActive(false);
        this.TextStartGame.gameObject.SetActive(false);
        this.TextWin.gameObject.SetActive(false);
        this.TextFail.gameObject.SetActive(false);
        
        this.GameStart = this.StartGame();
        this.OnStartGame();
    }
    public void OpenMenu()
    {
        if (isOpen)
        {
            this.OnCloseMenu();
        }
        else
        {
            UIInput.Instance.SetSelectedUI(UIMenu.gameObject, this.CloseMenu);

            SoundManager.Instance?.PlaySound(SoundDataDefine.UIButtonSwitch_01);
            this.isOpen = true;
        }
    }
    public void OnCloseMenu()
    {
        this.CloseMenu();
    }
    private void CloseMenu()
    {
        this.isOpen = false;
        this.UIMenu.gameObject.SetActive(false);

        InputManager.Instance?.EnableInputSetting(InputType.Player);
        SoundManager.Instance?.PlaySound(SoundDataDefine.UIButtonSwitch_01);
    }
    public void OnExitGame()
    {
        SoundManager.Instance?.PlaySound(SoundDataDefine.UIButtonSwitch_02);
        this.ExitGame();
    }
    private void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void OnClickPutSign()
    {
        SoundManager.Instance?.PlaySound(SoundDataDefine.UIButtonSwitch_01);
        //在原地生成物体
        MapService.Instance.SendSummonSign(User.Instance.UserCharacter.CharacterId,
            User.Instance.UserCharacter.characterBase.transform.position,
            User.Instance.UserCharacter.characterBase.transform.forward);
    }
    private IEnumerator StartGame()
    {
        float waitTime = 3.5f;
        float temp = 0;
        this.TextStartGame.gameObject.SetActive(true);
        while (temp <= waitTime)
        {
            temp += Time.deltaTime;
            yield return null;
        }
        this.TextStartGame.gameObject.SetActive(false);
    }
    public void OnStartGame()
    {
        this.StartCoroutine(GameStart);
    }
    public void OnWin()
    {
        this.TextWin.gameObject.SetActive(true);
    }
    public void OnFail()
    {
        this.TextFail.gameObject.SetActive(true);
    }
}
