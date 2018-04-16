using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
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
			byte[] buf;
			byte[] temp = new byte[240];
			buf = BitConverter.GetBytes((Int16)10);
			Array.Reverse(buf);
			Array.Copy(buf, 0, temp, 0, 2);
			buf = BitConverter.GetBytes((Int16)100);
			Array.Reverse(buf);
			Array.Copy(buf, 0, temp, 2, 2);
			buf = BitConverter.GetBytes((Int16)1000);
			Array.Reverse(buf);
			Array.Copy(buf, 0, temp, 4, 2);
			Packet packet = new Packet(temp);
			Console.ReadKey();
		}
	}
}
