using NetWork;
using NetServerTools;
using Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Servers
{
    public class HeartBeatSerivce : Singleton<HeartBeatSerivce>, IDisposable
    {
        public HeartBeatSerivce() 
        {
            MessageDistributer<NetConnection>.Instance.Subscribe<HeartBeatRequest>(this.OnHeartBeatRequest);
        }
        public void Dispose()
        {
            MessageDistributer<NetConnection>.Instance.Unsubscribe<HeartBeatRequest>(this.OnHeartBeatRequest);
        }
        public void Init()
        {
            Console.WriteLine("HeartBeatSerivce is Active!!!");
        }

        private void OnHeartBeatRequest(NetConnection sender, HeartBeatRequest request)
        {
            Console.WriteLine(string.Format("收到心跳包 来源：[{0}]", sender.Id));
            sender.session.Response.HeartBeatRes = new HeartBeatResponse();
            sender.session.Response.HeartBeatRes.Result = RESULT.Success;
            sender.SendNetMessage();
        }
    }
}
