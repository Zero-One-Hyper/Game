using NetWork;
using Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Entity
    {
        public Entity(int ID, NetConnection connection)
        {
            this.Id = ID;
            this.Connection = connection;
        }
        public int Id;
        public NetConnection Connection;
        public NVector3 Position;
        public NVector3 Direction;

        public NCharacterInfo NCharacterInfo
        {
            get 
            {                 
                NCharacterInfo info = new NCharacterInfo()
                {
                    CharacterID = this.Id,
                    CurrentPosition= this.Position,
                    CurrentDirection= this.Direction,
                };
                info.ArmorIDs.Add(this.Armors);
                return info;
            }
            set 
            { 
                this.Id = value.CharacterID;
                this.Position = value.CurrentPosition; 
                this.Direction = value.CurrentDirection;
                
                this.Armors = value.ArmorIDs.ToArray();
            }
        }

        public int[] Armors = new int[6];  


        public void SetData(NVector3 position, NVector3 direction)
        {
            this.Position = position;
            this.Direction = direction;
        }
    }
}
