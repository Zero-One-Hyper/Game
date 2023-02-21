using NetWork;
using NetServerTools;
using Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace TestCode
{
    internal class TestService : Singleton<TestService>, IDisposable
    {
        public TestService() 
        {
            MessageDistributer<NetConnection>.Instance.Subscribe<TestMessage>(OnTextMEssage);
        }

        public void Dispose()
        {
           MessageDistributer<NetConnection>.Instance.Unsubscribe<TestMessage>(OnTextMEssage);
        }

        public void Init()
        {
            Console.WriteLine("TestService is activing!!!");
        }

        private void OnTextMEssage(NetConnection sender, TestMessage message)
        {
            Console.WriteLine(message.SayHello);
        }
    }
}
