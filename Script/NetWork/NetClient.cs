using System.IO;
using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;
using Common;
using Server;

public class NetClient : MonoSingleton<NetClient>
{
    IPEndPoint Address;
    Socket clientSocket;
    
    [SerializeField]
    private bool isConnected;
    public bool IsConnected
    {
        get
        {
            return isConnected;
        }
    }
    private PackageHandler packageHandler;

    private Queue<NetMessage> SendMessagesQueue = new Queue<NetMessage>();
    private MemoryStream sendBuffer = new MemoryStream();
    private int sendOffset = 0;

    private MemoryStream receiveBuffer = new MemoryStream(64 * 1024);
    private int receiveOffset = 0;

    private ServerLoader serverLoader;

    protected override void OnStart()
    {
        this.packageHandler = new PackageHandler(null);
        serverLoader = new ServerLoader(this);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnDestroy()
    {
        this.Disconnected();
    }
    public void Init(string serverIP, int port)
    {
        this.Address = new IPEndPoint(IPAddress.Parse(serverIP), port);
        //clientSocket = new Socket(EndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
    }

    public void DoConnect(Action<bool, string> connectCallBack)
    {
        if (isConnected) return;
        if (clientSocket != null)
        {
            clientSocket.Close();
            clientSocket = null;
        }

        StartCoroutine(StartConnect(connectCallBack));
    }
    IEnumerator StartConnect(Action<bool, string> action)
    {
        IAsyncResult async = null;
        try
        {
            this.clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.clientSocket.Blocking = true;

            Debug.LogFormat("Connect to server[{0}]", this.Address);
            async = this.clientSocket.BeginConnect(this.Address, OnClientConnect, null);
        }
        catch (SocketException sex)
        {
            this.Disconnected();
            Debug.LogErrorFormat("DoConnect SocketException:[{0},{1},{2}]{3} ", sex.ErrorCode, sex.SocketErrorCode, sex.NativeErrorCode, sex.ToString());
        }
        catch (Exception ex)
        {
            Debug.Log("DoConnect Exception:" + ex.ToString() + "\n");
        }

        while (async.IsCompleted)
        {
            Debug.Log("Connecting......");
            yield return null;
        }
        try
        {
            clientSocket.EndConnect(async);
        }
        catch(SocketException ex)
        {
            action?.Invoke(false, ex.ToString());
            Debug.Log(ex.ToString());
            yield break;
        }

        Debug.Log("Connected Success!!!");

        if (clientSocket.Connected)
        {
            clientSocket.Blocking = false;
        }

        this.isConnected = true;
        MessageDistributer.Instance.ClearQueue();

        action?.Invoke(true, "Logging Success!!!");
        
        serverLoader.InitServer();
    }

    private void OnClientConnect(IAsyncResult ar)
    {
        Debug.Log("Connected to Server");
    }

    public void Disconnected()
    {
        if (!isConnected) return;

        this.isConnected = false;
        if (this.clientSocket != null)
        {
            this.clientSocket.Close();
        }
        clientSocket = null;
        this.SendMessagesQueue.Clear();
        this.sendBuffer.Position = 0;
        this.sendOffset = 0;

        serverLoader.DisposeServer();

    }
    // Update is called once per frame
    void Update()
    {
        if (!isConnected) return;

        if (clientSocket.Poll(0, SelectMode.SelectError))
        {
            Debug.LogError("Client Socket SelectError");
            Disconnected();
            return;
        }
        ProcessReceive();
        ProcessSend();

    }
    //SendMessage只将消息存入队列中
    public void SendMessage(NetMessage message)
    {
        if (!isConnected) return;

        SendMessagesQueue.Enqueue(message);

    }
    //处理发送
    private void ProcessSend()
    {
        if (!isConnected) return;

        try
        {
            if (!clientSocket.Poll(0, SelectMode.SelectWrite))
            {
                //Debug.LogError("Client Socket SendError");
                //Disconnected();
                //现在不可以发消息
                return;
            }
            if (this.sendBuffer.Position > sendOffset)
            {
                long size = this.sendBuffer.Position - sendOffset;
                int sendCount = clientSocket.Send(this.sendBuffer.GetBuffer(), sendOffset, (int)size, SocketFlags.None);
                if (sendCount <= 0)
                {
                    this.Disconnected();
                    return;
                }
                this.sendOffset += sendCount;
                if (this.sendOffset >= this.sendBuffer.Position)
                {
                    this.sendOffset = 0;
                    //流的当前位置是下一个读取或写入操作可以执行的位置
                    this.sendBuffer.Position = 0;
                    SendMessagesQueue.Dequeue();
                }
            }
            else
            {
                BufferSendAddMessage();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(enabled.ToString());
            this.Disconnected();
        }
    }
    private void BufferSendAddMessage()
    {
        if (this.SendMessagesQueue.Count > 0)
        {
            NetMessage message = SendMessagesQueue.Peek();
            byte[] package = ProtoBuffPacker.Pack(message);
            //把消息数据流写入SendBuffer中
            sendBuffer.Write(package, 0, package.Length);
        }
    }
    void ProcessReceive()
    {
        if (!isConnected) return;
        try
        {
            if (!this.clientSocket.Poll(0, SelectMode.SelectRead))
            {
                //Debug.LogError("Client Socket Receive Error");
                //Disconnected();
                //现在无消息可读
                return;
            }
            int size = clientSocket.Receive(this.receiveBuffer.GetBuffer(), this.receiveOffset, this.receiveBuffer.Capacity, SocketFlags.None);
            if (size <= 0)
            {
                Debug.LogError("Client Socket Receive Net zero byte");
                Disconnected();
                return;
            }

            this.packageHandler.ReceiveData(this.receiveBuffer.GetBuffer(), 0, size);

        }
        catch (Exception ex)
        {
            Debug.LogError(ex.ToString());
            this.Disconnected();
        }
        var tt = MessageDistributer.Instance.Distribute();
        tt.Add(null);
    }
}
