using BattleDrakeStudios.ModularCharacters;
using Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    [Header("装备数据")]
    public EquipData AllEquips;
    [Header("模型引用")]
    public GameObject Modle;

    public Character character;
    [Header("虚拟相机瞄准点")]
    public Transform CameraAim;
    public Transform CameraFollow;
    [Header("组件控制器")]
    public IInputController inputController;
    public ICharacterController characterController;

    public Func<PlayerAnimEventArg> OnAnimPlay;

    [SerializeField]
    private PlayerAnimEventArg eventarg;

    [SerializeField]
    private ModularCharacterManager modularCharacterManager;
    private CharacterEquip CharacterEquip;
    public bool isPlayer = false;
#if UNITY_EDITOR
    private bool isPlaying = true;
#endif

    private bool Invincible_frames = false;
    private bool isBlock = false;

    public WeaponHandler WeaponHandler;
    public void InitCharacter(int[] armorPartIDs = null)
    {
#if UNITY_EDITOR
        //Debug.Log("CharacterBase");
        if (User.Instance.UserCharacter == null)
        {
            if (this.character == null)
                this.character = new Character(this);
            User.Instance.UserCharacter = this.character;
            isPlaying = false;
        }
#endif
        this.modularCharacterManager = this.GetComponentInChildren<ModularCharacterManager>();
        this.CharacterEquip = new CharacterEquip(this.modularCharacterManager, AllEquips);
        this.CharacterEquip.Init(armorPartIDs);
    }
    private void Awake()
    {
        this.inputController = this.GetComponent<IInputController>();
        this.characterController = this.GetComponent<ICharacterController>();
    }
    private void Start()
    {
        AnimEventCenter.Instance.AddListener("OpenInvincibleFrames", this.OpenInvincibleFrames);
        AnimEventCenter.Instance.AddListener("CloseInvincibleFrames", this.CloseInvincibleFrames);
    }

    private void FixedUpdate()
    {
#if UNITY_EDITOR
        if (!isPlaying) return;
#endif
        this.UpdateEntityTransform();
        this.UpdateEntityAnim();
    }

    private void UpdateEntityAnim()
    {
        if(!isPlayer)
            this.inputController.SetAnim((PlayerAnimEventArg)this.character.AnimEventArg);
    }
    int lateSend = 2;
    private void UpdateEntityTransform()
    {
        //不是本地的玩家 就从entity里拿位置信息
        if (!this.isPlayer)
        {
            this.transform.position = this.character.Position;
            this.Modle.transform.forward = this.character.Direction;
        }
        else 
        {
            if (lateSend % 2 != 0) return;
            if (lateSend > 100)
                lateSend = 1;
            //是本地玩家 就把位置信息写道entity里
            character.EntityUpdate(this.transform.position, this.Modle.transform.forward);

            //写完发送
            if (this.character.CanPosDirUpdate())
            {
                this.eventarg = this.OnAnimPlay?.Invoke();
                MapService.Instance.SendEntitySync(new EntitySyncMessage()
                {
                    Position = this.character.NPosition,
                    Direction = this.character.NDirection,

                    IsLock = this.eventarg.isLocked,
                    XSpeed = (int)(eventarg.speedX * 100),
                    YSpeed = (int)(eventarg.speedY * 100),
                    IsRunning = this.eventarg.isRunning,
                    IsUseShield = this.eventarg.isUseShield,
                });
            }
        }
    }

    public void Set(Vector3 postion, Vector3 forward)
    {
        this.transform.position = postion;
        this.Modle.transform.forward = forward;
    }
    public int[] GetEquips()
    {
        return this.CharacterEquip.GetAllEquipArmors();
    }
    public void NEquipArmor(ArmorType type, int EquipID)
    {
        this.CharacterEquip.NEquipArmor(type, EquipID);
    }
    public void EquipArmor(Equip equip)
    {
        this.CharacterEquip.EquipArmor(equip);
    }
    public void UnEquipArmor(ArmorType type)
    {
        this.CharacterEquip.UnEquip(type);
    }
    public void CloseInvincibleFrames(object sender, EventArgs e)
    {
        Invincible_frames = false;
    }
        #region 编辑器代码自动生成Icon
#if UNITY_EDITOR
        public void SceenShotEquip(int IconID, ArmorType armorType)
    {
        EquipData data = AssetDatabase.LoadAssetAtPath<EquipData>(@"Assets/SODatas/Armor Data/EquipData.asset");
        this.EquipArmor(data.AllEquips[IconID]);
        for(int i = 0; i < 4; i++)
        {           
            if(i == (int)ArmorType.Glove)
            {
                this.CharacterEquip.DisActivePart(ModularBodyPart.左侧手臂关节);
                this.CharacterEquip.DisActivePart(ModularBodyPart.左侧下臂);
                this.CharacterEquip.DisActivePart(ModularBodyPart.左手);
                this.CharacterEquip.DisActivePart(ModularBodyPart.头发);
                this.CharacterEquip.DisActivePart(ModularBodyPart.头部);
                this.CharacterEquip.DisActivePart(ModularBodyPart.面部毛发);
            }
            if ((int)armorType == i)
                continue;
            HideArmor((ArmorType)i);
        }
    }
    public void HideArmor(ArmorType armorType)
    {
        this.CharacterEquip.DisActivePart(armorType);
    }
#endif
    #endregion
    public void GetHit(float attackDamage)
    {
        if (Invincible_frames) return;
        if (this.isBlock)
        {
            attackDamage /= 5;
            this.character.characterState.CurrentEnergy -= 20.0f;
            if (this.character.CharacterId == User.Instance.PlayerId)
                UImainCanvas.Instance.UICharacterState.EditEnergy(20.0f);
        }
        this.character.characterState.CurrentHealth -= attackDamage;
        if(this.character.characterState.CurrentHealth <= 0)
        {
            GameManager.Instance.OnPlayerDead?.Invoke(this.character.CharacterId);
            this.characterController.DoDead();
            InputManager.Instance.DisAbleInputSetting(InputType.Player);
            this.transform.GetComponent<Rigidbody>().useGravity = false;
            this.transform.GetComponent<Collider>().enabled = false;    
        }
        if (this.character.CharacterId == User.Instance.PlayerId)
            UImainCanvas.Instance.UICharacterState.EditHealth(attackDamage);

        this.characterController.DoHit();
    }
    public void OpenInvincibleFrames(object sender, EventArgs e)
    {
        Invincible_frames = true;
    }
    internal void Block(bool isUseShield)
    {
        this.isBlock = isUseShield;
    }
}
