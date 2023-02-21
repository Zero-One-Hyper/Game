using Managers;
using NetWork;
using Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;

namespace NetServerTools
{
    internal class DebugCommand
    {
        public static void Loop()
        {
            bool run = true;
            Console.Write("Waiting \n");
            while (run)
            {
                Console.Write(">");
                string line = Console.ReadLine();
                
                try
                {
                    string[] cmd = line.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    switch (cmd[0])
                    {
                        case "end":
                            run = false;
                            break;
                        case "test":
                            OnTestServer();
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.ToString());
                }
            }
        }
        static void OnTestServer()
        {
            int id = CharacterManager.Instance.CharacterList.Count;
            Console.WriteLine("发送对象Id为：" + id);
            if (CharacterManager.Instance.Characters.TryGetValue(id, out NetConnection player))
            {
                /*
                player.session.Response.HeartBeatRes = new HeartBeatResponse();
                */
                player.session.Request.HeartBeatReq = new HeartBeatRequest();
                player.SendNetMessage();
            }

        }
    }
    
}
