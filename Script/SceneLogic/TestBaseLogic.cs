using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBaseLogic : SceneLogic//, MonoBehaviour
{
    public static bool isPlaying = false;
    public GameObject PlayerCharacter;
    public Equip Equip1;
    public Equip Equip2;
    public EquipData EquipData;
    // Start is called before the first frame update
    void Start()
    {        
        UIInput.Instance.Init();
        BagManager.Instance.Init();
        //UIInput.Instance
        if (User.Instance.CharacterTypeID != 0)
        {
            InitCharacter(); 
            //this.TestCharacter.SetActive(false);
        }
        else
        {
            CameraManager.Instance.InitAim();            
            PlayerCharacter.GetComponent<CharacterBase>().InitCharacter(CharacterDefineTemp.characters[User.Instance.CharacterTypeID]);
            InputManager.Instance.EnableInputSetting(InputType.UI);
        }
        BagManager.Instance.AddEquip(EquipData.AllEquips[12]);
        BagManager.Instance.AddEquip(EquipData.AllEquips[13]);
        BagManager.Instance.AddEquip(EquipData.AllEquips[14]);
        BagManager.Instance.AddEquip(EquipData.AllEquips[15]);
        BagManager.Instance.AddEquip(EquipData.AllEquips[0]);
        BagManager.Instance.AddEquip(EquipData.AllEquips[1]);
        BagManager.Instance.AddEquip(EquipData.AllEquips[2]);
        BagManager.Instance.AddEquip(EquipData.AllEquips[3]);
        BagManager.Instance.AddEquip(EquipData.AllEquips[16]);
        BagManager.Instance.AddEquip(EquipData.AllEquips[18]);
        BagManager.Instance.AddEquip(EquipData.AllEquips[19]);
        BagManager.Instance.AddEquip(EquipData.AllEquips[20]);
        //AnimEventCenter.Instance.Init();

        isPlaying = true;
    }

    private void InitCharacter()
    {
        //Debug.Log("TestBaseLogic");
        //string characterPath = CharacterDefineTemp.CharacterPath;        
        //GameObject go = GameObjectManager.Instance.InitCharacter(characterPath, true);
        //go = Instantiate(go);
        CharacterBase cb = PlayerCharacter.GetComponent<CharacterBase>();

        cb.isPlayer = true;
        cb.character = new Character(cb)
        {
            CharacterId = User.Instance.PlayerId,

        };

        //暂时 后续放在其他地方
        User.Instance.UserCharacter = cb.character;

        cb.InitCharacter(CharacterDefineTemp.characters[User.Instance.CharacterTypeID]);
        cb.character.ArmorIDS = cb.GetEquips();


        this.OnCreatCharacter(cb.CameraAim, cb.CameraFollow);

        PlayerCharacter.transform.position = this.transform.position;

       

    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (NetClient.Instance.IsConnected)
                this.OnEnterGameServer();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayerCharacter.GetComponent<CharacterBase>().EquipArmor(Equip1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayerCharacter.GetComponent<CharacterBase>().EquipArmor(Equip2);
        }
        */
    }/*
    public void OnEnterGameServer()
    {
        MapService.Instance.SendEnterGameServer();
    }*/
    public void OnCreatCharacter(Transform aim, Transform foll)
    {
        CameraManager.Instance.InitAim(aim, foll);
    }
}
