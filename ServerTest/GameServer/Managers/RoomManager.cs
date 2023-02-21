using GoBangServer.Servers;
using Managers;
using Model;
using NetServerTools;
using NetWork;
using Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Managers
{
    public class RoomManager : Singleton<RoomManager>
    {
        public Dictionary<int, Room> AllRooms = new Dictionary<int, Room>();

        public Room GetRoom(NetConnection player)
        {
            foreach (var room in this.AllRooms.Values)
            { 
                if(room.HasMember(player))
                { 
                    return room; 
                }    
            }
            return null;    
        }
        public Room GetRoom(int RoomID)
        {
            if(this.AllRooms.TryGetValue(RoomID, out Room room)) 
                return room;
            return null;
        }

        public int EnterRoom(NetConnection player)
        {
            foreach(var room in AllRooms.Values)
            {
                if(room.HasMember(player))
                {
                    room.RemoveMember(player);
                }
            }
            foreach(var room in AllRooms.Values)
            {
                if(room.Owner == null)
                {
                    room.AddMember(player);
                    room.Owner = player;
                    this.AllRooms.Add(this.AllRooms.Count, room);
                    Console.WriteLine(string.Format("User[{0}] Creat a Room, RoomId[{1}]", player.Id, room.RoomId));
                    return room.RoomId;
                }
            }
            Room newRoom = new Room(player, AllRooms.Count);
            this.AllRooms.Add(this.AllRooms.Count, newRoom);
            Console.WriteLine(string.Format("User[{0}] Creat a Room, RoomId[{1}]", player.Id, newRoom.RoomId));
            return newRoom.RoomId;
        }
        public bool EnterRoom(NetConnection connection, int RoomID)
        {
            if(AllRooms.TryGetValue(RoomID, out Room room))
            {
                room.AddMember(connection);
                return true;
            }
            return false;

        }
        public void ExitRoom(NetConnection player)
        {
            foreach(var room in AllRooms)
            {
                if(room.Value.HasMember(player))
                {
                    room.Value.RemoveMember(player);                    
                    break;
                }
            }
        }
        internal void EntitySync(NetConnection player, EntitySyncMessage message)
        {
            Room SyncRoom = null;
            foreach(var room in AllRooms)
            {
                if (room.Value.HasMember(player))
                {
                    SyncRoom = room.Value;
                    break;
                }
            }
            if (SyncRoom == null) return;
            SyncRoom.SyncPlayer(message);
        }

        internal void UploadRoom(NetConnection sender, UploadEnemyInfosRequest message)
        {
            Room PlayerRoom = RoomManager.Instance.GetRoom(sender);
            foreach(var NEnemyInfo in message.NEnemyInfos)
            {
                //PlayerRoom.
            }
        }
        internal void EnemyEntitySync(NetConnection sender, EnemyEntityRequest message)
        {
            Room SyncRoom = null;
            foreach (var room in AllRooms)
            {
                if (room.Value.HasMember(sender))
                {
                    SyncRoom = room.Value;
                    break;
                }
            }
            if (SyncRoom == null) return;
            SyncRoom.SyncEnemy(sender, message);            
        }

    }
}
