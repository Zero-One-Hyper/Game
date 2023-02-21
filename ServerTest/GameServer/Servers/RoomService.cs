using Common;
using Managers;
using Model;
using NetServerTools;
using NetWork;
using Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servers
{
    internal class RoomService : Singleton<RoomService>, IDisposable
    {
        public RoomService() 
        {
            MessageDistributer<NetConnection>.Instance.Subscribe<UploadEnemyInfosRequest>(this.OnEnemyInfoUpload);
            MessageDistributer<NetConnection>.Instance.Subscribe<EnemyEntityRequest>(this.OnEnemyEntityRequest);
            MessageDistributer<NetConnection>.Instance.Subscribe<EnemyAttackMessage>(this.OnEnemyAttack);
            MessageDistributer<NetConnection>.Instance.Subscribe<SummonCooperatorSignRequest>(this.OnSummonSign);
            MessageDistributer<NetConnection>.Instance.Subscribe<SummonCooperatorRequest>(this.OnSummonCoopratorRequest);
        }


        public void Dispose()
        {            
            MessageDistributer<NetConnection>.Instance.Unsubscribe<UploadEnemyInfosRequest>(this.OnEnemyInfoUpload);
            MessageDistributer<NetConnection>.Instance.Unsubscribe<EnemyEntityRequest>(this.OnEnemyEntityRequest);
            MessageDistributer<NetConnection>.Instance.Unsubscribe<EnemyAttackMessage>(this.OnEnemyAttack);
            MessageDistributer<NetConnection>.Instance.Unsubscribe<SummonCooperatorSignRequest>(this.OnSummonSign);
            MessageDistributer<NetConnection>.Instance.Unsubscribe<SummonCooperatorRequest>(this.OnSummonCoopratorRequest);
        }
        public void Init()
        {

        }
        private void OnSummonSign(NetConnection sender, SummonCooperatorSignRequest message)
        {
            sender.entity.NCharacterInfo = message.NCharacterInfo;
            Console.WriteLine(string.Format("收到召唤信号 [{0}]", message.PlayerID));
            //召唤标记需要发给所有玩家
            foreach(var player in CharacterManager.Instance.CharacterList)
            {
                if (player == sender)
                    continue;
                player.session.Response.SummonCooperatorSignRequest = new SummonCooperatorSignRequest();
                player.session.Response.SummonCooperatorSignRequest.PlayerID = message.PlayerID;
                player.session.Response.SummonCooperatorSignRequest.SummonPosition = message.SummonPosition;
                player.session.Response.SummonCooperatorSignRequest.SummonDirection = message.SummonDirection;
                player.SendNetMessage();
            }
        }
        //玩家召唤记号所有者
        private void OnSummonCoopratorRequest(NetConnection sender, SummonCooperatorRequest message)
        {
            int RoomId = RoomManager.Instance.EnterRoom(sender);
            NetConnection cha = CharacterManager.Instance.GetCharacter(message.PlayerId);

            RoomManager.Instance.EnterRoom(cha, RoomId);

            sender.entity.NCharacterInfo = message.NCharacterInfo;

            sender.session.Response.SummonCooperatorResponse= new SummonCooperatorResponse();
            cha.entity.Position = message.CallPosition;
            cha.entity.Direction = message.CallDirection;
            sender.session.Response.SummonCooperatorResponse.SummorID = sender.Id;
            sender.session.Response.SummonCooperatorResponse.CharacterInfos = cha.entity.NCharacterInfo;
            sender.SendNetMessage();

            cha.session.Response.SummonCooperatorResponse = new SummonCooperatorResponse();
            sender.entity.Position = message.SummonPosition;
            sender.entity.Direction = message.SummonDirection;
            cha.session.Response.SummonCooperatorResponse.SummorID = sender.Id;
            cha.session.Response.SummonCooperatorResponse.CharacterInfos = sender.entity.NCharacterInfo;
            cha.SendNetMessage();
            Console.WriteLine(string.Format("Character [{0}] Summon Cooperator [{1}]", sender.Id, cha.Id));
        }

        private void OnEnemyInfoUpload(NetConnection sender, UploadEnemyInfosRequest message)
        {
            RoomManager.Instance.UploadRoom(sender, message);
        }

        private void OnEnemyEntityRequest(NetConnection sender, EnemyEntityRequest message)
        {
            //Console.WriteLine(string.Format("EnemeyEntity [{0}]", message.EnemyId));
            RoomManager.Instance.EnemyEntitySync(sender, message);
        }
        private void OnEnemyAttack(NetConnection sender, EnemyAttackMessage message)
        {
            Console.WriteLine(string.Format("EnemeyAttackEntity [{0}]", message.EnemyID));
            Room room = RoomManager.Instance.GetRoom(sender);
            if (room == null)
                return;
            foreach(var member in room.RoomMembers)
            {
                if(member.Id == sender.Id)
                {
                    continue;
                }
                member.session.Response.EnemyAttackMsg = message;
                member.SendNetMessage();
            }
        }
    }
}
