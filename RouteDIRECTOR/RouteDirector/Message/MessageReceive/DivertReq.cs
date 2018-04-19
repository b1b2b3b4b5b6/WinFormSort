using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RouteDirector.Utility;
namespace RouteDirector.PacketProcess
{
	class DivertReq : MessageBase
	{
		static private Int16 msgId = (Int16)MessageType.DivertReq;
		public Int16 nodeId;
		public Int16 cartSeq;
		public Int32 attribute;
		public string codeStr;
		static public int len = 20;
		public DivertReq(byte[] buf, int offset)
		{
			offset += 2;
			offset += DataConversion.ByteToNum(buf, offset, ref nodeId, false);
			offset += DataConversion.ByteToNum(buf, offset, ref cartSeq, false);
			offset += DataConversion.ByteToNum(buf, offset, ref attribute, false);
			codeStr = Encoding.ASCII.GetString(buf, offset, 10);
		}

	}
}
