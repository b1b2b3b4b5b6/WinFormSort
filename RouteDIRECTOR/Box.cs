using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRouteDirector
{
	public class Box
	{
		public enum BoxStatus
		{
			Success = 0,
			WrongLine,
			Register,
			Missing,
			OutList,
			Sorting,
			Inital,
		}

		public string barcode = "";
		public Int16 exNode;
		public Int16 exLane;
		public BoxStatus status;
		public int showTimes;
		public Box()
		{
			barcode = "";
			exNode = 0;
			exLane = 0;
			status = BoxStatus.Inital;
			showTimes = 0;
		}
		public Box(Box box)
		{
			barcode = box.barcode;
			exNode = box.exNode;
			exLane = box.exLane;
			status = box.status;
			showTimes = box.showTimes;
		}
	};
}
