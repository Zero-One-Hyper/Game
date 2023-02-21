using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    public class ServerLoader
    {
        public ServerLoader(NetClient netClient)
        {
            this.client = netClient;
        }

        private NetClient client;

        public void InitServer()
        {
            UserService.Instance.Init();
            MapService.Instance.Init();
            BattleService.Instance.Init();
            EquipService.Instance.Init();
            EnemyService.Instance.Init();
        }

        public void DisposeServer()
        {
            UserService.Instance.Dispose();
            MapService.Instance.Dispose();
        }
    }
}