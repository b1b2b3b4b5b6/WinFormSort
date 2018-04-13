using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using RouteDIRECTOR.RouteDirectorControl;
using System.Collections;

namespace RouteDirector.TcpSocket
{
	class TCPSocket
	{
		private Socket clientSocket;
		public Queue packetQueue = new Queue();
		public bool ConnectStatus { set; get; }
		public TCPSocket() {
			ConnectStatus = false;
		}

		public int ConnectServer(IPEndPoint ipe)
		{
			if (ConnectStatus == true)
				return -1;
			try
			{
				clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				clientSocket.Connect(ipe);
				//clientSocket.BeginConnect(ipe, new AsyncCallback(EndConnect), clientSocket);
			}
			catch (Exception e)
			{
				throw e;
			}
			ConnectStatus = true;
			return 0;
		}

		public void DisconnectServer()
		{
			try
			{
				if (ConnectStatus == true)
				{
					clientSocket.Disconnect(true);
					ConnectStatus = false;
				}

			}
			catch
			{
			}
		}

		public void ReceiveData()
		{
			int len;
			while (true)
			{
				try
				{
					byte[] buf = new byte[240 * 2];
					len = clientSocket.Receive(buf);
					if(len != 0)
						packetQueue.Enqueue(buf);
				}

				catch
				{
					//throw;
				}
	
			}
			
		}

		public int SendData(byte[] data)
		{
			try
			{
				int len = clientSocket.Send(data);
				return len;
			}
			catch
			{ }
			return -1;
		}
	}
}
