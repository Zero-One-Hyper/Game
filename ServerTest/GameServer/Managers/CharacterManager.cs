using NetWork;
using NetServerTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using System.Numerics;

namespace Managers
{
    //存储NetSession 具体数据防砸Entity
    internal class CharacterManager : Singleton<CharacterManager>
    {
        public Dictionary<int, NetConnection> Characters = new Dictionary<int, NetConnection>();
        public List<NetConnection> CharacterList = new List<NetConnection>(); 

        public CharacterManager() 
        {
            
        }
        public void Init()
        {

        }
        public void AddCharacter(NetConnection character)
        {
            character.Id = Characters.Count + 1;
            if(!Characters.ContainsKey(character.Id))
            {
                Characters.Add(character.Id, character);
            }
            CharacterList.Add(character);
            character.entity = new Entity(character.Id, character);
            EntityManager.Instance.AddEntity(character.entity);
        }
        public void AddCharacter(int id, NetConnection character)
        {
            if (Characters.ContainsKey(id))
            {
                CharacterList.Remove(character);
                Characters.Remove(character.Id);
            }
            Characters.Add(id, character);
            character.entity = new Entity(character.Id, character);
            EntityManager.Instance.AddEntity(character.entity);
        }

        public void RemoveCharacter(NetConnection character)
        {
            if(Characters.ContainsKey(character.Id))
            {
                Characters.Remove(character.Id);
                CharacterList.Remove(character);
            }
            EntityManager.Instance.RemoveEntity(character.Id);
        }

        public NetConnection GetCharacter(int characterID)
        {
            if(!Characters.TryGetValue(characterID, out NetConnection player))
            {
                Console.WriteLine("未找到此玩家");
            }
            return player;
        }
    }
}
