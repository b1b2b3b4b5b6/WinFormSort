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
		public int lane = 0;
 
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
			try
			{
				index = boxList.FindIndex((Box mBox) =>
				{
					if (mBox.barcode == divertReq.codeStr)
						return true;
					else
						return false;
				});
			}
			catch
			{
				return null;
			}

			int number;
			try
			{
				number = boxList.FindIndex((Box mBox) =>
				{
					if (mBox.status == Box.BoxStatus.Missing)
						return true;
					else
						return false;
				});
			}

			catch(Exception e)
			{
				Log.log.Error("find another same box or the previous box was not be sorting success", e);
				throw e;
			}

			if (number == index)
			{
				boxList[index].status = Box.BoxStatus.Sorting;
				return new DivertCmd(divertReq, boxList[index].exLane);
			}
			else
				return null;


		}


	}
}
