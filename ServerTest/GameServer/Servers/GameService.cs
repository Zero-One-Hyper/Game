using GoBangServer.Servers;
using Managers;
using NetWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestCode;

namespace Servers
{
    internal class GameService
    {
        NetService netWork;
        Thread updateThread;

        public void Init()
        {
            netWork = new NetService();
            netWork.Init();

            UserService.Instance.Init();   
            TestService.Instance.Init();
            HeartBeatSerivce.Instance.Init();
            EntityService.Instance.Init();
            BattleService.Instance.Init();
            EquipService.Instance.Init();
            RoomService.Instance.Init();

            CharacterManager.Instance.Init();
            

            updateThread = new Thread(new ThreadStart(this.Update));
        }

        public void Start()
        {
            netWork.Start();
            updateThread.Start();
        }

        public void Update()
        {

        }

        internal void Stop()
        {
            updateThread.Join();
            netWork.Stop();
        }
    }
}
