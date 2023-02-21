using Common;
using Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleService : Singleton<BattleService>, IDisposable
{
    public BattleService() 
    {
        MessageDistributer.Instance.Subscribe<AttackResponse>(this.OnCharaAttack);
        MessageDistributer.Instance.Subscribe<RollResponse>(this.OnCharaRoll);
        MessageDistributer.Instance.Subscribe<CharacterGetHit>(this.OnCharaHit);
    }

    public void Dispose()
    {
        MessageDistributer.Instance.Unsubscribe<AttackResponse>(this.OnCharaAttack);
        MessageDistributer.Instance.Unsubscribe<RollResponse>(this.OnCharaRoll);
        MessageDistributer.Instance.Unsubscribe<CharacterGetHit>(this.OnCharaHit);

    }
    internal void Init()
    {

    }

    private void OnCharaAttack(object sender, AttackResponse message)
    {
        BattleManager.Instance.OnCharaAttack(message.PlayerAttackID);
    }

    public void CharacterAttack()
    {
        NetMessage message= new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.AttackRequest = new AttackRequest();
        NetClient.Instance?.SendMessage(message);
    }

    private void OnCharaRoll(object sender, RollResponse message)
    {
        BattleManager.Instance.OnCharaRoll(message.PlayerRollID);
    }
    public void CharacterRoll()
    {
         NetMessage message = new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.RollRequest = new RollRequest();
        NetClient.Instance?.SendMessage(message);
    }
    private void OnCharaHit(object sender, CharacterGetHit message)
    {
        Character character = CharacterManager.Instance.GetCharacter(message.CharacterID);
        character.characterBase.GetHit(message.Damage / 100.0f);
    }
    public void SendCharacterHit(int characterID, int damage)
    {
        NetMessage message = new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.CharacterGetHitRequest = new CharacterGetHit()
        {
            CharacterID = characterID,
            Damage = damage,
        };
        NetClient.Instance?.SendMessage(message);
    }
}
