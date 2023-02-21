using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISelectCharacter : MonoBehaviour
{
    private int selectCharacterId;
    public int SelectCharacter
    {
        get
        {
            return this.selectCharacterId;
        }
        set
        {
            if(this.selectCharacterId != value)
            {
                this.selectCharacterId = value;
                this.OnSelectCharacterChange?.Invoke(value);
            }
        }
    }
    public Action<int> OnSelectCharacterChange;
    public UICharacter[] uiCharacters;

    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            uiCharacters[i].Set(this);
            this.OnSelectCharacterChange += uiCharacters[i].OnSelectCharacterChange;
            
        }
        this.uiCharacters[0].CharacterInfo.SetActive(true);
    }
    private void OnEnable()
    {
        UIInput.Instance.SelectObject = this.uiCharacters[0].gameObject;
        this.uiCharacters[0].CharacterInfo.SetActive(true);
    }
    private void OnDisable()
    {
        for (int i = 0; i < 3; i++)
        {
            uiCharacters[i].CharacterInfo.SetActive(false);           
        }
    }
    public void OnSelectCharacter(int val)
    {
        this.SelectCharacter = val;
    }

    public void OnClickEnterGame()
    {
        SoundManager.Instance.PlayButtonSwitch01Sound();
        /*
        CharacterManager.Instance.SetCharacterID(this.selectCharacterId);
        */
        SceneManager.Instance.LoadScene("SceneTestBaseComponent");
        
        User.Instance.CharacterTypeID = this.selectCharacterId;
    }

    public void OnClickExit()
    {
        UIInput.Instance.CloseUI();
    }
    public void ClickExit()
    {
        SoundManager.Instance.PlayButtonSwitch01Sound();
        this.gameObject.SetActive(false);
    }
}
