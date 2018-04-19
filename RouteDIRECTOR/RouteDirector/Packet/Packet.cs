using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using RouteDirector.Utility;
namespace RouteDirector.PacketProcess
{
	 class Packet
	{
		public enum Identification : Int16
		{
			PLC1 = 21,
			PLC2 = 22,
			PLC9 = 29,
			RouteDirector = 30,
			InductManager = 31,
		}
		static int packetMaxLength = 240;

		public Int16 cycleNum;
		public Int16 senderId;
		public Int16 receiverId;
		public Int16 ack;
		public Int16 transportError;


		List<MessageBase> messageList = new List<MessageBase>();
		public byte[] packetBuf;

		public Packet(byte[] buf)
		{
			int offset = 0;
			if (buf.Length > packetMaxLength)
				throw new NotImplementedException();
			packetBuf = (byte[])buf.Clone();
			offset += DataConversion.ByteToNum(buf, offset, ref cycleNum, false);
			offset += DataConversion.ByteToNum(buf, offset, ref senderId, false);
			offset += DataConversion.ByteToNum(buf, offset, ref receiverId, false);
			offset += DataConversion.ByteToNum(buf, offset, ref ack, false);
			offset += DataConversion.ByteToNum(buf, offset, ref transportError, false);
			messageList = UnpackMessage(buf, offset);
		}

		public Packet(Int16 tCycleNum, Int16 tAck, List<MessageBase> tMessageList)
		{
			senderId = (Int16)Identification.RouteDirector;
			receiverId = (Int16)Identification.PLC1;
			cycleNum = tCycleNum;
			ack = tAck;
			transportError = 0;
			messageList = tMessageList;
			pack();
		}

		public Packet(Int16 tCycleNum, Int16 tAck, MessageBase MessageBase)
		{
			senderId = (Int16)Identification.RouteDirector;
			receiverId = (Int16)Identification.PLC1;
			cycleNum = tCycleNum;
			ack = tAck;
			transportError = 0;
			messageList.Add(MessageBase);
			pack();
		}

		private void pack()
		{
			packetBuf = DataConversion.NumToByte(cycleNum, false);
			packetBuf = DataConversion.NumToByte(senderId, packetBuf, false);
			packetBuf = DataConversion.NumToByte(receiverId, packetBuf, false);
			packetBuf = DataConversion.NumToByte(ack, packetBuf, false);
			packetBuf = DataConversion.NumToByte(transportError, packetBuf, false);
			packetBuf = packetBuf.Concat(PackMessage(messageList)).ToArray();
			int len = 240 - packetBuf.Length;
			byte[] padding = Enumerable.Repeat((byte)0xff, len).ToArray();
			packetBuf = packetBuf.Concat(padding).ToArray();

		}
		private List<MessageBase> UnpackMessage(byte[] buf, int offset)
		{
			int len = buf.Length;
			List<MessageBase> tMsgList = new List<MessageBase>();
 			while (true)
			{
				if (DataConversion.ByteToNum<Int16>(buf, offset, false) == -1)
				{
					offset += 2;
					if (DataConversion.ByteToNum<Int16>(buf, offset, false) == -1)
						break;
					else
					{
						MessageBase message = new MessageBase(buf, ref offset);
						tMsgList.Add(message);
					}
				}
				if(offset >= (packetMaxLength - 10))
					throw new NotImplementedException();
			}
			if (tMsgList.Count() <= 0)
				throw new NotImplementedException();
			return tMsgList;
		}

		private byte[] PackMessage(List<MessageBase> tMessageList)
		{
			byte[] buf = null;
			int len = tMessageList.Count;
			
			if (len <= 0)
				throw new NotImplementedException();
			foreach (MessageBase messageBase in tMessageList)
			{
				buf = DataConversion.NumToByte((Int16)(-1), buf, false);
				buf = buf.Concat(messageBase.messageBuf).ToArray();
			}
			return buf;
		}
	}
}
