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
		static int packetMaxLength = 240;

		public Int16 cycleNum;
		public Int16 senderId;
		public Int16 receiverId;
		public Int16 ack;
		public Int16 transportError;


		byte[] messageBuf;
		//static public uint terminator = 0xffffffff;
		//public int terminatorOffset;

		//byte[] padding;
		byte[] packet;

		public Packet(byte[] buf)
		{
			int offset = 0;
			if (buf.Length > packetMaxLength)
				return;
			Array.Copy(buf, packet, buf.Length);
			offset += DataConversion.ByteToNum(buf, offset, ref cycleNum, false);
			offset += DataConversion.ByteToNum(buf, offset, ref senderId, false);
			offset += DataConversion.ByteToNum(buf, offset, ref receiverId, false);
			offset += DataConversion.ByteToNum(buf, offset, ref ack, false);
			offset += DataConversion.ByteToNum(buf, offset, ref transportError, false);
		}

		public Packet()
		{
	
		}
	}
}
