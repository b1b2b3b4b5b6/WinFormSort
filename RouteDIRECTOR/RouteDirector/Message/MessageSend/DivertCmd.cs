using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RouteDirector.Utility;
namespace RouteDirector.PacketProcess
{
	class DivertCmd
	{
		public byte[] message;
		static public Int16 msgId = (Int16)MessageBase.MessageType.NodeAva;
		public Int16 nodeId = 0;
		public Int16 cartSeq = 0;
		public Int16 priority = 0;
		public Int16 laneId = 0;
		static public int len = 10;
		public DivertCmd(byte[] buf, int offset)
		{
			offset += 2;
			Pack();
			offset += DataConversion.ByteToNum(message, offset, ref nodeId, false);
			offset += DataConversion.ByteToNum(message, offset, ref cartSeq, false);
			offset += DataConversion.ByteToNum(message, offset, ref priority, false);
			offset += DataConversion.ByteToNum(message, offset, ref laneId, false);
		}

		public DivertCmd(Int16 tNodeId, Int16 tCartSeq, Int16 tPriority, Int16 tLaneId)
		{
			nodeId = tNodeId;
			cartSeq = tCartSeq;
			priority = tPriority;
			laneId = tLaneId;
			Pack();
		}

		public void Pack()
		{
			message = DataConversion.NumToByte(msgId, false);
			message = DataConversion.NumToByte(nodeId,message, false);
			message = DataConversion.NumToByte(cartSeq, message, false);
			message = DataConversion.NumToByte(priority, message, false);
			message = DataConversion.NumToByte(laneId, message, false);
		}
	}
}
