﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using static RouteDirector.MessageBase;

namespace RouteDirector
{	
	public class RouteDirectControl 
	{
		Thread receiveThread;
		TCPSocket tcpSocket;
		System.Timers.Timer heartTime;

		private Queue recMsgQuene = new Queue();
		Semaphore recMsgCount = new Semaphore(0, 1000);

		static readonly object sendLock = new object();
		public static Int16 heartBeatTime = 5;
		static Int16 cycleNum = 0;
		static Int16 ack = 0;
		public bool online = false;

		public RouteDirectControl() {
			tcpSocket = new TCPSocket();
			receiveThread = new Thread(ReceiveHandle) { IsBackground = true };
			HeartTimerInit(heartBeatTime * 2 + 1);
		}
        /// <summary>
        /// 建立连接
        /// </summary>
        /// <param name="ip">ip字符</param>
        /// <param name="port">port字符</param>
        /// <returns>连接状态</returns>
        public int EstablishConnection()
		{
			
			IPEndPoint ipe = new IPEndPoint(IPAddress.Parse("172.16.18.171"), Convert.ToInt32("3000"));
			HeartTimerStart();
			if (tcpSocket.ConnectServer(ipe) == 0)
			{
				receiveThread.Start();
                SendStart();
				online = true;
				Log.log.Debug("EstablishConnection success");
				return 0;
			}
			Log.log.Debug("EstablishConnection fail");
			return -1;
		}

		#region  心跳监视
		private void HeartTimerReset()
		{
			heartTime.Stop();
			heartTime.Start();
			Log.log.Debug("Heartbeat reset");
		}

		private void HeartTimerInit(int s)
		{
			Log.log.Debug("Heartbeat init");
			heartTime = new System.Timers.Timer();
			heartTime.Elapsed += HeartTime_Elapsed;
			heartTime.Interval = s*1000;
			heartTime.AutoReset = false;
			heartTime.Stop();
		}

		private void HeartTimerStart()
		{
			Log.log.Debug("Heartbeat start");
			heartTime.Start();
		}

		private void HeartTime_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			Log.log.Debug("Heartbeat break，break connection");
			StopConnection();			
		}
		private void HeartTimeStop()
		{
			Log.log.Debug("Heartbeat stop");
			heartTime.Stop();
		}
		#endregion

		/// <summary>
		/// 断开连接
		/// </summary>
		public void StopConnection()
		{
			HeartTimeStop();
			receiveThread.Abort();
			//缺少对receive是否完成的判断
			receiveThread = new Thread(ReceiveHandle) { IsBackground = true };
			tcpSocket.DisconnectServer();
			Log.log.Debug("StopConnection success");
			online = false;
			Log.log.Debug("exit");
		}

		private void Reconnection()
		{
			Log.log.Debug("Reconnectioning");
			StopConnection();
			if (EstablishConnection() == 0)
				Log.log.Debug("Reconnection success");
			else
				Log.log.Debug("Reconnection fail");
		}

		private void Unexpect()
		{
			Log.log.Debug("Unexpect");
		}

		private void ReceiveHandle()
		{
			while (true)
			{
				byte[] packetBuf;
				packetBuf = tcpSocket.ReceiveData();
				if (packetBuf == null)
				{
					StopConnection();
				}
				HeartTimerReset();
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

								Packet packet = new Packet(qPacketBuf);
								if(packet.cycleNum != 0)
									ack = packet.cycleNum;
								foreach (MessageBase msg in packet.messageList)
								{
									if (msg.msgId == (Int16)MessageType.HeartBeat)
									{
										HeartBeat heartBeat = new HeartBeat(heartBeatTime);
										SendMsg(heartBeat);
										break;
									}

									if (msg.msgId == (Int16)MessageType.CommsErr)
									{
										Log.log.Debug("Get CommsErr");
										StopConnection();
									}

									if (msg.msgId == (Int16)MessageType.NodeAva)
									{
										Unexpect();
									}

									if (msg.msgId == (Int16)MessageType.NoType)
									{
										Unexpect();
									}

									recMsgQuene.Enqueue(msg);
									recMsgCount.Release();

								}
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
        /// 发送一个报文
        /// </summary>
        /// <param name="packet">packet对象</param>

		private void SendStart()
		{
			Packet packet = new Packet();
			HeartBeat heartBeat = new HeartBeat(heartBeatTime);
			packet.AddMsg(heartBeat);
			packet.AddCycleNum(0, 0);
			tcpSocket.SendData(packet.GetBuf());
			Log.log.Debug("Send start");
			cycleNum = 1;
		}


		public MessageBase WaitMsg()
		{
			recMsgCount.WaitOne();

			return (MessageBase)recMsgQuene.Dequeue();
		}

		public MessageBase GetMsg()
		{
			bool res = recMsgCount.WaitOne(1);
			if(res == true)
				return (MessageBase)recMsgQuene.Dequeue();
			else
				return null;
		}

		public void SendMsg(MessageBase msg)
		{
			lock (sendLock)
			{
				Packet packet = new Packet();
				packet.AddCycleNum(cycleNum, ack);

				packet.AddMsg(msg);

				tcpSocket.SendData(packet.GetBuf());
				Log.log.Debug("Send packet No." + cycleNum);
				cycleNum++;
				if (cycleNum > 99)
					cycleNum = 1;			
			}
		}

		public void FlushMsg()
		{
			Thread.Sleep(2000);
			while (true)
			{
				MessageBase msg = new MessageBase();
				msg = GetMsg();
				if (msg == null)
					break;
				else
					Log.log.Debug("abandon last msg");
			}
		}
	}
}
