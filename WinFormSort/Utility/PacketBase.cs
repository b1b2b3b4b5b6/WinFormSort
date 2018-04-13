using System;
using System.Text;
namespace WinFormSort.Utility
{
    public class PacketBase
    {
       
        //头关键字
        protected string HeaderKey ="87CD";
        //头长度
        protected string SizeOfHeader = "0008";
        //数据长度
        protected string SizeOfMsg { get; set; }
        //功能码
        protected string FunctionCode = "0001";
        //消息类型
        protected string MessageType { get; set; }
        //参数长度
        protected string NumOfPara { get; set; }

        protected PacketBase()
        {

        }
        protected int writeIndex = 0;
        protected int ReadIndex = 0;
        protected byte[] _byteBuffer;

        public virtual byte[] ToBytes()
        {
            throw new NotImplementedException();
        }

        public virtual int LoadFrom(byte[] buffer)
        {
            
            throw new NotImplementedException();
        }
      
    }
}
