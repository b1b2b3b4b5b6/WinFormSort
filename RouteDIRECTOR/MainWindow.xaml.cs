using RouteDirector;
using RouteDirector.PacketProcess;
using System;
using System.Text;
using System.Threading;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
        short node1Lane = 4;
        short node6Lane = 999;
        short node23Lane = 999;
		StringBuilder log1 = new StringBuilder();
		StringBuilder log6 = new StringBuilder();
		StringBuilder log23 = new StringBuilder();
		System.Timers.Timer heartTime;
		Boolean autoMode = true;
		int[,] limit = new int[5,3];
		bool node6Unknow;
		bool node23Unknow;
		public MainWindow()
		{
			InitializeComponent();
            btnConnect.IsEnabled = true;
			autoSwitch.IsChecked = true;
			LoadLimit();
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

		private void CheckBox_Checked(object sender, RoutedEventArgs e)
		{
			node1Hand.IsEnabled = false;
			node6Hand.IsEnabled = false;
			node23Hand.IsEnabled = false;
			node1Auto.IsEnabled = true;
			node6Auto.IsEnabled = true;
			node23Auto.IsEnabled = true;
			btnSave.IsEnabled = true;
			autoMode = true;
		}

		private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
		{
			node1Hand.IsEnabled = true;
			node6Hand.IsEnabled = true;
			node23Hand.IsEnabled = true;
			node1Auto.IsEnabled = false;
			node6Auto.IsEnabled = false;
			node23Auto.IsEnabled = false;
			btnSave.IsEnabled = false;
			autoMode = false;
		}

		#region 判断，保存，读取条码范围
		private void SaveLimit()
		{
			byte[] result = new byte[limit.Length * sizeof(int)];
			Buffer.BlockCopy(limit, 0, result, 0, result.Length);
			File.WriteAllBytes("limit", result);
		}

		private void LoadLimit()
		{
			//string path = Environment.CurrentDirectory;
			string path = "limit";
			if (File.Exists(path))
			{
				byte[] result = File.ReadAllBytes(path);
				Buffer.BlockCopy(result, 0, limit, 0, result.Length);
				if(limit[0,0] == 1)
				{
					limit1Left.Text = limit[0, 1].ToString();
					limit1Right.Text = limit[0, 2].ToString();
				}
				if (limit[1, 0] == 1)
				{
					limit2Left.Text = limit[1, 1].ToString();
					limit2Right.Text = limit[1, 2].ToString();
				}

				if (limit[2, 0] == 1)
				{
					limit3Left.Text = limit[2, 1].ToString();
					limit3Right.Text = limit[2, 2].ToString();
				}

				if (limit[3, 0] == 1)
				{
					limit6Left.Text = limit[3, 1].ToString();
					limit6Right.Text = limit[3, 2].ToString();
				}

				if (limit[4, 0] == 1)
				{
					limit23Left.Text = limit[4, 1].ToString();
					limit23Right.Text = limit[4, 2].ToString();
				}
			}
		}

		private void btnSave_Click(object sender, RoutedEventArgs e)
		{
			int[,] tLimit = new int[5, 3];
			Action<TextBox, TextBox, int> getLimit = ((txtLeft, txtRight, group) =>
			{
				string strLeft = txtLeft.Text;
				string strRight = txtRight.Text;
				if ((strLeft == "") || (strRight == ""))
				{
					tLimit[group, 0] = 0;
				}
				else
				{
					tLimit[group, 0] = 1;
					tLimit[group, 1] = Convert.ToInt32(strLeft);
					tLimit[group, 2] = Convert.ToInt32(strRight);
				}
			});

			try
			{
				getLimit(limit1Left, limit1Right, 0);
				getLimit(limit2Left, limit2Right, 1);
				getLimit(limit3Left, limit3Right, 2);
				getLimit(limit6Left, limit6Right, 3);
				getLimit(limit23Left, limit23Right, 4);
			}
			catch
			{
				MessageBox.Show("请检查条码范围，只能输入整数");
				return;
			}

			for (int n = 0; n < 5; n++)
			{
				if (tLimit[n, 0] == 1)
				{
					if (tLimit[n, 1] > tLimit[n, 2])
					{
						MessageBox.Show("请检查条码范围，右值应大于等于左值");
						return;
					}
				}
			}

			limit = (int[,])tLimit.Clone();
			SaveLimit();
			return;
		}

		private void check6_Checked(object sender, RoutedEventArgs e)
		{
			node6Unknow = true;
		}

		private void check6_Unchecked(object sender, RoutedEventArgs e)
		{
			node6Unknow = false;
		}

		private void check23_Checked(object sender, RoutedEventArgs e)
		{
			node23Unknow = true;
		}

		private void check23_Unchecked(object sender, RoutedEventArgs e)
		{
			node23Unknow = false;
		}

		private short GetLane(string code, int node)
		{
			code = code.Trim();
			if (code == "?")
			{
				if (node == 6)
				{
					if (node6Unknow == true)
						return 997;
					else
						return 999;
				}
				if (node == 23)
				{
					if (node23Unknow == true)
						return 997;
					else
						return 999;
				}
			}

			int num = Convert.ToInt32(code);
			switch (node)
			{
				case 1:
					if ((num >= limit[0, 1]) && (num <= limit[0, 2]) && (limit[0,0] == 1))
						return 1;
					if ((num >= limit[1, 1]) && (num <= limit[1, 2]) && (limit[1, 0] == 1))
						return 2;
					if ((num >= limit[2, 1]) && (num <= limit[2, 2]) && (limit[2, 0] == 1))
						return 3;
					else
						return 4;

				case 3:
					if ((num >= limit[3, 1]) && (num <= limit[3, 2]) && (limit[4, 0] == 1))
						return 997;
					else
						return 999;

				case 23:
					if ((num >= limit[4, 1]) && (num <= limit[4, 2]) && (limit[5, 0] == 1))
						return 997;
					else
						return 999;
			}
			return 0;
		}
		#endregion

		#region 业务
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
				   scro1.ScrollToEnd();
				   scro2.ScrollToEnd();
				   scro3.ScrollToEnd();
			   });

				foreach (MessageBase msg in packetReceive.messageList)
                {
                    if (msg.msgId == (Int16)MessageType.DivertReq)
                    {
                        DivertReq temp = (DivertReq)msg;
						if (autoMode == true)
						{
							short lane = GetLane(temp.codeStr, temp.nodeId);
							cmd = new DivertCmd(temp, lane);
						}
						else
						{
							switch (temp.nodeId)
							{
								case 1:
									cmd = new DivertCmd(temp, node1Lane);
									break;
								case 6:
									cmd = new DivertCmd(temp, node6Lane);
									break;
								case 23:
									cmd = new DivertCmd(temp, node23Lane);
									break;
								default:
									continue;
							}
						}

                        sendPacket.AddMsg(cmd);
                    }

                    if (msg.msgId == (Int16)MessageType.DivertRes)
                    {
                        DivertRes temp = (DivertRes)msg;
						Int16 tRes = temp.divertRes;
						switch (temp.nodeId)
						{
							case 1:
								log1.AppendLine(temp.codeStr.Trim() + "-----" + GetLaneStr(temp.laneId) + " -----" + temp.GetResult());
								this.Dispatcher.Invoke(new Action(() =>
								{
									txt1.Text = log1.ToString();
									scro1.ScrollToEnd();
								}));
								break;
							case 6:
								log6.AppendLine(temp.codeStr.Trim() + "-----" + GetLaneStr(temp.laneId) + " -----" + temp.GetResult());
								this.Dispatcher.Invoke(new Action(() =>
								{
									txt2.Text = log6.ToString();
									scro2.ScrollToEnd();
								}));
								break;
							case 23:
								log23.AppendLine(temp.codeStr.Trim() + "-----" + GetLaneStr(temp.laneId) + " -----" + temp.GetResult());
								this.Dispatcher.Invoke(new Action(() =>
								{
									txt3.Text = log23.ToString();
									scro3.ScrollToEnd();
								}));
								break;
							default:
								continue;
						}
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

		#endregion

		#region  心跳监视
		private void HeartTimerReset()
		{
			heartTime.Stop();
			heartTime.Start();
		}

		private void HeartTimerInit()
		{
			heartTime = new System.Timers.Timer();
			heartTime.Elapsed += HeartTime_Elapsed;
			heartTime.Interval = 11000;
			heartTime.AutoReset = false;
			heartTime.Start();
		}

		private void HeartTime_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			DisConnect();
			heartTime.Stop();
			heartTime.Close();
			this.Dispatcher.Invoke(new Action(() =>
			{
				btnConnect.IsEnabled = true;
				txtStatus.Text = "连接建立失败，请10s后再次尝试建立连接";
			}));
			MessageBox.Show("心跳包超时，自动断开连接");
		}
		#endregion

		#region 线路选择
		private string GetLaneStr(short lane)
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
				case 4:
					return "直行";
				case 999:
					return "直行";
				default:
					return lane.ToString();
			}
		}

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            node1Lane = 4;
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            node1Lane = 1;
        }

        private void RadioButton_Checked_2(object sender, RoutedEventArgs e)
        {
            node1Lane = 2;
        }

        private void RadioButton_Checked_3(object sender, RoutedEventArgs e)
        {
            node1Lane = 3;
        }

        private void RadioButton_Checked_4(object sender, RoutedEventArgs e)
        {
            node6Lane = 999;
        }

        private void RadioButton_Checked_5(object sender, RoutedEventArgs e)
        {
            node6Lane = 997;
        }

        private void RadioButton_Checked_6(object sender, RoutedEventArgs e)
        {
            node23Lane = 999;
        }

        private void RadioButton_Checked_7(object sender, RoutedEventArgs e)
        {
            node23Lane = 997;
        }
		#endregion

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (routeDirect.online == true)
				DisConnect();
		}

	}

}
