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


		byte[] messageBuf = new byte[240];
		static public uint terminator = 0xffffffff;
		public int terminatorOffset;

		byte[] padding;
		byte[] packet = new byte[packetMaxLength];

		public Packet(byte[] buf)
		{
			Array.Copy(buf, packet, packetMaxLength);

		}

		public Packet()
		{
	
		}
	}
}
