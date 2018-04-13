using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace WinFormSort
{
   public  interface IChannel
    {
        /// <summary>
        /// 连接服务端
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        void ConnectServer(AGVConfig agv);
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="data"></param>
          void AsynSendData(byte[] data);
        /// <summary>
        /// 接收消息
        /// </summary>
         void ReceiveData();
        /// <summary>
        /// 关闭连接
        /// </summary>
         void Close();
    }
}
