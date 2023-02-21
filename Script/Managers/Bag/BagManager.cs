using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class BagManager :Singleton<BagManager>
{
    public BagManager()
    {
       
    }
    public void Init()
    {
        AllEquips.Clear();
    }
    public List<Equip> AllEquips = new List<Equip>();

    public Dictionary<int, Equip> Equips = new Dictionary<int, Equip>();

    public void AddEquip(Equip equip)
    {
        this.AllEquips.Add(equip);

        this.Equips.TryAdd(equip.EquipID, equip);
    }

}
