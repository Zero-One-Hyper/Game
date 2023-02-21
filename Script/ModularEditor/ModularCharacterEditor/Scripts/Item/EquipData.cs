using BattleDrakeStudios.ModularCharacters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "AllEquip", menuName = "Items/AllEquip")]
public class EquipData : ScriptableObject
{
    public List<Equip> AllEquips = new List<Equip>();
}
