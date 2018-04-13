using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormSort.Utility;

namespace WinFormSort.RecivePacket
{
   /// <summary>
   /// PLC的分拣请求
   /// </summary>
   public class DivertReq
    {
        public short CycleNumber { get; set; }
        public short Sender { get; set; }
        public short Receiver { get; set; }
        public short Acknowlege { get; set; }
        public short TransportError { get; set; }
        public string  startIcon { get; set; }
        public short Msg_ID { get; set; }
        public short Node_ID { get; set; }
        public short Cart_Seq { get; set; }
        public int Attribute { get; set; } 
        public string Code_Str { get; set; }

        public DivertReq LoadFrom(byte [] data)
        {
            //CycleNumber = BitConverter.ToInt16(Read(data,0,2),2);
            //Sender = BitConverter.ToInt16(Read(data, 2, 2), 2);
            //Receiver = BitConverter.ToInt16(Read(data, 4, 2), 2);
            //Acknowlege = BitConverter.ToInt16(Read(data, 6, 2), 2);
            //TransportError = BitConverter.ToInt16(Read(data, 8, 2), 2);

            startIcon = DataConversion.byteToHexStr(data, 10, 2);
            Msg_ID = BitConverter.ToInt16(Read(data, 12, 2), 2);
            Node_ID = BitConverter.ToInt16(Read(data, 14, 2), 2);
            Cart_Seq = BitConverter.ToInt16(Read(data, 16, 2), 2);
            Attribute = BitConverter.ToInt32(Read(data, 18, 4), 4);
            startIcon = Encoding.ASCII.GetString(Read(data,22,4));
            return this;
        }

        public byte [] Read(byte [] sorcedata,int index,int len)
        {
            byte[] result = new byte[len];
            Array.Copy(sorcedata, index, result, 0, len);
            return result;
        }
    }
}
