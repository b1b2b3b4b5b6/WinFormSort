using RouteDirector.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteDirector.PacketProcess
{
	class MessageBase
	{
		public enum MessageType : Int16
		{
			DivertReq = 1,
			DivertCmd = 2,
			DivertRes = 3,
			HeartBeat = 9,
			NodeAva = 11,
			CommsErr = 12,
		}

		public object message;
		public byte[] messageBuf;
		public Int16 msgId;

		public MessageBase() { }
		public MessageBase(byte[] buf, ref int offset)
		{
			DataConversion.ByteToNum(buf, offset, ref msgId, false);
			switch (msgId)
			{
				case (Int16)MessageType.DivertReq:
					message = new DivertReq(buf, offset);
					offset += DivertReq.len;
					break;

				case (Int16)MessageType.DivertCmd:
					message = new DivertCmd(buf, offset);
					offset += DivertCmd.len;
					break;

				case (Int16)MessageType.DivertRes:
					message = new DivertRes(buf, offset);
					offset += DivertRes.len;
					break;

				case (Int16)MessageType.HeartBeat:
					message = new HeartBeat(buf, offset);
					offset += HeartBeat.len;
					break;

				case (Int16)MessageType.NodeAva:
					message = new NodeAva(buf, offset);
					offset += NodeAva.len;
					break;

				case (Int16)MessageType.CommsErr:
					message = new CommsErr(buf, offset);
					offset += CommsErr.len;
					break;

				default:
					throw new NotImplementedException();
					break;
			}
		}
		static public MessageBase MessageCreate(byte[] buf, ref int offset)
		{
			Int16 tMsgId = 0;
			MessageBase messageBase;
			DataConversion.ByteToNum(buf, offset, ref tMsgId, false);
			switch (tMsgId)
			{
				case (Int16)MessageType.DivertReq:
					messageBase = new DivertReq(buf, offset);
					offset += DivertReq.len;
					break;

				default:
					throw new NotImplementedException();
					break;
			}
			return null;
		}
		public MessageBase(object msg)
		{
			message = null;

			if (msg is DivertCmd)
			{
				msgId = (Int16)MessageType.DivertCmd;
				message = msg;
				messageBuf = ((DivertCmd)message).message;
			}

			if (msg is HeartBeat)
			{
				msgId = (Int16)MessageType.HeartBeat;
				message = msg;
				messageBuf = ((HeartBeat)message).message;
			}

			if (message == null)
				throw new NotImplementedException();
		}

		public virtual void Pack(byte[] tMsgBuf)
		{
			messageBuf = tMsgBuf;
		}

		public virtual void UnPack() { }
	}
}
