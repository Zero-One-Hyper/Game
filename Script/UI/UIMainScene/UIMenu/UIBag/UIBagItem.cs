using Protocol;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIBagItem : MonoBehaviour
{
    public UIBag Owner;
    public Image BagItemIcon;
    public GameObject isEquipImage;
    public int equipId
    {
        get
        {
            return equip.EquipID;
        }
    }
    public string equipName
    {
        get
        {
            return equip.itemName;
        }
    }
    public ArmorType armorType
    {
        get
        {
            return this.equip.armorType;
        }
    }
    private Equip equip;
    private bool IsEquiped;
    public bool isEquip
    {
        get
        {
            return IsEquiped;
        }
        set
        {
            this.IsEquiped = value;
            isEquipImage.SetActive(value);
        }
    }

    public void InitBagItem(UIBag bag, Equip equip)
    {
        this.Owner = bag;
        if(equip != null ) this.equip = equip;
        if(this.BagItemIcon != null) this.BagItemIcon.overrideSprite = equip.Icon;
        this.BagItemIcon.color = new Color(255, 255, 255, 255);
    }

    public void OnClickItem()
    {
        if(this.equip != null)
        {
            if (this.isEquip)
            {
                this.isEquip = false;
                EquipManager.Instance.PlayerUnEquipArmor(this.equip.armorType);
                this.Owner.SetUIEquipItem(this, null);
            }
            else
            {
                this.isEquip = true;
                EquipManager.Instance.PlayerEquipArmor(this.equip);
                this.Owner.SetUIEquipItem(this, this.equip);
            }
        }
    }
    
}
