using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using WinFormSort.Utility;
using WinFormSort.SendPacket;
using WinFormSort.RecivePacket;
using System.Threading;

namespace WinFormSort
{
    public partial class MainForm : Form
    {
        public static MainForm divert;
        public MainForm()
        {
            InitializeComponent();
            divert = this;
        }
        public Socket ServerSocket;
        SocketChannel sh = new SocketChannel();

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                string host = txtHost.Text;
                int port = Convert.ToInt32(txtPort.Text);
                AGVConfig config = new AGVConfig(host, port);
                
                sh.ConnectServer(config);
                ServerSocket = sh.clientSocket;
                
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog4Ex("连接服务器异常", ex);
            }

        }

		private void btnDisconnect_Click(object sender, EventArgs e)
		{
			sh.Close();
			//停止定时器
			//time.Enabled = false;
			//time.Stop();
			lbconstatus.Text = "断开连接";
		}

		public void ShowMessage(byte[] resultdata)
        {
            try
            {
                //心跳消息
                if (resultdata.Length == 20)
                {
                    txtHeartMsg.AppendText("\n接收到PLC心跳（" + DateTime.Now + "）：" + DataConversion.ByteArrayToHexString(resultdata));
                    //判断是否是PLC的启动心跳
                    if (DataConversion.ByteToStr(resultdata, 0, 2) == "0000")
                        return;
                    else  //0001
                    {
                        //回复PLC心跳
                        HeartBeat hb = new HeartBeat();
                        hb.Acknowlege = DataConversion.byteToHexStr(resultdata, 0, 2);
                        byte[] senddata = hb.ToBytes();
                        sh.SendData(senddata);
                        LogHelper.WriteLog4("回复PLC心跳：" + DataConversion.byteToHexStr(senddata,0,20));
                        txtHeartMsg.AppendText("\n回复PLC心跳（" + DateTime.Now + "）：" + DataConversion.byteToHexStr(senddata,0,20));
                    }

                }
                //分拣信息
                else
                {
                    txtHeartMsg.AppendText("\n接收到PLC消息（" + DateTime.Now + "）：" + DataConversion.ByteArrayToHexString(resultdata));
                   // LogHelper.WriteLog4("接收到PLC消息:" + DataConversion.ByteArrayToHexString(resultdata));
                    List<DivertRsp> rsps = new List<DivertRsp>();
                    List<DivertReq> seqs = new List<DivertReq>();
                    CommonMsgBuffer mb = new CommonMsgBuffer();
                    int REnd = resultdata.Length;

                    #region  包头协议
                    byte[] msghead = mb.Read(resultdata, 10);
                    byte[] msgEnd = DataConversion.CutByteArray(resultdata, resultdata.Length - 2, 2);
                    string Acknowlege = DataConversion.byteToHexStr(resultdata, 0, 2);
                    #endregion

                    StringBuilder sb = new StringBuilder();
                    for (int i = 24; i <= 240; i = i + 2)
                    {
                        sb.Append("FFFF");
                    }

                    for (int i = 10; i < REnd - 4; i = i + 34)
                    {
                        string beginstr = DataConversion.byteToHexStr(resultdata, i, 2);
                        short msgtype = Convert.ToInt16(DataConversion.ByteToStr(resultdata, i + 2, 2));
                        if (msgtype == 3)//分拣结果
                        {
                            DivertRsp rsp = new DivertRsp();
                            rsp.LoadFrom(resultdata,i);

                            LogHelper.WriteLog4("接收到PLC的分拣结果：扫码节点为" + rsp.Node_ID + ", 箱子条码为：" + rsp.Code_Str + ", 分拣线路为：" + rsp.Lane_ID + ", 分拣结果为：" + rsp.Div_Result );
                            rtxDivertMsg.AppendText("接收到PLC的分拣结果：扫码节点为" + rsp.Node_ID + ", 箱子条码为：" + rsp.Code_Str+", 分拣线路为："+rsp.Lane_ID+", 分拣结果为："+rsp.Div_Result+ "\n \n");
                        }
                        else if (msgtype == 1)//分拣请求
                        {
                            short Node_ID = Convert.ToInt16(DataConversion.ByteToStr(resultdata, i + 4, 2));
                            string carseq = DataConversion.byteToHexStr(resultdata, i + 6, 2);
                            string barcode = Encoding.ASCII.GetString(DataConversion.CutByteArray(resultdata, i + 12, 4));
                            int code = 0;
                            if (barcode != "?   ")//未扫描到条码
                            {
                                code = Convert.ToInt32(barcode);
                            }
                            LogHelper.WriteLog4("接收到PLC的分拣请求：扫码节点为" + Node_ID + ",箱子条码为：" + barcode);
                            rtxDivertMsg.AppendText("接收到PLC的分拣请求：扫码节点为"+Node_ID+",箱子条码为："+barcode+ "\n \n");

                            //组合分拣命令
                            StringBuilder strb = new StringBuilder();
                            if (CommonMsgBuffer.CycleNumber > 99)
                                CommonMsgBuffer.CycleNumber = 1;
                            else
                                CommonMsgBuffer.CycleNumber += 1;
                            short CycleNumber = CommonMsgBuffer.CycleNumber;
                            strb.Append(CycleNumber.ToString("X2").PadLeft(4, '0'));
                            strb.Append(30.ToString("X2").PadLeft(4, '0'));
                            strb.Append(21.ToString("X2").PadLeft(4, '0'));
                            strb.Append(Acknowlege);
                            strb.Append(0.ToString("X2").PadLeft(4, '0'));
                            strb.Append("FFFF");
                            int lineid=999;
                            if (Node_ID == 1)
                            {
                                strb.Append(2.ToString("X2").PadLeft(4, '0'));
                                strb.Append(Node_ID.ToString("X2").PadLeft(4, '0'));
                                strb.Append(carseq);
                                strb.Append(0.ToString("X2").PadLeft(4, '0'));
                                
                                if (code%10==0 || code%10 ==5)
                                {
                                    lineid = 1;
                                   
                                }
                                else if (code % 10 == 1 || code % 10 == 6)
                                {
                                    lineid = 2;
                                }
                                else if (code % 10 == 2 || code % 10 == 7)
                                {
                                    lineid =3;//3线路
                                }
                                else
                                {
                                    lineid = 999;//直行
                                }
                                strb.Append(lineid.ToString("X2").PadLeft(4, '0'));
                            }
                            else
                            {
                                strb.Append(2.ToString("X2").PadLeft(4, '0'));
                                strb.Append(Node_ID.ToString("X2").PadLeft(4, '0'));
                                strb.Append(carseq);
                                strb.Append(0.ToString("X2").PadLeft(4, '0'));
                                if (Node_ID == 6)
                                {
                                    if (code % 10 == 3 || code % 10 == 8)
                                    {
                                        lineid = 997;
                                        //推出传输带
                                    }
                                    else
                                    {
                                        lineid = 999;//直行
                                    }
                                    strb.Append(lineid.ToString("X2").PadLeft(4, '0'));
                                }
                                else if (Node_ID == 23)
                                {
                                    if (code % 10 == 4 || code % 10 == 9)
                                    {
                                        lineid = 997;//推出传输带
                                    }
                                    else
                                    {
                                        lineid = 999;//直行
                                    }
                                    strb.Append(lineid.ToString("X2").PadLeft(4, '0'));
                                }
                            }
                            strb.Append(sb);
                            string str = strb.ToString();

                            byte[] SendData = DataConversion.StringToHexArray(str);
                            LogHelper.WriteLog4("主机发送的分拣命令：扫码节点：" + Node_ID + ",分拣线号：" + lineid + ",箱子条码：" + barcode);
                            rtxDivertMsg.AppendText("主机发送的分拣命令：扫码节点："+Node_ID+",分拣线号："+lineid+",箱子条码："+barcode+ "\n \n");
                            sh.SendData(SendData);
                            
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog4Ex("消息处理异常", ex);
                // throw;
            }


        }

		public void SendStartUp()
        {
            lbconstatus.Text = "连接成功";

            HeartBeat heart = new HeartBeat();
            heart.Acknowlege ="0000";
            byte[] senddata = heart.ToBytes(1);
            sh.SendData(senddata);
            
            txtHeartMsg.AppendText("\n 启动心跳：" + DataConversion.byteToHexStr(senddata,0,20));
        }

		private void btnDataAnalyze_Click(object sender, EventArgs e)
		{
			string HexString = @" 00 02 00 15 00 1E 00 00 00 00 FF FF 00 03 00 01 2A 56 00 04 00 01 31 32 30 32 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20  FF FF 00 03 00 01 2A 57 00 04 00 03 3F 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 FF FF 00 03 00 01 2A 58 00 04 00 03 3F 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 FF FF 00 03 00 06 1E E2 03 E7 00 01 3F 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 FF FF 00 03 00 06 1E E3 03 E7 00 01 3F 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 FF FF 00 03 00 06 1E E4 03 E7 00 01 31 31 38 34 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 FF FF FF FF 00 03 00 15 00 1E 00 00 00 00 FF FF 00 03 00 06 21 8A 03 E7 00 01 31 32 32 39 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 FF FF 00 03 00 06 21 8B 03 E7 00 01 31 30 33 36 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 FF FF 00 03 00 06 21 8C 03 E7 00 01 31 31 30 34 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 FF FF 00 03 00 06 21 8D 03 E7 00 01 31 30 32 34 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 FF FF 00 03 00 17 21 7C 03 E7 00 01 31 30 33 36 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 FF FF 00 03 00 17 21 7D 03 E7 00 01 31 31 30 34 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 20 FF FF FF FF ";
			byte[] data = DataConversion.StringToHexArray(HexString);
			//ShowMessage(data);




			HeartBeat heart = new HeartBeat();
			byte[] senddata = heart.ToBytes(1);

			rtxDivertMsg.AppendText("测试数据-启动心跳：" + DataConversion.ByteArrayToHexString(senddata) + "\n");
			//CommonMsgBuffer.AliveTime = DateTime.Now.AddSeconds(-100);
			//DateTime nowTime = DateTime.Now;
			//if (BaseInfo.DateDiff(nowTime, CommonMsgBuffer.AliveTime) > 3)
			//{
			//    //重新发送启动心跳
			//    HeartBeat heart = new HeartBeat();
			//    byte[] senddate = heart.ToBytes(1);
			//    //AsynSendData(senddate);
			//}
		}

		private void MainForm_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void btnheart_Click(object sender, EventArgs e)
        {
            //sh.ReceiveData();
            SendStartUp();
        }

		private void txtHost_TextChanged(object sender, EventArgs e)
		{

		}
	}
}
