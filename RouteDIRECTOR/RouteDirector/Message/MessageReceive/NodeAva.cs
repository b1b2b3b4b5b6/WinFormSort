﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RouteDirector.Utility;
namespace RouteDirector.PacketProcess
{
	class NodeAva : MessageBase
	{
		static private Int16 messageId = (Int16)MessageBase.MessageType.NodeAva;
		public Int16 nodeId;
		static public int len = 4;

		/// <summary>
		/// 解析消息数组至单个消息对象
		/// </summary>
		/// <param name="buf">消息数组</param>
		/// <param name="offset">数组偏移量</param>
		public NodeAva(byte[] buf, int offset) : base(messageId)
		{
			offset += 2;
			offset += DataConversion.ByteToNum(buf, offset, ref nodeId, false);
		}

		/// <summary>
		/// 打印消息的参数信息
		/// </summary>
		/// <param name="str">StringBuilder引用</param>
		/// <returns>StringBuilder引用</returns>
		public override StringBuilder GetInfo(StringBuilder str)
		{
			str = base.GetInfo(str);
			str.AppendLine("nodeId = " + nodeId.ToString());
			str.AppendLine();
			return str;
		}
	}
}