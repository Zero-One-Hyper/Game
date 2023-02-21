using BattleDrakeStudios.ModularCharacters;
using Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CharacterEquip
{
    const int No_Helement_Index = 26;
    private ModularCharacterManager ModularCharacterManager;
    private int[] Armor = new int[6] { 099, 199, 992, 993, 99, 99};
    private EquipData EquipData;
    public CharacterEquip(ModularCharacterManager modularCharacterManager, EquipData equipData)
    {
        this.ModularCharacterManager = modularCharacterManager;
#if UNITY_EDITOR
        this.EquipData = AssetDatabase.LoadAssetAtPath<EquipData>(@"Assets/SODatas/Armor Data/EquipData.asset");
#else
        this.EquipData = equipData;
#endif
    }
    public int[] Init(int[] armorTypeIDs = null)
    {
        //Debug.Log(this.ModularCharacterManager == null);
        /*
        if (armorTypeIDs != null)
            this.Armor = armorTypeIDs;
        */
        if (armorTypeIDs != null)
        {
            for (int i = 0; i < 4; i++)
            {
                this.EquipArmor(this.EquipData.AllEquips[armorTypeIDs[i]], false);
                //Debug.Log(this.Armor[i]);
                //EquipManager.Instance.PlayerEquipArmor(this.EquipData.AllEquips[armor], false);
                //this.EquipArmor(this.EquipData.AllEquips[armor], false);
            }
        }
        return this.Armor;
    }
    public void NEquipArmor(ArmorType type, int EquipID)
    {
        if (EquipData.AllEquips.Count < EquipID)
            return;
        Equip equip = EquipData.AllEquips[EquipID]; 
        if (equip.armorType != type)
        {
            Debug.LogFormat("ArmorType Error ID:[{0}] Error", EquipID);
            return;
        }
        this.DoEquip(equip);
    }
    public void EquipArmor(Equip equip, bool sendMessage = true)
    {
        //Equip equip = EquipData.AllEquips[equipID];
        this.DoEquip(equip);
        if (sendMessage && NetClient.Instance != null && NetClient.Instance.IsConnected)
        {
            EquipService.Instance.SendEquipArmorMesg(equip.armorType, equip.EquipID);
        }
    }
    private void DoEquip(Equip equip)
    {
        if (equip != null)
        {
            if (this.Armor[(int)equip.armorType] == equip.EquipID)
            {
                if (equip.EquipID >= No_Helement_Index && equip.EquipID <= No_Helement_Index + 3)
                    return;
                // this.ModularCharacterManager.ActivatePart();
                this.UnEquip(equip.armorType);
            }

            if (equip.armorType == ArmorType.Helmet)
            {
                if (equip.ShowHead)
                    this.ModularCharacterManager.ActivatePart(ModularBodyPart.头部, 0);
                else
                    this.ModularCharacterManager.DeactivatePart(ModularBodyPart.头部);
            }
            this.Armor[(int)equip.armorType] = equip.EquipID;
            this.ModularCharacterManager.ActiveArmor(equip.armorType, equip.armorParts);
        }
    }
    public void UnEquip(ArmorType type, bool sendMessage = true)
    {
        Equip equip = EquipData.AllEquips[(int)(type) + No_Helement_Index];
        DoEquip(equip);
        if (sendMessage && NetClient.Instance != null && NetClient.Instance.IsConnected && User.Instance.inRoom)
        {
            EquipService.Instance.SendEquipArmorMesg(equip.armorType, equip.EquipID);
        }
    }
    
    public int GetActiveEquipArmor(ArmorType type)
    {
        return this.Armor[(int)type];
    }
    public int[] GetAllEquipArmors()
    {
        return this.Armor;
    }
#if UNITY_EDITOR
    public void DisActivePart(ModularBodyPart modularBodyPart)
    {
        this.ModularCharacterManager.DeactivatePart(modularBodyPart);
    }
    public void DisActivePart(ArmorType armorType)
    {
        switch(armorType)
        {
            case ArmorType.Helmet:
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.头盔);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.头部附件);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.帽子);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.面具);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.头套);
                break;
            case ArmorType.BodyArmor:
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.背部附件);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.躯干);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.右侧肩部附件);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.左侧肩部附件);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.右侧上臂);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.左侧上臂);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.臀部_腰部附件);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.臀部_腰部);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.头发);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.头部);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.面部毛发);
                break;
            case ArmorType.Glove:
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.右侧手臂关节);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.左侧手臂关节);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.右侧下臂);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.左侧下臂);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.右手);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.左手);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.头发);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.头部);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.面部毛发);
                break;
            case ArmorType.Legs:
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.右侧膝盖);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.左侧膝盖);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.右脚);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.左脚);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.头发);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.头部);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.面部毛发);
                break;
        }
    }
#endif

}
