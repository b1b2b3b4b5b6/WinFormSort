using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using WinFormSort.Utility;
using System.Collections.Generic;
using WinFormSort.RecivePacket;
using WinFormSort.SendPacket;
using System.Threading;

namespace WinFormSort
{
    public class SocketChannel 
    {
        public  Socket clientSocket;

        private Thread receiveThread;

        static Type log = typeof(SocketChannel);
        /// <summary>
        /// 建立通讯连接
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        int count = 1;
        public void ConnectServer(AGVConfig agv)
        {
            try
            {
                IPAddress Host = IPAddress.Parse(agv.AGVServerIp);
                IPEndPoint ipe = new IPEndPoint(Host, agv.AGVServerPort);
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                LogHelper.WriteLog4("开始请求终端：" + ipe.ToString());

                clientSocket.BeginConnect(ipe,new AsyncCallback(EndConnect),clientSocket);
              
                receiveThread = new Thread(ReceiveData);
                receiveThread.IsBackground = true;
                
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog4Ex("请求远程终端失败", ex);
                //标记状态
                BaseInfo.ConnectStatus = BaseInfo.NetOptResType.DisConn;
                //开始重连
                LogHelper.WriteLog4("请求远程终端失败，开始第" + count + "次重试");
                count = count + 1;
                AGVConfig config = new AGVConfig(agv.AGVServerIp, agv.AGVServerPort);
                ConnectServer(config);
                throw new NotImplementedException();
            }
        }

