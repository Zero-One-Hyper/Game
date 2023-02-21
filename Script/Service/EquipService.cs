using Common;
using Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;

public class EquipService : Singleton<EquipService>, IDisposable
{
    public EquipService() 
    {
        MessageDistributer.Instance.Subscribe<EquipArmor>(this.OnEquipArmor);
    }

    public void Dispose()
    {
        MessageDistributer.Instance.Unsubscribe<EquipArmor>(this.OnEquipArmor);
    }

    public void Init()
    {

    }
    
    private void OnEquipArmor(object sender, EquipArmor message)
    {
        Character character = CharacterManager.Instance.GetCharacter(message.CharacterID);
        character.NEquipArmor(message.ArmorType, message.EquipID);
    }

    public void SendEquipArmorMesg(ArmorType type, int equipID)
    {
        //Debug.LogFormat("SendEquipArmorMesg [{0}]:[{1}]", type, equipID);
        NetMessage message = new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.EquipArmor = new EquipArmor()
        {
            CharacterID = User.Instance.UserCharacter.CharacterId,
            EquipID = equipID,
            ArmorType = type,
        };
        NetClient.Instance.SendMessage(message);
    }
}
