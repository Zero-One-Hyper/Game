using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICharacter : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler,
    ISelectHandler, IDeselectHandler,  ISubmitHandler
{
    
    public int CharacterId;
    public GameObject CharacterInfo;
    public GameObject SelectedImg;
    private UISelectCharacter Owner;

    public Text[] CharacterInfos;
    
    public void Set(UISelectCharacter owner)
    {
        this.Owner = owner;
        SetCharacterInfo();
        this.SelectedImg.SetActive(false);
    }

    void SetCharacterInfo()
    {

    }
    private void Start() 
    {
        this.CharacterInfo.SetActive(false);
    }

    public void OnSelectCharacterChange(int charId)
    {
        if(charId != this.CharacterId)
        {
            this.SelectedImg.SetActive(false);
        }
    }
    public int OnSelectCharacter()
    {
        
        return CharacterId;
    }
    public void ShowData()
    {
        this.CharacterInfo.SetActive(true);
    }
    public void HideData()
    {
        this.CharacterInfo.SetActive(false);
    }
    public void SelectCharacter()
    {
        this.SelectedImg.SetActive(true);
        SoundManager.Instance.PlayButtonSwitch02Sound();
        this.Owner.OnSelectCharacter(this.CharacterId);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        this.ShowData();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        this.SelectCharacter();
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        this.HideData();
    }

    public void OnSelect(BaseEventData eventData)
    {
        this.ShowData();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        this.HideData();
    }


    public void OnSubmit(BaseEventData eventData)
    {
        Debug.Log(114514);
        this.SelectCharacter();
    }
}
