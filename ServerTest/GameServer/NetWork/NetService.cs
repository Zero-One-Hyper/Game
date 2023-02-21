using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Security;
using Managers;
using Common;
using Model;

namespace NetWork
{
    class NetService
    {
        public NetService()
        {
            serverSocketAsyncAccept = new SocketAsyncEventArgs();
            serverSocketAsyncAccept.AcceptSocket = null;
            serverSocketAsyncAccept.Completed += OnAsyncCompleted;
        }

        private Socket serverSocket;
        private SocketAsyncEventArgs serverSocketAsyncAccept;

        private string scoketAddr = "192.168.0.104";
        private int maxConnect = 10;

        private List<NetConnection> AllPlayers = new List<NetConnection>();

        public void Init()
        {

        }
        public void Start()
        {
            //创建一个Socket对象
            this.serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //绑定一个端口
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(scoketAddr), 8848);

            serverSocket.Bind(iPEndPoint);
            serverSocket.Listen(maxConnect); //参数表示等待连接的最大数量

            //这里可以用SocketAsyncEventArgs做 socket消息的异步接收和发送
            //学习使用异步

            ServerSocketAccept(serverSocketAsyncAccept);

            MessageDistributer<NetConnection>.Instance.Start();

        }
        private void OnAsyncCompleted(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccecpt(e);

        }

        private void ServerSocketAccept(SocketAsyncEventArgs e)
        {
            e.AcceptSocket = null;
            //AccecptAsync 如果 I/O 操作挂起，则为 true。 操作完成时，将引发 e 参数的 Completed 事件。
            //如果 I/ O 操作同步完成，则为 false。 将不会引发 e 参数的 Completed 事件，
            //并且可能在方法调用返回后立即检查作为参数传递的 e 对象以检索操作的结果。
            //如果刚进入AccecptAsync就完成I/O操作同步 不会引发OnAsyncCompleted 
            serverSocket.AcceptAsync(e);           
            
        }

        private void ProcessAccecpt(SocketAsyncEventArgs args)
        {
            if(args.SocketError == SocketError.Success)
            {
                //处理连接
                NetConnection connection = new NetConnection(args.AcceptSocket);
                connection.Connect();
                AllPlayers.Add(connection);
                Console.WriteLine("已连接");

                CharacterManager.Instance.AddCharacter(connection);
                
            }
            lock (this)
            {
                //和ScoketAccecpt 互相调用，就不需要在线程中while死循环并阻塞了
                ServerSocketAccept(args);
            }
        }

       


        public void Stop()
        {
            MessageDistributer<NetConnection>.Instance.Stop();
            serverSocketAsyncAccept.Completed -= OnAsyncCompleted;

            try
            {
                serverSocket.Close();
                serverSocket.Dispose();
                serverSocketAsyncAccept.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine("NetService Stop");

            }
            DisconnectAll();

        }

        private void DisconnectAll()
        {
            foreach (var player in AllPlayers)
            {
                player.Disconnect();
            }
        }
    }
}
