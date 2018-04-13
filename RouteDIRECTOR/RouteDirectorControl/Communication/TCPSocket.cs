using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Threading;
using RouteDIRECTOR.RouteDirectorControl;
using System.Collections;

namespace RouteDirector.TcpSocket
{
	class TCPSocket
	{
		private Socket clientSocket;
		public Queue packetQueue = new Queue();
		public TCPSocket() { }
		public int ConnectServer(IPEndPoint ipe)
		{
			
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
			return 0;
		}

		private void EndConnect(IAsyncResult ar)
		{
			Socket client = (Socket)ar.AsyncState;
			try
			{
				client.EndConnect(ar);
				Thread t = new Thread(ReceiveData) { IsBackground = true };
				t.Start();
			}
			catch (Exception e)
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
					packetQueue.Enqueue(buf);
				}

				catch
				{
					throw;
				}
	
			}
			
		}
	}
}
