using NetServerTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Protocol;

namespace Managers
{
    internal class EntityManager : Singleton<EntityManager>
    {
        public List<Entity> AllEntity = new List<Entity>();
        public Dictionary<int, Entity> Entities = new Dictionary<int, Entity>();

        public List<Entity> AllEnemyEntity = new List<Entity>();
        public Dictionary<int, Entity> EnemyEntities = new Dictionary<int, Entity>();

        public EntityManager() 
        { 
        
        }
        public void Init()
        {

        }
        public void AddEntity(Entity entity)
        {
            if(!Entities.ContainsKey(entity.Id))
            {
                Entities.Add(entity.Id, entity);
            }
            if(!AllEntity.Contains(entity)) 
            { 
                AllEntity.Add(entity);
            }
        }
        public void RemoveEntity(int id)
        {
            if(Entities.ContainsKey(id))
            {
                AllEntity.Remove(Entities[id]);
                Entities.Remove(id);
            }
        }

        public Entity GetEntity(int id)
        {
            if(Entities.TryGetValue(id, out Entity entity)) 
                return entity;
            return null;
        }

        internal void SetEntitySyncData(EntitySyncMessage entitySyncMessage)
        {
            if(Entities.TryGetValue(entitySyncMessage.CharacterID, out Entity entity))
            {
                //X速度和Y速度 暂时不填
                entity.SetData(entitySyncMessage.Position, entitySyncMessage.Direction);
            }
        }

        public void AddEnemyEntity(Entity enemy)
        {
            if (!this.EnemyEntities.ContainsKey(enemy.Id))
            {
                Entities.Add(enemy.Id, enemy);
            }
            if (!AllEnemyEntity.Contains(enemy))
            {
                AllEntity.Add(enemy);
            }
        }
        public void RemoveEnemyEntity(int id)
        {
            if(this.EnemyEntities.ContainsKey(id))
            {
                Entities.Remove(id);
                this.EnemyEntities.Remove(id);
            }
        }
        public Entity GetEnemyEntity(int id)
        {
            if(this.EnemyEntities.ContainsKey(id))
                return this.EnemyEntities[id];
            return null;
        }

        internal void SetEnemyEntityData(EnemyEntityRequest message)
        {
            if (EnemyEntities.TryGetValue(message.EnemyId, out Entity entity))
            {
                //X速度和Y速度 暂时不填
                entity.SetData(message.NPosition, message.NDirection);            
            }
        }
    }
}
