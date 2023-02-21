using Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace GoBangServer.NetWork
{
    internal class NetWorkSerialize
    {
        static byte[] MessageHead = { 0xA1, 0x1A };
        //编写序列化工具
        public static byte[] Serialize(NetMessage message)
        {
            //首先判空并从type中判断是否添加了Serialize
            if(message == null || !message.GetType().IsSerializable) 
                return null;
            //使用 BinaryFormatter 与 MemoryStream 结合实现序列化
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using(MemoryStream stream = new MemoryStream())
            {
                binaryFormatter.Serialize(stream, message);
                byte[] temp = stream.ToArray();
                List<byte> result = new List<byte>();
                result.AddRange(MessageHead);
                int length = temp.Length;
                byte[] PackLength = { (byte)((length & 0xff00) >> 8), (byte)(length & 0x00ff) };
                result.AddRange(PackLength);
                result.AddRange(temp);
                return result.ToArray();
            }
           
        }

        public static NetMessage Deserialize(byte[] data)
        {
            if(data == null || data.Length== 0) 
                return null;

            BinaryFormatter binaryFormatter= new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream(data))
            {
                object ob = binaryFormatter.Deserialize(stream);
                return ob as NetMessage;
            }
        }
    }
}
