using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RouteDirector.Utility;
namespace RouteDirector.PacketProcess
{
	class NodeAva
	{
		static public Int16 msgId = (Int16)MessageBase.MessageType.NodeAva;
		public Int16 nodeId;
		static public int len = 4;
		public NodeAva(byte[] buf, int offset)
		{
			offset += 2;
			offset += DataConversion.ByteToNum(buf, offset, ref nodeId, false);
		}

	}
}
