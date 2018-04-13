using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormSort.Utility;

namespace WinFormSort.RecivePacket
{
    /// <summary>
    /// PLC的分拣结果
    /// </summary>
    public class DivertRsp
    {
        public string startIcon { get; set; }
        public short Msg_ID { get; set; }
        public short Node_ID { get; set; }
        public short Cart_Seq { get; set; }
        public short Lane_ID { get; set; }
        public short Div_Result { get; set; }
        public string Code_Str { get; set; }
        public DivertRsp LoadFrom(byte[] data,int i)
        {

            startIcon = DataConversion.byteToHexStr(data, i, 2);
            Msg_ID = Read(data, i + 2, 2);
            Node_ID = Read(data, i + 4, 2);
            Cart_Seq = Read(data, i + 6, 2);
            Lane_ID = Read(data, i + 8, 2);
            Div_Result =Read(data, i+10, 2);
            Code_Str = Encoding.ASCII.GetString(DataConversion.CutByteArray(data,i+12,4));
            return this;
        }

        public Int16 Read(byte[] sorcedata, int index, int len)
        {
            Int16 result;
            byte[] bytearray = new byte[len];
            Array.Copy(sorcedata, index, bytearray, 0, len);
            result= (Int16)(bytearray[0] * 256 + bytearray[1]);
            return result;
        }
    }
}
