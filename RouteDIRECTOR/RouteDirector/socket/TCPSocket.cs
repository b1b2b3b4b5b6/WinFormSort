using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;

namespace RouteDirector.TcpSocket
{
	class TCPSocket
	{
		private Socket clientSocket;

		public bool ConnectStatus { set; get; }
		public TCPSocket() {
			ConnectStatus = false;
		}

		public int ConnectServer(IPEndPoint ipe)
		{
			if (ConnectStatus == true)
				return 0;
			try
			{
				clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				clientSocket.Connect(ipe);
				//clientSocket.BeginConnect(ipe, new AsyncCallback(EndConnect), clientSocket);
			}
			catch
			{
				return -1;
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

		public byte[] ReceiveData()
		{
			byte[] buf = new byte[1024*100];
			int len;
			try
			{
				
				len = clientSocket.Receive(buf);
				byte[] packet = new byte[len];
				Array.Copy(buf, packet, len);
				return packet;
			}

			catch
			{
				throw;
			}
		}

		public int SendData(byte[] data)
		{
			int len;
			try
			{
				len = clientSocket.Send(data);
			}
			catch
			{
				throw;
			}
			return len;
		}
	}
}
