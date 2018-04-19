using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RouteDirector;
using RouteDirector.PacketProcess;
using RouteDirector.Utility;

namespace RouteDIRECTOR
{
	static class Program
	{
		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		//[STAThread]
		static void Main()
		{

			//Application.EnableVisualStyles();
			//Application.SetCompatibleTextRenderingDefault(false);
			//Application.Run(new Form1());

			RouteDirectControl routeDirect = new RouteDirectControl();
			int res;
			res = routeDirect.EstablishConnection("172.16.18.171", "3000");
			if (res == 0)
				Console.WriteLine("连接成功");

			HeartBeat heartBeat = new HeartBeat(4);
			Packet packet = new Packet(0, 0);
			packet.AddMsg(heartBeat);
			routeDirect.SendPacket(packet.GetBuf());

			

			StringBuilder str = new StringBuilder();
			//packet.GetInfo(str);
			Console.Write(str);
			Console.ReadKey();
			routeDirect.StopConnection();
		}
	}
}
