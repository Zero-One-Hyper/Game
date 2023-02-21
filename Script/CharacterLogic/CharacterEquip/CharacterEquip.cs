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
                    this.ModularCharacterManager.ActivatePart(ModularBodyPart.ͷ��, 0);
                else
                    this.ModularCharacterManager.DeactivatePart(ModularBodyPart.ͷ��);
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
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.ͷ��);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.ͷ������);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.ñ��);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.���);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.ͷ��);
                break;
            case ArmorType.BodyArmor:
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.��������);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.����);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.�Ҳ�粿����);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.���粿����);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.�Ҳ��ϱ�);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.����ϱ�);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.�β�_��������);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.�β�_����);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.ͷ��);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.ͷ��);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.�沿ë��);
                break;
            case ArmorType.Glove:
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.�Ҳ��ֱ۹ؽ�);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.����ֱ۹ؽ�);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.�Ҳ��±�);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.����±�);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.����);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.����);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.ͷ��);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.ͷ��);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.�沿ë��);
                break;
            case ArmorType.Legs:
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.�Ҳ�ϥ��);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.���ϥ��);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.�ҽ�);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.���);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.ͷ��);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.ͷ��);
                this.ModularCharacterManager.DeactivatePart(ModularBodyPart.�沿ë��);
                break;
        }
    }
#endif

}
