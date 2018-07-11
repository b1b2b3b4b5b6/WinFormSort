using RouteDirector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Threading;
using IRouteDirector;
namespace IRouteDirector
{
	static class main
	{
		static void Main(string[] args)
		{
			RouteDirectControl routeDirectControl = new RouteDirectControl();
			while (true)
			{
				int res = routeDirectControl.EstablishConnection();
				if (res != 0)
				{
					Log.log.Debug("EstablishConnection fail,try to reconnenct,wiat 10s");
					Thread.Sleep(10000);
				}
				else
					break;
			}

			while (true)
			{
				Thread.Sleep(20000);
				if (routeDirectControl.online == false)
				{
					Log.log.Debug("Connection  break,try to reconnenct");
					while (true)
					{
						int res = routeDirectControl.EstablishConnection();
						if (res != 0)
						{
							Log.log.Debug("EstablishConnection fail,try to reconnenct,wiat 10s");
							Thread.Sleep(10000);
						}
						else
							break;
					}
				}

			}
			StackSeq stackSeq = new StackSeq();
			List < string> strList = new List<string>
			{ "1203 1 1","1144 1 2","1165 1 3" };
			stackSeq.AddBoxList(strList);
			routeDirectControl.FlushMsg();
			while (true)
			{
				MessageBase msg;
				msg = routeDirectControl.WaitMsg();
				if (stackSeq.stackStatus == StackSeq.StackStatus.Busying)
				{
					if (msg.msgId == (Int16)MessageBase.MessageType.DivertReq)
					{
						DivertCmd divertCmd;
						divertCmd = stackSeq.HanderReq((DivertReq)msg);
						if (divertCmd == null)
							routeDirectControl.SendMsg(new HeartBeat(RouteDirectControl.heartBeatTime));
						else
							routeDirectControl.SendMsg(divertCmd);
					}

					if (msg.msgId == (Int16)MessageBase.MessageType.DivertRes)
					{
						stackSeq.HanderRes((DivertRes)msg);
					}
				}
			}

		}
	}

}
