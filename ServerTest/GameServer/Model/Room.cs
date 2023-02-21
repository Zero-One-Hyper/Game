using GoBangServer.Servers;
using Managers;
using NetWork;
using Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Room
    {
        public Room(NetConnection owner, int roomId)
        {
            this.Owner = owner;
            this.RoomId = roomId;    
            RoomMembers.Add(owner);
        }
        public int RoomId;
        public NetConnection Owner;
        public List<NetConnection> RoomMembers = new List<NetConnection>();
        public List<Entity> EnemyMembers = new List<Entity>();
        public void AddMember(NetConnection player)
        {
            RoomMembers.Add(player);
        }
        public void RemoveMember(NetConnection player)
        {
            if(player == this.Owner)
            {
                this.Owner = null;
                this.RoomMembers.Clear();
            }
            else
            {
                this.RoomMembers.Remove(player);
            }
        }
        public bool HasMember(NetConnection player)
        {
            return this.RoomMembers.Contains(player);
        }

        public void SyncPlayer(EntitySyncMessage message)
        {

            foreach (var player in this.RoomMembers)
            {
                if (player.Id == message.CharacterID)
                {
                    EntityManager.Instance.SetEntitySyncData(message);
                }
                else
                {
                    EntityService.Instance.SendEntitySyncMessage(player, message);
                }
            }
        }
        public void AddEnemy()
        {

        }

        public void SyncEnemy(NetConnection sender, EnemyEntityRequest message)
        {
            foreach (var player in this.RoomMembers)
            {
                if (sender.entity.Id == player.Id)
                {
                    EntityManager.Instance.SetEnemyEntityData(message);
                }
                else
                {
                    EntityService.Instance.SendEnemyEntitySync(player, message);
                }
            }
        }
    }
}
