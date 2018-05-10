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
namespace RouteDirector
{
	public class RouteDirectControl 
	{
		Thread receiveThread;
		TCPSocket tcpSocket;
		private Queue packetQuene = new Queue();
		Semaphore packetCount = new Semaphore(0, 1000);
		private static Int16 heartBeatTime = 5;
		static Int16 cycleNum = 0;
		static Int16 ack = 0;
		public bool online = false;

		public RouteDirectControl() {
			tcpSocket = new TCPSocket();
			receiveThread = new Thread(ReceiveHandle) { IsBackground = true };
		}
        /// <summary>
        /// 建立连接
        /// </summary>
        /// <param name="ip">ip字符</param>
        /// <param name="port">port字符</param>
        /// <returns>连接状态</returns>
        public int EstablishConnection(string ip, string port)
		{
			
			IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(ip), Convert.ToInt32(port));

			if (tcpSocket.ConnectServer(ipe) == 0)
			{
				receiveThread.Start();
                SendStart();
                WaitPacket();
				online = true;
				return 0;
			}
			return -1;
		}
        /// <summary>
        /// 断开连接
        /// </summary>
		public void StopConnection()
		{
			receiveThread.Abort();
			//缺少对receive是否完成的判断
			receiveThread = new Thread(ReceiveHandle) { IsBackground = true };
			tcpSocket.DisconnectServer();
			online = false;
		}

		private void ReceiveHandle()
		{
			while (true)
			{
				byte[] packetBuf;
				//Console.WriteLine("try receive");
				packetBuf = tcpSocket.ReceiveData();
				
				PacketResolve(packetBuf);
			}
		}

		private void PacketResolve(byte[] packetBuf)
		{
			int start = 0;
			int end = 0;
			int len = packetBuf.Length;
			if (packetBuf[len - 1] != 0xff)
				throw new NotImplementedException();
			if (packetBuf[len - 2] != 0xff)
				throw new NotImplementedException();
			if (packetBuf[len - 3] != 0xff)
				throw new NotImplementedException();
			if (packetBuf[len - 4] != 0xff)
				throw new NotImplementedException();
			while (true)
			{
				if (packetBuf[end] == 0xff)
				{
					end++;
					if (packetBuf[end] == 0xff)
					{
						end++;
						if (packetBuf[end] == 0xff)
						{
							end++;
							if (packetBuf[end] == 0xff)
							{
								end++;
								byte[] qPacketBuf = new byte[end - start];
								Array.Copy(packetBuf, start, qPacketBuf, 0, end - start);
								start = end;
								//Console.WriteLine("get packet length = " + qPacketBuf.Length.ToString());
								//Console.Write(BitConverter.ToString(packetBuf));
								packetQuene.Enqueue(qPacketBuf);
								packetCount.Release();
								if (end == packetBuf.Length)
									break;
							}
							else
								end++;
						}
						else
							end++;
					}
					else
						end++;
				}
				else
					end++;
			}
		}
        
        /// <summary>
        /// 等待接收一个报文
        /// </summary>
        /// <returns>packet对象</returns>
		public Packet WaitPacket()
		{
			packetCount.WaitOne();
			byte[] buf = (byte[])packetQuene.Dequeue();
			Packet packet = new Packet(buf);
			ack = packet.cycleNum;
			//StringBuilder str = new StringBuilder();
			//packet.GetInfo(str);
			return packet;
		}

        /// <summary>
        /// 发送一个报文
        /// </summary>
        /// <param name="packet">packet对象</param>
        public void SendPacket(Packet packet)
		{
			packet.AddCycleNum(cycleNum, ack);
			//Console.WriteLine("send! cycleNum = {0:D}, ack = {1:D}", cycleNum, ack);
			tcpSocket.SendData(packet.GetBuf());
			
			cycleNum++;
			if (cycleNum > 99)
				cycleNum = 1;
		}

		private void SendStart()
		{
			Packet packet = new Packet();
			HeartBeat heartBeat = new HeartBeat(heartBeatTime);
			packet.AddMsg(heartBeat);
			packet.AddCycleNum(0, 0);
			tcpSocket.SendData(packet.GetBuf());
			//Console.WriteLine("send! cycleNum = {0:D}, ack = {0:D}", 0, 0);
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
