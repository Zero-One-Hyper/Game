using Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetWork
{
    public class NetSession
    {
        //大的消息
        private NetMessage netMessages;
        //服务器只发送response 客户端发送request
        public NetMessageResponse Response
        {
            get
            {
                if(netMessages == null)
                {
                    netMessages = new NetMessage();
                }
                if(netMessages.Response == null)
                {
                    netMessages.Response = new NetMessageResponse();
                }

                return netMessages.Response;
            }
        }
        public byte[] GetNetMessage()
        {
            if (netMessages != null)
            {
                byte[] data = null;
                data = ProtoBuffPacker.Pack(netMessages);
                netMessages = null;
                return data;
            }
            return null;
        }

        public NetMessageRequest Request
        {
            get
            {
                if (netMessages == null)
                {
                    netMessages = new NetMessage();
                }
                if (netMessages.Request== null)
                {
                    netMessages.Request = new NetMessageRequest();
                }

                return netMessages.Request;
            }
        }
    }
}
