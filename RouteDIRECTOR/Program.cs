using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
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
			Int64 num = 12345678;
			int offset = 0;
			buf = BitConverter.GetBytes(num);
			//Array.Reverse(buf, 0, buf.Length);
			offset = DataConversion.ByteToNum(buf, offset, ref num, true);
			Console.WriteLine(num);
			Console.WriteLine(offset);
			Console.ReadKey();
		}
	}
}
