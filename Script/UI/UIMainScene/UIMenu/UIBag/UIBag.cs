using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIBag : MonoBehaviour
{
    public Transform ItemRoot;
    public List<UIBagItem> items = new List<UIBagItem>();
    public GameObject itemPrefab;
    public Button CloseBag;
    public Button Exit;
    public UIBagItem[] EquipItems;

    private void Awake()
    {
        this.EquipItems= new UIBagItem[6];
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        int[] currentEquips = User.Instance.UserCharacter.characterBase.GetEquips();
        for (int i = 0; i < BagManager.Instance.AllEquips.Count; i++)
        {
            if (items.Count < i)
            {
                GameObject go = Instantiate(itemPrefab, ItemRoot);

                this.items.Add(go.GetComponent<UIBagItem>());
            }
            this.items[i].gameObject.SetActive(true);
            this.items[i].InitBagItem(this, BagManager.Instance.AllEquips[i]);
            //判断是不是已经装备了
            if (items[i].equipId == currentEquips[(int)items[i].armorType])
            {
                this.SetUIEquipItem(items[i], BagManager.Instance.AllEquips[i]);
                items[i].isEquip = true;
                /*
                EquipItems[(int)items[i].armorType] = items[i];
                */
            }
        }
        for(int j = BagManager.Instance.AllEquips.Count; j < items.Count; j++)
        {
            this.items[j].gameObject.SetActive(false);
        }
        if(items.Count> 0)
        {
            UIInput.Instance.SelectObject = this.items[0].gameObject;
        }
        else
        {
            UIInput.Instance.SelectObject = CloseBag.gameObject;
        }
    }

    internal void SetUIEquipItem(UIBagItem item, Equip equip)
    {
        if(equip == null)
        {
            this.EquipItems[(int)item.armorType] = null;

            return;
        }
        //Debug.Log((int)equip.armorType);
        //把原来装备的卸下来
        if (this.EquipItems[(int)item.armorType] != null && this.EquipItems[(int)item.armorType] != item)
        {
            this.EquipItems[(int)item.armorType].isEquip = false;
        }
        this.EquipItems[(int)item.armorType] = item;
    }

    internal void ReSetUIEquipItemIcon(int type)
    {
        this.EquipItems[type].isEquip = false;
    }
}
