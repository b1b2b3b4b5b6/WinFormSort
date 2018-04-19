using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RouteDirector.Utility;
namespace RouteDirector.PacketProcess
{
	class HeartBeat
	{
		public byte[] message;

		static public Int16 msgId = (Int16)MessageBase.MessageType.HeartBeat;
		public Int16 period;

		static public int len = 4;

		public HeartBeat(byte[] buf, int offset)
		{
			offset += 2;
			Pack();
			offset += DataConversion.ByteToNum(message, offset, ref period, false);
		}

		public HeartBeat(Int16 mPeriod)
		{
			period = mPeriod;
			Pack();
		}

		public void Pack()
		{
			message = DataConversion.NumToByte(msgId, false);
			message = DataConversion.NumToByte(period, message, false);
		}
	}
}