        private void EndConnect(IAsyncResult async)
        {
            if(clientSocket!=null)
            {
                clientSocket.EndConnect(async);

                if (clientSocket.Connected)
                {
                    CommonMsgBuffer.AliveTime = DateTime.Now;
                    receiveThread.Start();
                    BaseInfo.ConnectStatus = BaseInfo.NetOptResType.ConSucc;
                    MainForm.divert.SendStartUp();//发送启动协议
                }
                    
                else
                    BaseInfo.ConnectStatus = BaseInfo.NetOptResType.DisConn;
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="data"></param>
        public void SendData(byte[] data)
        {
            try
            {
                if (clientSocket != null && clientSocket.Connected)
                {
                    int result = clientSocket.Send(data);

                    if (result > 0)
                    {
                        LogHelper.WriteLog4("发送的原始报文的信息：" + DataConversion.ByteArrayToHexString(data));
                        BaseInfo.MsgStatus = Convert.ToInt32(BaseInfo.NetOptResType.OnError_MsgRead);
                    }
                    else
                    {
                        LogHelper.WriteLog4("发送消息失败，报文内容为：" + DataConversion.ByteArrayToHexString(data));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }

        #region 接收数据
        byte[] MsgBuffer = new byte[1024*10];
        public void ReceiveData()
        {
            try
            {
                //if (!clientSocket.Connected)
                //{
                //    LogHelper.WriteLog4("通讯连接中断", Level.WARN);
                //    return;
                //}
                while(clientSocket.Connected)
                {

                    #region 监测心跳
                    if (CommonMsgBuffer.AliveTime.ToString() != "0001/1/1 0:00:00")
                    {
                        DateTime nowTime = DateTime.Now;
                        if (BaseInfo.DateDiff(nowTime, CommonMsgBuffer.AliveTime) > 5)
                        {
                            LogHelper.WriteLog4("心跳中断，重启心跳");
                            CommonMsgBuffer.AliveTime = DateTime.Now;
                            //重新发送启动心跳
                            MainForm.divert.SendStartUp();

                        }
                    }
                    #endregion
                    int iMsglength = clientSocket.Available;
                    if (iMsglength > 0)
                    {
                        int REnd = clientSocket.Receive(MsgBuffer, 0, iMsglength, SocketFlags.None);
                        byte[] data = new byte[REnd];
                        Array.Copy(MsgBuffer, 0, data, 0, REnd);
                        CommonMsgBuffer.AliveTime = DateTime.Now;
                        DoRead(data);
                    }
                    else
                    {
                        //LogHelper.WriteLog4("可接收数据为0！");
                    }
                }
                
           }
            catch (Exception ex)
            {
                LogHelper.WriteLog4Ex("接收消息异常：", ex);
            }
        }
        private void DoRead(byte[] buffer)
        {

            string HexString = DataConversion.ByteArrayToHexString(buffer);
            LogHelper.WriteLog4("接收的数据长度:" + buffer.Length);
            LogHelper.WriteLog4("接收原始报文的信息：" + HexString);


            List<DivertRsp> rsps = new List<DivertRsp>();
            List<DivertReq> seqs = new List<DivertReq>();
            //针对不同数据进行处理
            if (buffer.Length == 20)
            {
                MainForm.divert.ShowMessage(buffer);
            }
            else
            {
                #region 解决半包和粘包
                string[] splitchar = new string[] { "FF FF FF FF " };
                string[] strArray = HexString.Split(splitchar, StringSplitOptions.RemoveEmptyEntries);
                if (HexString.EndsWith("FF FF FF FF "))
                {
                    for (int i = 0; i < strArray.Length; i++)
                    {
                        if (string.IsNullOrEmpty(strArray[i]))
                            return;
                        byte[] checkdata = DataConversion.StringToHexArray(strArray[i] + "FF FF FF FF ");
                        //判断包头是否完整
                        if (checkdata[3] == 21 && checkdata[5] == 30)
                        {
                            MainForm.divert.ShowMessage(checkdata);
                        }
                        else
                        {
                            //后半段消息合并至缓存中
                            CommonMsgBuffer.CacheHexStr += strArray[i] + "FF FF FF FF ";
                            CommonMsgBuffer._byteBuffer = DataConversion.StringToHexArray(CommonMsgBuffer.CacheHexStr);
                            //判断缓存数据是否已经组合成一个完整的消息包
                            if (CommonMsgBuffer._byteBuffer[3] == 21 && CommonMsgBuffer._byteBuffer[5] == 30)
                            {
                                MainForm.divert.ShowMessage(CommonMsgBuffer._byteBuffer);
                                //清空缓存区
                                Array.Clear(CommonMsgBuffer._byteBuffer, 0, CommonMsgBuffer._byteBuffer.Length);
                                CommonMsgBuffer.CacheHexStr = "";
                            }
                        }
                    }
                }
                else //包含半包前半段数据
                {
                    for (int i = 0; i < strArray.Length; i++)
                    {
                        byte[] halfdata = DataConversion.StringToHexArray(strArray[i]);
                        //判断数据完整性
                        if (halfdata.Length < 5)
                            return;
                        if (halfdata[3] == 21 && halfdata[5] == 30)
                        {
                            if ((halfdata.Length - 10) % 34 == 0)
                            {
                                byte[] fulldata = DataConversion.StringToHexArray(strArray[i] + "FF FF FF FF ");
                                MainForm.divert.ShowMessage(fulldata);
                            }
                            else
                            {
                                //前半段消息放入缓存中
                                CommonMsgBuffer.CacheHexStr = strArray[i];
                            }
                        }

                    }
                }
                #endregion

            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            #region  分拣机通讯消息解析
            try
            {
                if (!clientSocket.Connected)
                {
                    LogHelper.WriteLog4("通讯连接中断",Level.WARN);
                    return;
                }

                #region 监测心跳
                if (CommonMsgBuffer.AliveTime.ToString() != "0001/1/1 0:00:00")
                {
                    DateTime nowTime = DateTime.Now;
                    if (BaseInfo.DateDiff(nowTime, CommonMsgBuffer.AliveTime) > 5)
                    {
                        LogHelper.WriteLog4("心跳中断，重启心跳");
                        CommonMsgBuffer.AliveTime = DateTime.Now;
                        //重新发送启动心跳
                        MainForm.divert.SendStartUp();

                    }
                }
                #endregion

                int REnd = clientSocket.EndReceive(ar);
                if (REnd > 0)
                {
                    //转移到指定长度的新数组中
                    byte[] data = new byte[REnd];
                    Array.Copy(MsgBuffer, 0, data, 0, REnd);
                    string HexString = DataConversion.ByteArrayToHexString(data);
                    LogHelper.WriteLog4("接收的数据长度:" + REnd);
                    LogHelper.WriteLog4("接收原始报文的信息：" + HexString);

                    List<DivertRsp> rsps = new List<DivertRsp>();
                    List<DivertReq> seqs = new List<DivertReq>();
                    //针对不同数据进行处理
                    if (REnd == 20)
                    {
                        CommonMsgBuffer.AliveTime =DateTime.Now;
                        MainForm.divert.ShowMessage(data);
                    }
                    else
                    {
                        #region 解决半包和粘包
                        string[] splitchar = new string[] { "FF FF FF FF " };
                        string[] strArray = HexString.Split(splitchar, StringSplitOptions.RemoveEmptyEntries);
                        if (HexString.EndsWith("FF FF FF FF "))
                        {
                            for (int i = 0; i < strArray.Length; i++)
                            {
                                if (string.IsNullOrEmpty(strArray[i]))
                                    return;
                                byte[] checkdata = DataConversion.StringToHexArray(strArray[i] + "FF FF FF FF ");
                                //判断包头是否完整
                                if (checkdata[3] == 21 && checkdata[5] == 30)
                                {
                                    MainForm.divert.ShowMessage(checkdata);
                                }
                                else
                                {
                                    //后半段消息合并至缓存中
                                    CommonMsgBuffer.CacheHexStr += strArray[i] + "FF FF FF FF ";
                                    CommonMsgBuffer._byteBuffer = DataConversion.StringToHexArray(CommonMsgBuffer.CacheHexStr);
                                    //判断缓存数据是否已经组合成一个完整的消息包
                                    if (CommonMsgBuffer._byteBuffer[3] == 21 && CommonMsgBuffer._byteBuffer[5] == 30)
                                    {
                                        MainForm.divert.ShowMessage(CommonMsgBuffer._byteBuffer);
                                        //清空缓存区
                                        Array.Clear(CommonMsgBuffer._byteBuffer, 0, CommonMsgBuffer._byteBuffer.Length);
                                        CommonMsgBuffer.CacheHexStr = "";
                                    }
                                }
                            }
                        }
                        else //包含半包前半段数据
                        {
                            for (int i = 0; i < strArray.Length; i++)
                            {
                                byte[] halfdata = DataConversion.StringToHexArray(strArray[i]);
                                //判断数据完整性
                                if (halfdata.Length < 5)
                                    return;
                                if (halfdata[3] == 21 && halfdata[5] == 30)
                                {
                                    if ((halfdata.Length - 10) % 34 == 0)
                                    {
                                        byte[] fulldata = DataConversion.StringToHexArray(strArray[i] + "FF FF FF FF ");
                                        MainForm.divert.ShowMessage(fulldata);
                                    }
                                    else
                                    {
                                        //前半段消息放入缓存中
                                        CommonMsgBuffer.CacheHexStr = strArray[i];
                                    }
                                }

                            }
                        }
                        #endregion

                    }
                    //继续接收
                    clientSocket.BeginReceive(MsgBuffer, 0, MsgBuffer.Length, 0, new AsyncCallback(ReceiveCallback), null);
                }
            }
            catch (SocketException ex)
            {
                LogHelper.WriteLog4Ex("接收消息异常：",ex);
                //Console.WriteLine(ex.Message);
            }
            #endregion
        }

        #endregion

        public void Close()
        {
            try
            {
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog4Ex("关闭socket连接异常", ex);
            }
        }
    }
}
