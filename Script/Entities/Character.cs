using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;
using System.Linq;

public struct CharacterState
{
    public float MaxHealth;
    public float MaxEnergy;

    public float CurrentHealth;
    public float CurrentEnergy;
}
public class CharacterDataDefine
{
    public static float EnergyRecover = 1.0f;

    public static float AttackCost = 22.0f;
    public static float RollCost = 10.0f;
}
public class Character : Entity
{
    private int characterID;
    public int CharacterId
    {
        get
        {
            return characterID;
        }
        set
        {
            characterID = value;
            this.EntityID = value;
        }
    }

    public CharacterBase characterBase;
    public GameObject CharacterGameObject;

    public int[] ArmorIDS = new int[6];

    public float AttackDamage = 20.0f;

    public CharacterState characterState = new CharacterState()
    {
        MaxHealth = 100.0f,
        MaxEnergy = 100.0f,
        
        CurrentHealth = 100.0f,
        CurrentEnergy = 100.0f,
    };
    public NCharacterInfo CharacterInfo
    {
        get
        {
            this.EntityHealth = this.characterState.CurrentHealth;
            NCharacterInfo info = new NCharacterInfo()
            {
                CharacterID = this.CharacterId,
                CurrentPosition = this.NPosition,
                CurrentDirection = this.NDirection,
                CharacterHealth = this.NEntityHealth
            };
            info.ArmorIDs.Add(this.ArmorIDS);
            return info;
        }        
        set
        {
            this.CharacterId = value.CharacterID;
            this.NPosition = value.CurrentPosition;
            this.NDirection = value.CurrentDirection;

            this.ArmorIDS = value.ArmorIDs.ToArray();

            this.NEntityHealth = value.CharacterHealth;
            this.characterState.CurrentHealth = this.EntityHealth;
        }
    }        
    public Character(NCharacterInfo info)
    {
        this.CharacterInfo = info;

        /*
        //后续做在某点画符召唤
        this.CallPosition = this.characterInfo.CurrentPosition;
        this.CallDirection = this.characterInfo.CurrentDirection;
        
        */        
    }
    public Character(CharacterBase cb)
    {
        //this.characterInfo = new NCharacterInfo();
        this.characterBase = cb;
    }

    public void CharacterAttack()
    {
        this.characterBase.inputController.DoAttack();
    }
    public void CharacterRoll()
    {
        this.characterBase.inputController.DoRoll();
    }
    public void NEquipArmor(ArmorType type, int EquipID)
    {
        this.characterBase.NEquipArmor(type, EquipID);    
    }
}
