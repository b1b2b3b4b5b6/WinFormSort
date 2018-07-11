using System;
using System.Collections.Generic;
using RouteDirector;
using static IRouteDirector.Box;

namespace IRouteDirector
{
	public class StackSeq : ISorting
	{
		public enum StackStatus
		{
			Success = 0,
			LosingBox,
			SortFalut,
			RobotFalut,
			Busying,
			Inital,
		}

		static public List<Box> boxList = new List<Box> { };
		static public List<NodeSeq> nodeSeqList = new List<NodeSeq> { };
		public StackStatus stackStatus = StackStatus.Inital;

		public int AddBoxList(List<string> tBoxList)
		{
			Reset();
			boxList.Clear();
			nodeSeqList.Clear();
			foreach (string str in tBoxList)
			{
				string[] sArray = str.Split(' ');
				Box box = new Box();
				box.barcode = sArray[0];
				box.exNode = Convert.ToInt16(sArray[1]);
				box.exLane = Convert.ToInt16(sArray[2]);
				box.status = BoxStatus.Missing;
				boxList.Add(box);

				int index;
				try
				{
					index = nodeSeqList.FindIndex((NodeSeq mNodeSeq) =>
					{
						if (mNodeSeq.node == box.exNode)
							return true;
						else
							return false;
					});
					nodeSeqList[index].AddBox(box);
				}
				catch
				{
					NodeSeq nodeSeq = new NodeSeq(box.exNode);
					nodeSeqList.Add(nodeSeq);
					nodeSeq.AddBox(box);
				}
			}
			
			stackStatus = StackStatus.Busying;
			return boxList.Count;
		}

		public void Reset()
		{
			boxList.Clear();
			nodeSeqList.Clear();
			stackStatus = StackStatus.Inital;
		}

		public DivertCmd HanderReq(DivertReq divertReq)
		{
			int index = LocationBox(divertReq.codeStr, true);
			if (index < 0)
				return null;
			foreach (NodeSeq nodeSeq in nodeSeqList)
			{
				if (nodeSeq.node == divertReq.nodeId)
					return nodeSeq.HanderReq(divertReq);
			}
			return  null;
		}

		public void HanderRes(DivertRes divertRes)
		{
			int index = LocationBox(divertRes.codeStr, false);
			if (index >= 0)
			{
				Box box = boxList[index];
				if (box.status == BoxStatus.Sorting)
				{
					if ((box.exNode == divertRes.nodeId) && (box.exLane == divertRes.laneId))
					{
						box.status = BoxStatus.Success;
						Log.log.Debug("box: " + box.barcode + " sort success");
						CheckStatus();
					}	
					else
					{
						Log.log.Error("box with barcode: " + box.barcode + " sorting falut: " +  divertRes.divertRes);
						stackStatus = StackStatus.SortFalut;
						throw new Exception();
					}
				}
			}
		}


		private int LocationBox(string barcode, bool isCounting)
		{
			int index = -2;
			if (barcode.Contains("?"))
			{
				Log.log.Debug("find unknow box");
				return -1;
			}

			index = boxList.FindIndex(box => box.barcode.Equals(barcode));

			if (index == -1)
			{
				Log.log.Debug("find box out of list with barcode: " + barcode);
				return -1;
			}

			if (boxList[index].status == BoxStatus.Success)
			{
				Log.log.Error("find same box or sorting falut happen");
				throw new Exception();
			}

			if (boxList[index].status == BoxStatus.Missing)
			{
				boxList[index].status = BoxStatus.Register;
				Log.log.Debug("register box: " + boxList[index].barcode);
			}

			if (isCounting)
				boxList[index].showTimes = boxList[index].showTimes + 1;
			return index;
		}

		private void CheckStatus()
		{
			foreach (Box box in boxList)
			{
				if (box.status != BoxStatus.Success)
					return;
			}
			Log.log.Debug("stack sort success");
			stackStatus = StackStatus.Success;
		}
	}
}
