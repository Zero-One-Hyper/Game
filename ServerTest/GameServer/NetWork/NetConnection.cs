using Managers;
using Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Common;
using Model;

namespace NetWork
{
    public class MessageData
    {
        public IPEndPoint RemoteEndPoint { get; set; }
        public byte[] Data { get; set; }
        public int OffetSet { get; set; }
        public int Length { get; set; }
    }
    public class NetConnection
    {
        private Socket Socket;
        private SocketAsyncEventArgs reciveEventArg;
        
        public IPAddress IP
        {
            get
            {
                if(this.Socket != null)
                    return ((IPEndPoint)Socket.RemoteEndPoint).Address;
                return null;                    
            }
        }
        public int Id { get; set; }

        public PackageHandler<NetConnection> packgeHandler;
        public NetSession session;
        public Entity entity;


        public NetConnection(Socket socket) 
        { 
            this.Socket = socket;
            session = new NetSession();
            this.packgeHandler = new PackageHandler<NetConnection>(this);
        }

        public void Connect()
        {
            reciveEventArg = new SocketAsyncEventArgs();
            reciveEventArg.AcceptSocket = this.Socket;
            reciveEventArg.Completed += OnAsyncReciveComplet;
            reciveEventArg.SetBuffer(new byte[64 * 1024], 0, 64 * 1024);

            TryRecive();
        }

        private void SendData(byte[] data, int offset, int count)
        {
            lock(this)
            {
                if(Socket.Connected)
                {
                    Socket.BeginSend(data, offset, count, SocketFlags.None, null, null);
                }
            }
        }

        public void SendNetMessage()
        {
            byte[] data = session.GetNetMessage();
            
            this.SendData(data, 0, data.Length);
        }

        private void OnAsyncReciveComplet(object sender, SocketAsyncEventArgs arg)
        {
            if(arg.BytesTransferred == 0)
            {
                Disconnect();
                return;
            }
            if(arg.SocketError != SocketError.Success)
            {
                Disconnect();
                return;
            }
            byte[] data = new byte[arg.BytesTransferred];
            Array.Copy(arg.Buffer, arg.Offset, data, 0, arg.BytesTransferred);
            MessageData dataEventArg = new MessageData()
            { 
                RemoteEndPoint = arg.AcceptSocket.RemoteEndPoint as IPEndPoint,
                Data = data,
                OffetSet = arg.Offset,
                Length = arg.BytesTransferred                
            };
            OnDataRecive(dataEventArg);

            TryRecive();
        }

        private void OnDataRecive(MessageData dataEventArg)
        {
            //Console.WriteLine(string.Format("Client[{0}] DataReciveed Len:[{1}]", dataEventArg.RemoteEndPoint.ToString(), dataEventArg.Length));

            this.packgeHandler.ReceiveData(dataEventArg.Data, dataEventArg.OffetSet, dataEventArg.Length);
        }

        private void TryRecive()
        {
            lock (this)
            {
                if (Socket.Connected)
                    Socket.ReceiveAsync(reciveEventArg);
            }
        }



        public void Disconnect()
        {
            Console.WriteLine(string.Format("NetConnection [{0}] is DisConnect", IP.ToString()));
            try
            {
            Socket.Shutdown(SocketShutdown.Both);
            }
            catch { }
            Socket.Close();
            Socket = null;
            reciveEventArg.Completed -= OnAsyncReciveComplet;
            
            reciveEventArg.Dispose();

            //Socket.Dispose();
            
        }
    }
}
