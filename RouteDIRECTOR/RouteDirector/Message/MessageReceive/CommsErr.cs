using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RouteDirector.Utility;
namespace RouteDirector.PacketProcess
{
	class CommsErr
	{
		enum Error : Int16
		{
			MsgFormatErr = 1,
			HeartBeatTimeOut = 2,
		}

		static public Int16 msgId = (Int16)MessageBase.MessageType.CommsErr;
		public Int16 error;
		static public int len = 4;

		public CommsErr(byte[] buf, int offset)
		{
			offset += 2;
			offset += DataConversion.ByteToNum(buf, offset, ref error, false);
		}

	}
}
