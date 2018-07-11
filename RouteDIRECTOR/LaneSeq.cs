using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RouteDirector;
namespace IRouteDirector
{
	public class LaneSeq
	{
		public List<Box> boxList = new List<Box> { };
		public Int16 lane = 0;
		public Int16 node;
 
		public LaneSeq(Int16 tLane)
		{
			lane = tLane;
		}

		public void AddBox(Box tBox)
		{
			boxList.Add(tBox);
		}

		public DivertCmd HanderReq(DivertReq divertReq)
		{
			int index;
			index = boxList.FindIndex(box => box.barcode == divertReq.codeStr);
			if (index == -1)
				return null;

			int number;
			number = boxList.FindIndex(box => box.status != Box.BoxStatus.Success);

			if (number == index)
			{
				boxList[index].status = Box.BoxStatus.Sorting;
				return new DivertCmd(divertReq, boxList[index].exLane);
			}
			else
			{
				Log.log.Debug("node:" + node+" lane:" + lane + "|next box: No." + number + " "+ boxList[number].barcode + "|reject box: NO." +index + " " + divertReq.codeStr);
			}
				return null;


		}


	}
}
