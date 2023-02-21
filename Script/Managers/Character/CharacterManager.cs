using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class CharacterDefineTemp
{
    //static string Character_01 = "CharacterTemp/ModularCharacter_01";
    public static string CharacterPath = "CharacterTemp/Player";
    static int[] Character_01 = { 0, 1, 2, 3 };
    static int[] Character_02 = { 16, 18, 19, 20 };
    static int[] Character_03 = { 26, 27, 28, 29};
    public static List<int[]> characters = new List<int[]>()
    { 
        null, Character_01, Character_02, Character_03
    };
    

}

public class CharacterManager : Singleton<CharacterManager>
{
    //事件由外部注册 用于生成角色 或者 Destory角色
    public UnityAction<Character> OnCharacterEnter;
    public UnityAction<Character> OnCharacterExit;

    public UnityAction OnChareacterEnterCallBack;

    public Dictionary<int, Character> Characters = new Dictionary<int, Character>();
    public CharacterManager()
    {

    }

    public void AddCharacter(Character character)
    {
        Debug.LogFormat("AddCharacter:[{0}]", character.CharacterId);
        this.Characters[character.CharacterId] = character;
        EntityManager.Instance.AddEntity(character);
        this.OnCharacterEnter?.Invoke(character);
        this.OnChareacterEnterCallBack?.Invoke();
    }

    public void RemoveCharacter(int characterId)
    {
        Debug.LogFormat("RemoveCharacter:[{0}}", characterId);
        if(this.Characters.ContainsKey(characterId))
        {
            OnCharacterExit?.Invoke(this.Characters[characterId]);
            EntityManager.Instance.RemoveEntity(this.Characters[characterId]);
            this.Characters.Remove(characterId);        
        }
    }
    public Character GetCharacter(int characterId)
    {
        if(Characters.TryGetValue(characterId, out Character character))
        {
            return character;
        }
        return null;
    }

    internal void OnCharacterAttack(int playerAttackID)
    {
        if(Characters.TryGetValue(playerAttackID, out Character character))
        {
            character.CharacterAttack();
        }
    }
    public void OnCharacterRoll(int playerRollID)
    {
        if (Characters.TryGetValue(playerRollID, out Character character))
        {
            character.CharacterRoll();
        }
    }
}
