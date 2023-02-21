using Model;
using NetServerTools;
using Protocol;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managers
{
    internal class EquipManager : Singleton<EquipManager>
    {
        public  EquipManager() 
        {

        }
        public void Init()
        {

        }

        public void EquipArmor(Entity target, EquipArmor data)
        {
            target.Armors[(int)data.ArmorType] = data.EquipID;
        }
    }
}
