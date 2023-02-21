using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using Protocol;
using System;
using Unity.VisualScripting;

public class MapService : Singleton<MapService>, IDisposable
{
    //public bool InRoom = false;
    public MapService()
    {
        MessageDistributer.Instance.Subscribe<SummonCooperatorSignRequest>(this.OnSummonSign);
        MessageDistributer.Instance.Subscribe<SummonCooperatorResponse>(this.OnSummonRresponse);
        MessageDistributer.Instance.Subscribe<UserExitResponse>(this.OnCharaExit);
        MessageDistributer.Instance.Subscribe<EntitySyncResponse>(this.OnEntitySync);
    }
    public void Dispose()
    {
        MessageDistributer.Instance.Unsubscribe<SummonCooperatorSignRequest>(this.OnSummonSign);
        MessageDistributer.Instance.Unsubscribe<SummonCooperatorResponse>(this.OnSummonRresponse);
        MessageDistributer.Instance.Unsubscribe<UserExitResponse>(this.OnCharaExit);
        MessageDistributer.Instance.Unsubscribe<EntitySyncResponse>(this.OnEntitySync);

    }
    public void Init()
    {

    }
    private void OnSummonSign(object sender, SummonCooperatorSignRequest message)
    {
        GameObjectManager.Instance.CreatCooperatorSign(message.PlayerID, message.SummonPosition);
    }
    private void OnSummonRresponse(object sender, SummonCooperatorResponse message)
    {
        //SummorID是房主的ID
        if (message.SummorID != User.Instance.PlayerId)
        {
            User.Instance.inRoom = true;
            EnemyManager.Instance.SwitchEnemyControlType?.Invoke(ControlType.NET);
            Debug.Log("Enemy ControlTypeChange!!");
        }
        Debug.LogFormat("OnCharacterEnter characterID: [{0}]", message.CharacterInfos.CharacterID);
        //交给对应的manager去生成对应Type的角色
        //这个地方new Character（entity） 本地玩家在第一次进入场景的时候New
        Character character = new Character(message.CharacterInfos);
        CharacterManager.Instance.AddCharacter(character);
    }

    public void SendSummonSign(int CooperatorID, Vector3 SummonPosition, Vector3 SummonDirection)
    {
        Debug.Log("MapService Send Summon Sign");
        NetMessage message = new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.SummonCooperatorSignRequest = new SummonCooperatorSignRequest();
        message.Request.SummonCooperatorSignRequest.NCharacterInfo = User.Instance.UserCharacter.CharacterInfo;
        message.Request.SummonCooperatorSignRequest.PlayerID= CooperatorID;
        message.Request.SummonCooperatorSignRequest.SummonPosition = VectorTool.VectorToNVector(SummonPosition);
        message.Request.SummonCooperatorSignRequest.SummonDirection = VectorTool.VectorToNVector(SummonDirection);

        NetClient.Instance.SendMessage(message);
    }
    public void SendSummonRequest(int CooperatorID, Vector3 SummonPosition, Vector3 SummonDirection)
    {
        Debug.Log("MapService Send Summon Request");
        NetMessage message = new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.SummonCooperatorRequest = new SummonCooperatorRequest();
        message.Request.SummonCooperatorRequest.PlayerId = CooperatorID;
        message.Request.SummonCooperatorRequest.CallPosition = VectorTool.VectorToNVector(SummonPosition);
        message.Request.SummonCooperatorRequest.CallDirection = VectorTool.VectorToNVector(SummonDirection);       
        message.Request.SummonCooperatorRequest.SummonPosition = User.Instance.UserCharacter.NPosition;
        message.Request.SummonCooperatorRequest.SummonDirection = User.Instance.UserCharacter.NDirection;

        message.Request.SummonCooperatorRequest.NCharacterInfo = User.Instance.UserCharacter.CharacterInfo;
        NetClient.Instance.SendMessage(message);

        //EnemyManager.Instance.

    }
    
    private void OnCharaExit(object sender, UserExitResponse message)
    {
        Debug.LogFormat("OnCharacterExit: [{0}]", message.LeaveUserId);
        
        CharacterManager.Instance.RemoveCharacter(message.LeaveUserId);
    }

    public void SendCharaExit()
    {
        if (!NetClient.Instance.IsConnected) return;
        //登录的时候就给到了PlayerID 所以只要有连接 就有ID

        NetMessage message = new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.UserExitRequest= new UserExitRequest();
        message.Request.UserExitRequest.LeaveUserId = User.Instance.PlayerId;
        NetClient.Instance.SendMessage(message);
    }
    
    private void OnEntitySync(object sender, EntitySyncResponse message)
    {
        //Debug.Log(586997631);

        foreach(var data in message.EntitySyncMessages)
        {
            EntityManager.Instance.OnEntitySync(data);
        }
    }

    public void SendEntitySync(EntitySyncMessage SyncMessage)
    {
        NetMessage message = new NetMessage();
        message.Request = new NetMessageRequest();
        message.Request.EntitySyncRequest = new EntitySyncRequest();
        message.Request.EntitySyncRequest.EntitySyncMessage = SyncMessage;
        message.Request.EntitySyncRequest.EntitySyncMessage.CharacterID = User.Instance.UserCharacter.CharacterId;
        
        NetClient.Instance.SendMessage(message);
    }
}
