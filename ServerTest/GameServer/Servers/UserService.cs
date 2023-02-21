using NetWork;
using NetServerTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Protocol;
using Common;
using Managers;

namespace Servers
{
    internal class UserService : Singleton<UserService>, IDisposable
    {
        public UserService()
        {
            MessageDistributer<NetConnection>.Instance.Subscribe<LoginRequest>(this.OnLoginReq);
            MessageDistributer<NetConnection>.Instance.Subscribe<UserExitRequest>(this.OnUserExitReq);
        }

        public void Dispose()
        {
            MessageDistributer<NetConnection>.Instance.Unsubscribe<LoginRequest>(this.OnLoginReq);
            MessageDistributer<NetConnection>.Instance.Unsubscribe<UserExitRequest>(this.OnUserExitReq);
        }
        public void Init()
        {
            Console.WriteLine("UserService is activing!!!");
        }

        private void OnLoginReq(NetConnection sender, LoginRequest message)
        {
            Console.WriteLine(string.Format("UserLoggingin[{0}]", sender.Id));
            sender.session.Response.LoginResponse = new LoginResponse();
            sender.session.Response.LoginResponse.PlayerId = sender.Id;
            sender.session.Response.LoginResponse.Result = RESULT.Success;
            sender.session.Response.LoginResponse.Erromsg = "";
            sender.SendNetMessage();
        }

        private void OnUserExitReq(NetConnection sender, UserExitRequest message)
        {
            foreach(var cha in CharacterManager.Instance.CharacterList)
            {
                if (cha.Id == message.LeaveUserId) continue;

                cha.session.Response.UserExitResponse = new UserExitResponse();
                cha.session.Response.UserExitResponse.LeaveUserId = message.LeaveUserId;
                cha.SendNetMessage();
            }
            RoomManager.Instance.ExitRoom(sender);
            CharacterManager.Instance.RemoveCharacter(sender);


            //临时把UserExit作为断开连接的消息
            sender.Disconnect();
        }



    }
}
