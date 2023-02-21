using System;
using System.Collections;
using System.Collections.Generic;
using Protocol;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class EntityManager : Singleton<EntityManager>
{
    public Dictionary<int, Entity> Entities = new Dictionary<int, Entity>();    

    public void AddEntity(Entity entity)
    {
        this.Entities[entity.EntityID] = entity;
    }

    public void RemoveEntity(Entity entity)
    {
        if (this.Entities.ContainsKey(entity.EntityID))
        {
            this.Entities.Remove(entity.EntityID);
        }
    }

    public void OnEntitySync(EntitySyncMessage SyncMsg)
    {
        if (Entities.TryGetValue(SyncMsg.CharacterID, out Entity entity))
        {
            //先处理位置
            entity.NPosition = SyncMsg.Position;
            entity.NDirection = SyncMsg.Direction;

            //再处理动画
            if(entity.AnimEventArg == null) entity.AnimEventArg = new PlayerAnimEventArg();
            ((PlayerAnimEventArg)entity.AnimEventArg).isLocked = SyncMsg.IsLock;
            ((PlayerAnimEventArg)entity.AnimEventArg).speedX = SyncMsg.XSpeed / 100.0f;
            ((PlayerAnimEventArg)entity.AnimEventArg).speedY = SyncMsg.YSpeed / 100.0f;
            ((PlayerAnimEventArg)entity.AnimEventArg).isUseShield = SyncMsg.IsUseShield;
            ((PlayerAnimEventArg)entity.AnimEventArg).isRunning = SyncMsg.IsRunning;

        }
    }
}
