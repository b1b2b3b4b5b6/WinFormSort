using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RouteDirector.PacketAnalyze;
using RouteDirector.PacketProcess;
using RouteDirector.TcpSocket;

namespace RouteDIRECTOR.RouteDirectorControl
{
	public class RouteDirectControl
	{
		byte[] packet = new byte[240];
		byte[,] messageList = new byte [20,240];
		int messageNum;
		Thread receiveThread;
		TCPSocket tcpSocket;

		public RouteDirectControl() {
			tcpSocket = new TCPSocket();
			receiveThread = new Thread(tcpSocket.ReceiveData) { IsBackground = true };
		}
		public void BeginWork(byte[] arr)
		{

		}

		public int EstablishConnection(string ip, string port)
		{
			
			IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(ip), Convert.ToInt32(port));

			if (tcpSocket.ConnectServer(ipe) == 0)
			{
				
				receiveThread.Start();

				return 0;
			}
			else
			{
			}
	
			return -1;
		}

		public void StopConnection()
		{
			receiveThread.Abort();
			receiveThread = new Thread(tcpSocket.ReceiveData) { IsBackground = true };
			tcpSocket.DisconnectServer();
		}

		int GetMessage()
		{

			return 0;
		}
	}
}
