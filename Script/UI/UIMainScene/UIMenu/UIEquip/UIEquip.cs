using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEquip : MonoBehaviour
{
    public UIEquipItem[] equipItems;

    private void Start()
    {
        int[] temparmor = User.Instance.UserCharacter.ArmorIDS;
        Equip equip;
        for(int i = 0; i < 4; i++)//暂时选4个 后面添加武器
        {
            equip = BagManager.Instance.Equips[temparmor[i]];
            this.SetEquipItem(equip);
        }
    }
    public void SetEquipItem(Equip equip)
    {
        equipItems[(int)equip.armorType].Set(this, equip);
    }
    public void SetUIEquipItemIcon(int type)
    {
        this.equipItems[type].SetIcon();
    }
}
