using Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class GameObjectManager : MonoSingleton<GameObjectManager>
{    
    protected override void OnStart()
    {
        CharacterManager.Instance.OnCharacterEnter += this.OnCharacterEnter;
        CharacterManager.Instance.OnCharacterExit += this.OnCharacterExit;
    }
    private void OnDestroy() 
    {
        CharacterManager.Instance.OnCharacterEnter -= this.OnCharacterEnter;
        CharacterManager.Instance.OnCharacterExit -= this.OnCharacterExit;
    }

    private void OnCharacterEnter(Character character)
    {
        Debug.LogFormat("CharacterEnter[{0}]", character.CharacterId);
        this.CreatCharacter(character);
    }

    public void OnCharacterExit(Character character)
    {
        //销毁Character
        Destroy(character.CharacterGameObject);
    }

    public void CreatCharacter(Character character, bool isPlayer = false)
    {
        string str = CharacterDefineTemp.CharacterPath;
        int[] characterDefineArray = character.ArmorIDS;
        //CharacterDefineTemp.characters[character.characterInfo.CharacterType];
        GameObject go = this.InitCharacter(str, isPlayer);
        go = Instantiate(go);
        character.CharacterGameObject = go;
        CharacterBase cb = go.GetComponent<CharacterBase>();
        cb.character = character;
        character.characterBase = cb;
        cb.InitCharacter(characterDefineArray);
        cb.Set(VectorTool.NVectorToVector(character.CharacterInfo.CurrentPosition),
                VectorTool.NVectorToVector(character.CharacterInfo.CurrentDirection));
    }

    public GameObject InitCharacter(string Path, bool isPlayer)
    {
        GameObject go = Resources.Load<GameObject>(Path);
        if (isPlayer)
        {
            go.AddComponent<PlayerInputController>();
            go.AddComponent<CharacterController>();
        }
        else
        {
            go.AddComponent<NetInputController>();
            go.AddComponent<NetCharacterController>();
        }
        return go;
    }

    internal void CreatCooperatorSign(int playerID, NVector3 summonPosition)
    {
        GameObject go = Resources.Load<GameObject>("SummonPlayer/SummonPlayerPrefab");
        go = Instantiate(go, VectorTool.NVectorToVector(summonPosition), Quaternion.identity);
        OtherPlayer op = go.GetComponent<OtherPlayer>();
        op.Init(playerID, VectorTool.NVectorToVector(summonPosition));
    }
}
