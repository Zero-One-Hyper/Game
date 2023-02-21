using Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    //给客户端使用 因为客户端没有Netconnction 和 Netsession
    public class PackageHandler : PackageHandler<object>
    {
        public PackageHandler(object sender) : base(sender)
        {

        }
    }
    public class PackageHandler<TSender>
    {
        public PackageHandler(TSender owner)
        {
            this.Owner = owner;
        }
        private MemoryStream stream = new MemoryStream(64 * 1024);
        private int readOffset = 0;
        int count = 0;
        byte[] Data;
        TSender Owner;
        /// <summary>
        /// 接受消息
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="offset">偏移量</param>
        /// <param name="count">数据长度</param>
        public void ReceiveData(byte[] data, int offset, int count)
        { 
            //保证接收到的信息不会超出stream的容量
            if(stream.Position + count > stream.Capacity)
            {
                Console.WriteLine("PackageHandler stream overflow!!!");
            }
            
            stream.Write(data, offset, count);
            
            this.Data = data;
            this.count= count;
            ParsePackage();
        }
        //解析数据
        //数据自定义 前两位是Header 固定为0xA1， 0x1A ，之后2位表示长度 最后是数据
        bool ParsePackage()
        {
            if (stream.GetBuffer()[0] == 0xA1 && stream.GetBuffer()[1] == 0x1A)
            {
                //说明接受到的是包头

                //去除4位header和长度
                if (readOffset + 6 < stream.Position)
                {

                    int packageSize = BitConverter.ToInt32(stream.GetBuffer(), readOffset + 2);//偏移量
                    if (packageSize + readOffset + 6 <= stream.Position)
                    {//包有效

                        NetMessage message = ProtoBuffPacker.UnPack(stream.GetBuffer(), this.readOffset + 6, packageSize);
                        if (message == null)
                        {
                            throw new Exception("PackageHandler ParsePackage faild,invalid package");
                        }
                        MessageDistributer<TSender>.Instance.ReceiveMessage(this.Owner, message);
                        this.readOffset += (packageSize + 6);
                        return ParsePackage();
                    }
                }
            }

            //未接收完/要结束了
            if (this.readOffset > 0)
            {
                long size = stream.Position - this.readOffset;
                if (this.readOffset < stream.Position)
                {
                    Array.Copy(stream.GetBuffer(), this.readOffset, stream.GetBuffer(), 0, stream.Position - this.readOffset);
                }
                //Reset Stream
                this.readOffset = 0;
                stream.Position = size;
                stream.SetLength(size);
            }
            return true;
        }
    }
}
