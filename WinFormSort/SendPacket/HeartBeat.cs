using System.Text;
using WinFormSort.Utility;

namespace WinFormSort.SendPacket
{
    /// <summary>
    /// 心跳包
    /// </summary>
    public class HeartBeat
    {
        /// <summary>
        /// 报文编号
        /// </summary>
        public short CycleNumber { get; set; }
        /// <summary>
        /// 上个已处理的编号
        /// </summary>
        public string Acknowlege { get; set; }

        /// <summary>
        /// 创建心跳包二进制数据
        /// </summary>
        /// <param name="type">
        /// 1:启动心跳
        /// </param>
        /// <returns></returns>
        public byte[] ToBytes(int type=0)
        {
            byte[] SendData = new byte[240];
            StringBuilder strb = new StringBuilder();
            if(type==0)
            {
                if (CommonMsgBuffer.CycleNumber >= 99)
                    CommonMsgBuffer.CycleNumber = 1;
                else
                    CommonMsgBuffer.CycleNumber += 1;
            }
            CycleNumber = CommonMsgBuffer.CycleNumber;
            strb.Append(CycleNumber.ToString("X2").PadLeft(4, '0'));
            strb.Append(30.ToString("X2").PadLeft(4, '0'));
            strb.Append(21.ToString("X2").PadLeft(4, '0'));
            strb.Append(Acknowlege);
            //strb.Append(0.ToString("X2").PadLeft(4, '0'));
            strb.Append(0.ToString("X2").PadLeft(4, '0'));
            strb.Append("FFFF");
            strb.Append(9.ToString("X2").PadLeft(4, '0'));
            strb.Append(4.ToString("X2").PadLeft(4, '0'));
            strb.Append("FFFF");
            for (int i = 19; i <= 240;i= i+2)
            {
                strb.Append("FFFF");
            }
            string str = strb.ToString();
            SendData = DataConversion.StringToHexArray(str);
            return SendData;
        }
    }
}
