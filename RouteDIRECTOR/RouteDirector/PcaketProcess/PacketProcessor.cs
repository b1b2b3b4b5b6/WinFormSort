using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RouteDirector;
namespace RouteDirector.PacketProcess
{
	class PacketProcessor
	{

		public PacketProcessor(byte[] buf)
		{
			Packet packet = new Packet(buf);
		}

		public int GetMessage(Queue message)
		{

			return 0;
		}
	}
}
