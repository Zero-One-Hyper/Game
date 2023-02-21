using Common;
using Google.Protobuf;
using Managers;
using Managers;
using NetServerTools;
using NetWork;
using Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoBangServer.Servers
{
    internal class EntityService : Singleton<EntityService>, IDisposable
    {
        public EntityService() 
        {
            MessageDistributer<NetConnection>.Instance.Subscribe<EntitySyncRequest>(this.OnEntitySyncRequest);
        }
        public void Dispose()
        {
            MessageDistributer<NetConnection>.Instance.Unsubscribe<EntitySyncRequest>(this.OnEntitySyncRequest);
        }
        public void Init()
        {

        }

        private void OnEntitySyncRequest(NetConnection sender, EntitySyncRequest message)
        {     
            /*
            Console.WriteLine(string.Format("Character:[{0}] Position:[{1}] Direction:[{2}]",
                message.EntitySyncMessage.CharacterID, message.EntitySyncMessage.Position, message.EntitySyncMessage.Direction));
            */
            RoomManager.Instance.EntitySync(sender, message.EntitySyncMessage);        
        }

        public void SendEntitySyncMessage(NetConnection sender, EntitySyncMessage message)
        {
            Console.WriteLine(string.Format("SendEntitySync [{0}]", sender.Id));
            sender.session.Response.EntitySyncResponse = new EntitySyncResponse();
            sender.session.Response.EntitySyncResponse.EntitySyncMessages.Add(message);
            
            sender.SendNetMessage();
        }

        internal void SendEnemyEntitySync(NetConnection member, EnemyEntityRequest message)
        {
            Console.WriteLine(string.Format("SendEnemySync [{0}]", message.EnemyId));
            member.session.Response.EnemyEntityResponse = new EnemyEntityResponse();
            member.session.Response.EnemyEntityResponse.EnemyEntityResponses.Add(message);
            //试一试他能不能在其他地方一同发送过去 不可以 悲
            member.SendNetMessage();
        }
    }
}
