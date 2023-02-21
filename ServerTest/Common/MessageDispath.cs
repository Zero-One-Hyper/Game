using NetServerTools;
using Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{


    //消息路径    作为一个工具 只与MessageDistributer连接
    internal class MessageDispath<TSender> : Singleton<MessageDispath<TSender>>
    {
        public void Dispatch(TSender sender, NetMessageRequest req)
        {
            if (req.HeartBeatReq != null) { MessageDistributer<TSender>.Instance.RaiseEvent(sender, req.HeartBeatReq); }
            if (req.LoginRequest!= null) { MessageDistributer<TSender>.Instance.RaiseEvent(sender, req.LoginRequest); }

            if (req.SummonCooperatorSignRequest != null) { MessageDistributer<TSender>.Instance.RaiseEvent(sender, req.SummonCooperatorSignRequest); }
            if (req.SummonCooperatorRequest != null) { MessageDistributer<TSender>.Instance.RaiseEvent(sender, req.SummonCooperatorRequest); }
            if (req.SummonCooperatorResponse != null) { MessageDistributer<TSender>.Instance.RaiseEvent(sender, req.SummonCooperatorResponse); }

            if (req.EntitySyncRequest!= null) { MessageDistributer<TSender>.Instance.RaiseEvent(sender, req.EntitySyncRequest); }
            
            if (req.AttackRequest!= null) { MessageDistributer<TSender>.Instance.RaiseEvent(sender, req.AttackRequest); }
            if (req.RollRequest!= null) { MessageDistributer<TSender>.Instance.RaiseEvent(sender, req.RollRequest); }
                      
            if (req.EquipArmor!= null) { MessageDistributer<TSender>.Instance.RaiseEvent(sender, req.EquipArmor); }

            if (req.UploadEnemyInfosRequest!= null) { MessageDistributer<TSender>.Instance.RaiseEvent(sender, req.UploadEnemyInfosRequest); }
            if (req.EnemyEntityRequest!= null) { MessageDistributer<TSender>.Instance.RaiseEvent(sender, req.EnemyEntityRequest); }
            if (req.EnemyAttackMsg!= null) { MessageDistributer<TSender>.Instance.RaiseEvent(sender, req.EnemyAttackMsg); }
        
        }

        public void Dispatch(TSender sender, NetMessageResponse res)
        {
            if(res.HeartBeatRes != null) { MessageDistributer<TSender>.Instance.RaiseEvent(sender, res.HeartBeatRes); }
            if(res.LoginResponse != null) { MessageDistributer<TSender>.Instance.RaiseEvent(sender, res.LoginResponse); }
            

            if(res.SummonCooperatorSignRequest != null) { MessageDistributer<TSender>.Instance.RaiseEvent(sender, res.SummonCooperatorSignRequest); }
            if(res.SummonCooperatorRequest != null) { MessageDistributer<TSender>.Instance.RaiseEvent(sender, res.SummonCooperatorRequest); }
            if(res.SummonCooperatorResponse != null) { MessageDistributer<TSender>.Instance.RaiseEvent(sender, res.SummonCooperatorResponse); }
            
            if(res.EntitySyncResponse != null) { MessageDistributer<TSender>.Instance.RaiseEvent(sender, res.EntitySyncResponse); }
            
            if(res.AttactResponse!= null) { MessageDistributer<TSender>.Instance.RaiseEvent(sender, res.AttactResponse); }
            if(res.Rollresponse!= null) { MessageDistributer<TSender>.Instance.RaiseEvent(sender, res.Rollresponse); }

            if(res.EnemyEntityResponse!= null) { MessageDistributer<TSender>.Instance.RaiseEvent(sender, res.EnemyEntityResponse); }
            if(res.EnemyAttackMsg!= null) { MessageDistributer<TSender>.Instance.RaiseEvent(sender, res.EnemyAttackMsg); }
        }
    }
}
