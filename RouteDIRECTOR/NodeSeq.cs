using RouteDirector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRouteDirector
{
	public class NodeSeq
	{
		public Int16 node;
		public List<LaneSeq> laneSeqList = new List<LaneSeq> { };
		public NodeSeq(Int16 tNode)
		{
			node = tNode;
		}

		public NodeSeq(NodeSeq tNodeSeq)
		{
			node = tNodeSeq.node;
			laneSeqList = tNodeSeq.laneSeqList;
		}
		public void AddBox(Box tBox)
		{
			int index;
			try
			{
				index = laneSeqList.FindIndex((LaneSeq mLaneSeq) =>
				{
					if (mLaneSeq.lane == tBox.exLane)
						return true;
					else
						return false;
				});
				laneSeqList[index].AddBox(tBox);
			}
			catch
			{
				LaneSeq laneSeq = new LaneSeq(tBox.exLane);
				laneSeqList.Add(laneSeq);
				laneSeq.AddBox(tBox);
			}
		}

		public DivertCmd HanderReq(DivertReq divertReq)
		{
			foreach (LaneSeq laneSeq in laneSeqList)
			{
				DivertCmd divertCmd = laneSeq.HanderReq(divertReq);
				if (divertCmd != null)
					return divertCmd;
				
			}
			return null;
		}

	}
}
