using System;
namespace WinFormSort
{
    public class AGVConfig
    {
        /// <summary>
        /// AGV连接Ip
        /// </summary>
        public String AGVServerIp { get ; set ; }
        /// <summary>
        /// AGV连接端口
        /// </summary>
        public int AGVServerPort { get ; set ; }

        //构造函数
        public AGVConfig(string ip, int port)
        {
            this.AGVServerIp = ip;
            this.AGVServerPort = port;
        }
    }
}
