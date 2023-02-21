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
    internal class BattleService : Singleton<BattleService>, IDisposable
    {
        public BattleService() 
        {
            MessageDistributer<NetConnection>.Instance.Subscribe<AttackRequest>(this.OnAttackReq);
            MessageDistributer<NetConnection>.Instance.Subscribe<RollRequest>(this.OnRollReq);
            MessageDistributer<NetConnection>.Instance.Subscribe<EnemyGetHit>(this.OnEnemyHit);
            MessageDistributer<NetConnection>.Instance.Subscribe<CharacterGetHit>(this.OnCharacterHit);
        }

        public void Dispose()
        {
            MessageDistributer<NetConnection>.Instance.Unsubscribe<AttackRequest>(this.OnAttackReq);
            MessageDistributer<NetConnection>.Instance.Unsubscribe<RollRequest>(this.OnRollReq);
            MessageDistributer<NetConnection>.Instance.Unsubscribe<EnemyGetHit>(this.OnEnemyHit);
            MessageDistributer<NetConnection>.Instance.Unsubscribe<CharacterGetHit>(this.OnCharacterHit);
        }

        internal void Init()
        {
            
        }
        private void OnRollReq(NetConnection sender, RollRequest message)
        {
            Console.WriteLine(string.Format("Character [{0}] Roll", sender.Id));
            Room room = RoomManager.Instance.GetRoom(sender);
            if (room == null) return;

            foreach(var character in room.RoomMembers)
            {
                if (character.Id == sender.Id)
                    continue;
                character.session.Response.Rollresponse = new RollResponse();
                character.session.Response.Rollresponse.PlayerRollID= sender.Id;
                character.SendNetMessage();

            }
        }

        private void OnAttackReq(NetConnection sender, AttackRequest message)
        {
            Console.WriteLine(string.Format("Character [{0}] Attack", sender.Id));
            Room room = RoomManager.Instance.GetRoom(sender);
            if(room == null) return;
            foreach (var character in room.RoomMembers)
            {
                if (character.Id == sender.Id)
                    continue;
                character.session.Response.AttactResponse = new AttackResponse();
                character.session.Response.AttactResponse.PlayerAttackID= sender.Id;
                character.SendNetMessage();

            }
        }
        private void OnEnemyHit(NetConnection sender, EnemyGetHit message)
        {
            Console.WriteLine(string.Format("Enemy [{0}] Hit [{1}]", message.EnemyID, message.Damage));
            Room room = RoomManager.Instance.GetRoom(sender);
            foreach(var member in room.RoomMembers)
            {
                if(member.Id == sender.Id) 
                    continue;
                member.session.Response.EnemyGetHitResponse = message;
                member.SendNetMessage();
            }
        }
        private void OnCharacterHit(NetConnection sender, CharacterGetHit message)
        {
            Console.WriteLine(string.Format("Character [{0}] Hit [{1}]", message.CharacterID, message.Damage));
            Room room = RoomManager.Instance.GetRoom(sender);
            foreach (var member in room.RoomMembers)
            {
                if (member.Id == sender.Id)
                    continue;
                member.session.Response.CharacterGetHitResponse = message;
                member.SendNetMessage();
            }
        }

    }
}
