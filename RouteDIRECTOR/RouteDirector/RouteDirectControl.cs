using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RouteDirector.PacketProcess;
using RouteDirector.TcpSocket;
using RouteDIRECTOR;
namespace RouteDirector
{
	public class RouteDirectControl 
	{
		Thread receiveThread;
		TCPSocket tcpSocket;

		public RouteDirectControl() {
			tcpSocket = new TCPSocket();
			receiveThread = new Thread(ReceiveHandle) { IsBackground = true };
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
			return -1;
		}

		public void StopConnection()
		{
			receiveThread.Abort();
			//缺少对receive是否完成的判断
			receiveThread = new Thread(ReceiveHandle) { IsBackground = true };
			tcpSocket.DisconnectServer();
		}

		private void ReceiveHandle()
		{
			while (true)
			{
				byte[] packetBuf;
				Console.WriteLine("try receive");
				packetBuf = tcpSocket.ReceiveData();
				Console.WriteLine("receive complete");
				if (packetBuf != null)
				{
					//开启新task来处理接收的最新报文
					Task task = new Task(() => { PacketAnalyze(packetBuf); });
					task.Start();
				}
				else
					Console.WriteLine("receive null packet");
			}
		}

		private void PacketAnalyze(byte[] tPacket)
		{
			Console.WriteLine("get packet length = " + tPacket.Length.ToString());
			Packet packet = new Packet(tPacket);
			StringBuilder str = new StringBuilder();
			Console.Write(packet.GetInfo(str));
		}

		public void SendPacket(byte[] buf)
		{
			int len = tcpSocket.SendData(buf);
			Console.WriteLine("send packet length = " + len.ToString());
		}
	}
}
