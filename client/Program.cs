using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.ClientEngine;
using SuperSocket.ProtoBase;
using SuperSocket.ClientEngine.Protocol;
using System.Net;

namespace client
{
	class Program
	{
		static void Main(string[] args)
		{
			var client = new EasyClient();

			// Initialize the client with the receive filter and request handler
			client.Initialize(new MyReceiveFilter(), (request) =>
			{
				// handle the received request
				Console.WriteLine("1234");
			});
			Task<bool> connected = client.ConnectAsync(new IPEndPoint(IPAddress.Parse("172.16.18.171"), 3000));
			if (connected.Result)
			{
				Console.WriteLine("connect ok");
				// Send data to the server
				client.Send(Encoding.ASCII.GetBytes("LOGIN kerry"));
			}

			Console.ReadKey();
		}
	}

	public class MyReceiveFilter : TerminatorReceiveFilter<StringPackageInfo>
	{
		static byte[] terminator = new byte[] { 0xff, 0xff, 0xff, 0xff };
		public MyReceiveFilter(): base(terminator) // two vertical bars as package terminator
		{
		}

		public  override StringPackageInfo ResolvePackage(IBufferStream bufferStream)
		{
			return null;
		}
		// other code you need implement according yoru protocol details
	}
}
