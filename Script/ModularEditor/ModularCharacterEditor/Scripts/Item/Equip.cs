using BattleDrakeStudios.ModularCharacters;
using Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEquip", menuName = "Items/Equip")]
public class Equip : ScriptableObject
{
    public int EquipID;
    public string itemName;
    public bool ShowHead;
    public ArmorType armorType;
    public Sprite Icon;
    public BodyPartLinker[] armorParts;
}
