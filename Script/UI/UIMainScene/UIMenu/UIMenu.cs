using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenu : MonoBehaviour
{
    public UIBag bag;
    public UIEquip equip;

    public void SetUIEquipIcon(int type)
    {
        equip.SetUIEquipItemIcon(type);
    }
}
