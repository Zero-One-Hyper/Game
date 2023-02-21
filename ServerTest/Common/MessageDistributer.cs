using NetServerTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Protocol;
using System.Runtime.CompilerServices;
using System.Collections.Concurrent;
using System.Threading;

namespace Common
{
   
    public class MessageDistributer : MessageDistributer<object>
    {

    }

    //分发器 分发给Server
    public class MessageDistributer<TSender> : Singleton<MessageDistributer<TSender>>
    {
        public MessageDistributer()
        {

        }
        public class MessageArgs
        {
            public TSender sender;
            public NetMessage message;
        }

        bool isRunning = false;
        //把所有回调方法存起来
        private Dictionary<string, Delegate> CallBacks = new Dictionary<string, Delegate>();
        public delegate void ServerCallBack<TMessage>(TSender sender, TMessage message); 
        
        //一个队列存储所有接收到的消息
        private Queue<MessageArgs> AllNetMessages = new Queue<MessageArgs>();
        
        Thread messageDistributThread;
        private AutoResetEvent autoResetEvent = new AutoResetEvent(true);//第一次尝试继续不会阻塞
        public void Start()
        {
            this.AllNetMessages.Clear();
            this.isRunning = true;

            messageDistributThread = new Thread(ThreadDistribute)
            {
                IsBackground = true,
            };
            messageDistributThread.Start();
        }
        //给客户端调用
        public void ClearQueue()
        {
            AllNetMessages.Clear();
        }

        public void Stop()
        {
            isRunning = false;
            messageDistributThread.Join();
            autoResetEvent.Set();
        }

        //注册方法
        public void Subscribe<TMessage>(ServerCallBack<TMessage> callBack)
        {
            string type = typeof(TMessage).Name;
            if(type == "None") 
            {
                return;
            }
            if(!this.CallBacks.ContainsKey(type))
            {
                CallBacks.Add(type, callBack);//存入字典
            }
        }

        public void Unsubscribe<TMessage>(ServerCallBack<TMessage> callBack) 
        {
            string type = typeof(TMessage).Name;
            if (type == "None" || callBack == null)
            {
                return;
            }
            if(this.CallBacks.ContainsKey(type)) 
            {
                CallBacks.Remove(type);
            }
        }

        //将所有数据存入队列
        public void ReceiveMessage(TSender sender, NetMessage message)
        {
            //if (!isRunning) return;
            List<MessageArgs> res = new List<MessageArgs>();
            MessageArgs _message = new MessageArgs()
            {
                sender = sender,
                message = message,
            };

            this.AllNetMessages.Enqueue(_message);

            res.Add(_message);
            autoResetEvent.Set();            
        }

        //将方法调用
        public void RaiseEvent<TMessage>(TSender sender, TMessage message)
        {            
            string type = message.GetType().Name;
            if(CallBacks.TryGetValue(type, out Delegate callback))
            {
                if(callback != null)
                {
                    ServerCallBack<TMessage> reiseData = callback as ServerCallBack<TMessage>;
                    reiseData.Invoke(sender, message);
                }
            }
            else
            {
                Console.WriteLine(string.Format("No Event Called [{0}]", type));
            }
        }


        //一次性分发所有消息 给客户端用
        public List<MessageArgs> Distribute()
        {
            List<MessageArgs> res = new List<MessageArgs>();
            while (this.AllNetMessages.Count > 0)
            {
                MessageArgs packages = this.AllNetMessages.Dequeue();

                if (packages.message.Request != null)
                {
                    MessageDispath<TSender>.Instance.Dispatch(packages.sender, packages.message.Request);
                }
                if (packages.message.Response != null)
                {
                    MessageDispath<TSender>.Instance.Dispatch(packages.sender, packages.message.Response);
                }
                res.Add(packages);
            }
            return res;
        }

        //扔到线程中给线程用
        private void ThreadDistribute()
        {
            //Console.WriteLine("StartThread");
            try
            {

                while (isRunning)
                {
                    if (this.AllNetMessages.Count == 0)
                    {
                        autoResetEvent.WaitOne();
                        continue;
                    }
                    MessageArgs packages = this.AllNetMessages.Dequeue();
                    if(packages == null)
                    {
                        autoResetEvent.WaitOne();
                        continue;
                    }
                    if (packages.message.Request != null)
                    {
                        MessageDispath<TSender>.Instance.Dispatch(packages.sender, packages.message.Request);
                    }
                    if (packages.message.Response != null)
                    {
                        MessageDispath<TSender>.Instance.Dispatch(packages.sender, packages.message.Response);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("MesssageDistrubuter Error: " + ex.ToString());
            }
            finally
            {
                Console.WriteLine("MessageDistributer thread End!!");
            }
            Console.WriteLine("ThreadEnd");
        }


    }
}
