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
		private Queue packetQuene = new Queue();
		Semaphore packetCount = new Semaphore(0, 10);
		private static Int16 heartBeatTime = 5;
		static Int16 cycleNum = 0;
		static Int16 ack = 0;

		public RouteDirectControl() {
			tcpSocket = new TCPSocket();
			receiveThread = new Thread(ReceiveHandle) { IsBackground = true };
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
				Console.WriteLine("get packet length = " + packetBuf.Length.ToString());
				//Console.Write(BitConverter.ToString(packetBuf));
				packetQuene.Enqueue(packetBuf);
				packetCount.Release();
				
			}
		}

		public Packet WaitPacket()
		{
			packetCount.WaitOne();
			byte[] buf = (byte[])packetQuene.Dequeue();
			Packet packet = new Packet(buf);
			Console.WriteLine("set ack = {0:D}", packet.cycleNum);
			ack = packet.cycleNum;
			return packet;
		}

		public void SendPacket(Packet packet)
		{
			packet.AddCycleNum(cycleNum, ack);
			Console.WriteLine("send! cycleNum = {0:D}, ack = {1:D}", cycleNum, ack);
			tcpSocket.SendData(packet.GetBuf());
			
			cycleNum++;
			if (cycleNum > 99)
				cycleNum = 1;
		}

		public void SendStart()
		{
			Packet packet = new Packet();
			HeartBeat heartBeat = new HeartBeat(heartBeatTime);
			packet.AddMsg(heartBeat);
			packet.AddCycleNum(0, 0);
			tcpSocket.SendData(packet.GetBuf());
			Console.WriteLine("send! cycleNum = {0:D}, ack = {0:D}", 0, 0);
			cycleNum = 1;
		}

		public void SendHeartBeat()
		{
			Packet packetSend = new Packet();
			HeartBeat heartbeat = new HeartBeat(heartBeatTime);
			packetSend.AddMsg(heartbeat);
			SendPacket(packetSend);
		}
	}
}
