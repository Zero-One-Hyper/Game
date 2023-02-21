using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEquipItem : MonoBehaviour
{
    public UIEquip Owner;
    private Equip equip;
    public Image equipIcon;
    public void Set(UIEquip owner, Equip equip)
    {
        this.Owner = owner;
        this.equip = equip;
        if (this.equipIcon != null) 
        {
            this.SetIcon(equip.Icon);
        }
    }
    public void OnClickEquipItem()
    {
        if(equip == null) 
            return;
        EquipManager.Instance.PlayerUnEquipArmor(this.equip.armorType);
        this.SetIcon();
        this.equip = null;
    }
    public void SetIcon(Sprite sprite = null)
    {
        if(sprite != null)
        {
            this.equipIcon.overrideSprite = equip.Icon;
            this.equipIcon.color = new Color(255, 255, 255, 255);
        }
        else
        {
            this.equipIcon.color = new Color(255, 255, 255, 0);
            this.equipIcon.overrideSprite = null;
        }
    }
}
