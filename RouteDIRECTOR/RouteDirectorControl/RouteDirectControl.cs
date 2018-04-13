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
	class RouteDirectControl
	{
		byte[] packet = new byte[240];
		byte[,] messageList = new byte [20,240];
		int messageNum;
		void BeginWork(byte[] arr)
		{

		}
		int GetMessage()
		{
			
			return 0;
		}
		void EstablishConnection(IPEndPoint ipe)
		{
			TCPSocket tcpSocket = new TCPSocket();
			if (tcpSocket.ConnectServer(ipe) == 0)
			{
				Thread receiveThread = new Thread(tcpSocket.ReceiveData) { IsBackground = true};
				receiveThread.Start();
			}
			else
			{
			}
			
		}
	}
}
