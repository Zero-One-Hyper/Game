using Common;
using Managers;
using Model;
using NetServerTools;
using NetWork;
using Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Servers
{
    internal class EquipService : Singleton<EquipService>, IDisposable
    {
        public EquipService()
        {
            MessageDistributer<NetConnection>.Instance.Subscribe<EquipArmor>(this.OnEquipArmor);
        }

        public void Dispose()
        {
            MessageDistributer<NetConnection>.Instance.Unsubscribe<EquipArmor>(this.OnEquipArmor);
        }
        public void Init()
        {

        }

        private void OnEquipArmor(NetConnection sender, EquipArmor message)
        {
            Console.WriteLine(string.Format("Character[{0}] Equip[{1}]!!!!", sender.Id, message.EquipID));
            EquipManager.Instance.EquipArmor(sender.entity, message); 

            Room room = RoomManager.Instance.GetRoom(sender);
            if (room == null) return;
            foreach (var member in room.RoomMembers)
            {
                if (member == sender)
                    continue;
                member.session.Request.EquipArmor = message;
                member.SendNetMessage();
            }
        }
    }
}
