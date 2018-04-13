using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormSort.Utility;

namespace WinFormSort.SendPacket
{
    /// <summary>
    /// 主机下发分拣指令
    /// </summary>
   public class DivertCmd
    {
        public byte[] ToBytes(int nodeid,int seqid,int lineid)
        {
            byte[] SendData = new byte[240];
            StringBuilder strb = new StringBuilder();
            strb.Append(1.ToString("X2").PadLeft(4, '0'));
            strb.Append(30.ToString("X2").PadLeft(4, '0'));
            strb.Append(21.ToString("X2").PadLeft(4, '0'));
            strb.Append(0.ToString("X2").PadLeft(4, '0'));
            strb.Append(0.ToString("X2").PadLeft(4, '0'));
            strb.Append("FFFF");
            strb.Append(2.ToString("X2").PadLeft(4, '0'));
            strb.Append(nodeid.ToString("X2").PadLeft(4, '0'));
            strb.Append(seqid.ToString("X2").PadLeft(4, '0'));
            strb.Append(0.ToString("X2").PadLeft(4, '0'));
            strb.Append(lineid.ToString("X2").PadLeft(4, '0'));
            strb.Append("FFFF");
            for (int i = 25; i <= 240; i = i + 2)
            {
                strb.Append("FFFF");
            }
            string str = strb.ToString();
            LogHelper.WriteLog4(str);
            SendData = DataConversion.StringToHexArray(str);
            return SendData;
        }
    }
}
