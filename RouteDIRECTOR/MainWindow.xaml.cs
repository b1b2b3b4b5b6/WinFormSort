using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RouteDirector;
using System.Threading;
using RouteDirector.PacketProcess;
using static RouteDirector.PacketProcess.MessageBase;

namespace test
{
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : Window
	{
		RouteDirectControl routeDirect = new RouteDirectControl();
        Thread backThread;
        short node1 = 999;
        short node6 = 999;
        short node23 = 999;
		StringBuilder log1 = new StringBuilder();
		StringBuilder log6 = new StringBuilder();
		StringBuilder log23 = new StringBuilder();
		StringBuilder log0 = new StringBuilder();
		System.Timers.Timer heartTime= new System.Timers.Timer();
		public MainWindow()
		{
			InitializeComponent();
            btnConnect.IsEnabled = true;
		}
		private void btnEmpty_Click(object sender, RoutedEventArgs e)
		{
			log1.Clear();
			log6.Clear();
			log23.Clear();
			txt1.Text = "";
			txt2.Text = "";
			txt3.Text = "";

		}

		private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            int res = -1;
            txtStatus.Text = "正在建立连接，请稍等";
			btnConnect.IsEnabled = false;
			Task task = Task.Run(() =>
            {
                backThread = new Thread(BackHandle) { IsBackground = true };
                backThread.Start();
                for (int n = 0; n < 6; n++)
                {
                    Thread.Sleep(1000);
                    if (routeDirect.online == true)
                    {
                        res = 0;
                        break;
                    }
                }
                if (res == 0)
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        txtStatus.Text = "连接建立成功";
                        btnConnect.IsEnabled = false;
                    }));
                
                else
                {
                    DisConnect();
                    this.Dispatcher.Invoke(new Action(() =>
                    {
						btnConnect.IsEnabled = true;
						txtStatus.Text = "连接建立失败，请10s后再次尝试建立连接";
                    }));
                }
            });
        }

        private void DisConnect()
        {       
            routeDirect.StopConnection();
            backThread.Abort();
        }
		 
        private void BackHandle()
        {
			int res = routeDirect.EstablishConnection("172.16.18.171", "3000");
			if (res != 0)
				return;
			HeartTimerInit();

			while (true)
            {
                Packet packetReceive = routeDirect.WaitPacket();
				HeartTimerReset();
				Packet sendPacket = new Packet();
                DivertCmd cmd = null;

				Action falshUI = new Action(() =>
			   {
				   txt1.Text = log1.ToString();
				   txt2.Text = log6.ToString();
				   txt3.Text = log23.ToString();
			   });

				foreach (MessageBase msg in packetReceive.messageList)
                {
                    if (msg.msgId == (Int16)MessageType.DivertReq)
                    {
                        DivertReq temp = (DivertReq)msg;
                        switch (temp.nodeId)
                        {
                            case 1:
                                cmd = new DivertCmd(temp, node1);
                                break;
                            case 6:
								cmd = new DivertCmd(temp, (short)node6);
								break;
                            case 23:
								cmd = new DivertCmd(temp, (short)node23);
								break;
						}
                        sendPacket.AddMsg(cmd);
                    }

                    if (msg.msgId == (Int16)MessageType.DivertRes)
                    {
                        DivertRes temp = (DivertRes)msg;
						Int16 tRes = temp.divertRes;
						StringBuilder str = log0;
						switch (temp.nodeId)
						{
							case 1:
								str = log1;
								break;
							case 6:
								str = log6;
								break;
							case 23:
								str = log23;
								break;
						}
						str.AppendLine(temp.codeStr.Trim() + "-----" + GetLane(temp.laneId) + " -----" + temp.GetResult());
						this.Dispatcher.Invoke(falshUI);
					}

					if (msg.msgId == (Int16)MessageType.HeartBeat)
                    {
                        HeartBeat heart = new HeartBeat(5);
                        sendPacket.AddMsg(heart);
                    }
                }
                if (sendPacket.messageList.Count == 0)
                    routeDirect.SendHeartBeat();
                else
                    routeDirect.SendPacket(sendPacket);
            }
        }

		private void HeartTimerReset()
		{
			heartTime.Stop();
			heartTime.Start();
		}
		private void HeartTimerInit()
		{
			heartTime.Elapsed += HeartTime_Elapsed;
			heartTime.Interval = 11000;
			heartTime.AutoReset = false;
			heartTime.Enabled = true;
			heartTime.Start();
		}
		private void HeartTime_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			DisConnect();
			this.Dispatcher.Invoke(new Action(() =>
			{
				btnConnect.IsEnabled = true;
				txtStatus.Text = "连接建立失败，请10s后再次尝试建立连接";
			}));
			MessageBox.Show("心跳包超时，自动断开连接");
		}

		#region 线路选择
		private string GetLane(short lane)
		{
			switch (lane)
			{
				case 1:
					return "道口1";
				case 2:
					return "道口2";
				case 3:
					return "道口3";
				case 997:
					return "推出";
				case 999:
					return "直行";
				default:
					return "未知线路";
			}
		}

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            node1 = 999;
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            node1 = 1;
        }

        private void RadioButton_Checked_2(object sender, RoutedEventArgs e)
        {
            node1 = 2;
        }

        private void RadioButton_Checked_3(object sender, RoutedEventArgs e)
        {
            node1 = 3;
        }

        private void RadioButton_Checked_4(object sender, RoutedEventArgs e)
        {
            node6 = 999;
        }

        private void RadioButton_Checked_5(object sender, RoutedEventArgs e)
        {
            node6 = 997;
        }

        private void RadioButton_Checked_6(object sender, RoutedEventArgs e)
        {
            node23 = 999;
        }

        private void RadioButton_Checked_7(object sender, RoutedEventArgs e)
        {
            node23 = 997;
        }
		#endregion

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (routeDirect.online == true)
				DisConnect();
		}

	}

}
