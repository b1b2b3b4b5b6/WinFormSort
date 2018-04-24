using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using RouteDirector;
using RouteDirector.PacketProcess;
using RouteDirector.Utility;

namespace RouteDIRECTOR
{
	static class Program
	{
		static RouteDirectControl routeDirect = new RouteDirectControl();
		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		//[STAThread]
		static void Main()
		{

			//Application.EnableVisualStyles();
			//Application.SetCompatibleTextRenderingDefault(false);
			//Application.Run(new Form1());

			
			int res;
			res = routeDirect.EstablishConnection("172.16.18.171", "3000");
			if (res == 0)
				Console.WriteLine("连接成功");

			routeDirect.SendStart();
			routeDirect.WaitPacket();
			while (true)
			{
				Packet packetReceive = routeDirect.WaitPacket();
				StringBuilder str = new StringBuilder();
				packetReceive.GetInfo(str);
				Console.Write(str);
				routeDirect.SendHeartBeat();
			}
			Console.ReadKey();
			routeDirect.StopConnection();
	}
}
