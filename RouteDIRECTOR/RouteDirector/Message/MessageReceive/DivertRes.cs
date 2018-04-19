using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RouteDirector.Utility;
namespace RouteDirector.PacketProcess
{
	class DivertRes
	{
		static public Int16 msgId = (Int16)MessageBase.MessageType.DivertRes;
		public Int16 nodeId;
		public Int16 cartSeq;
		public Int16 laneId;
		public Int16 divertRes;
		public string codeStr;
		static public int len = 20;
		public DivertRes(byte[] buf, int offset)
		{
			offset += 2;
			offset += DataConversion.ByteToNum(buf, offset, ref nodeId, false);
			offset += DataConversion.ByteToNum(buf, offset, ref cartSeq, false);
			offset += DataConversion.ByteToNum(buf, offset, ref laneId, false);
			offset += DataConversion.ByteToNum(buf, offset, ref divertRes, false);
			codeStr = Encoding.ASCII.GetString(buf, offset, 10);
		}

	}
}
