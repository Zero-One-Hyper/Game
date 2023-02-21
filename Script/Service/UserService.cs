using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using Protocol;
using System;

namespace Server
{
    public class UserService : Singleton<UserService>, IDisposable
    {
        public UserService()
        {
            MessageDistributer.Instance.Subscribe<HeartBeatResponse>(this.OnHeartBeatRes);
            MessageDistributer.Instance.Subscribe<LoginResponse>(this.OnLoggin);
        }


        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<HeartBeatResponse>(this.OnHeartBeatRes);
            MessageDistributer.Instance.Unsubscribe<LoginResponse>(this.OnLoggin);
        }
        public void Init()
        {
            Debug.Log("Init UserService");
            this.SendLogin();
        }
        public void SendHeartBeat()
        {
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.HeartBeatReq = new HeartBeatRequest();
            message.Request.HeartBeatReq.Result = RESULT.Success;
            NetClient.Instance.SendMessage(message);
        }
        private void OnHeartBeatRes(object sender, HeartBeatResponse message)
        {
            Debug.Log("OnHeartBeat");
        }

        public void SendLogin()
        {
            Debug.Log("SendPlayerLogin");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.LoginRequest = new LoginRequest();
            
            NetClient.Instance.SendMessage(message);
        }

        private void OnLoggin(object sender, LoginResponse message)
        {
            if(message.Result == RESULT.Failed)
            {
                Debug.LogError("Login Error");
                return;
            }
            Debug.LogFormat("OnLogin ID:[{0}]", message.PlayerId);
            User.Instance.Init(message.PlayerId);
        }
    }
}
