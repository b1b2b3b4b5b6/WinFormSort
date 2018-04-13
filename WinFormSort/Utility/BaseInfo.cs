using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormSort.Utility
{
    public static class BaseInfo
    {
        /// <summary>
        /// 通讯状态类型
        /// </summary>
        public  enum NetOptResType
        {
            Nil = 0,    //未定义
            ConSucc = 1,    //连接成功
            ConFail = 2,    //连接失败(超时)
            DisConn = 3,    //连接断开
            OnError = 4,    //Socket错误

            MsgRead=10,
            OnError_MsgLength = 11,    //消息长度错误
            OnError_MsgParse = 12,    //消息解析失败
            OnError_MsgRead = 13,    //读取接收失败
            OnError_MsgReadNil = 14,    //读取消息失败(检验码不匹配)

            SendMsg=20,
            OnError_SendMsg=21,
        }

        public static int sequenceID = 0;

        public static byte[] msgBuffer;

        public static NetOptResType ConnectStatus = NetOptResType.Nil;

        public static int MsgStatus= Convert.ToInt32(NetOptResType.Nil);
        public static int DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            int dateDiff = 0;
            TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
            TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            dateDiff = ts.Seconds;
            return dateDiff;
        }

   }
}
