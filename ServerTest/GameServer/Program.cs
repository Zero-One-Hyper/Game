using Servers;
using NetServerTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoBangServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GameService server = new GameService();
            server.Init();
            server.Start();

            DebugCommand.Loop();


            server.Stop();
            Console.Read();
        }
    }
}
