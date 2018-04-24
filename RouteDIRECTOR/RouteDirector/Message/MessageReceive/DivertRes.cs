using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RouteDirector.Utility;
namespace RouteDirector.PacketProcess
{
	class DivertRes : MessageBase
	{
		private const Int16 codeStrLen = 22;

		private const Int16 messageId = (Int16)MessageBase.MessageType.DivertRes;
		public Int16 nodeId;
		public Int16 cartSeq;
		public Int16 laneId;
		public Int16 divertRes;
		public string codeStr;
		static public int len = 32;

		/// <summary>
		/// 解析消息数组至单个消息对象
		/// </summary>
		/// <param name="buf">消息数组</param>
		/// <param name="offset">数组偏移量</param>
		public DivertRes(byte[] buf, int offset) : base(messageId)
		{
			base.msgBuf = new byte[len];
			Array.Copy(buf, 0, base.msgBuf, 0, len);
			offset += 2;
			offset += DataConversion.ByteToNum(buf, offset, ref nodeId, false);
			offset += DataConversion.ByteToNum(buf, offset, ref cartSeq, false);
			offset += DataConversion.ByteToNum(buf, offset, ref laneId, false);
			offset += DataConversion.ByteToNum(buf, offset, ref divertRes, false);
			codeStr = Encoding.ASCII.GetString(buf, offset, codeStrLen);
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
			str.AppendLine("cartSeq = " + cartSeq.ToString());
			str.AppendLine("laneId = " + laneId.ToString());
			str.AppendLine("divertRes = " + divertRes.ToString());
			str.AppendLine("codeStr = " + codeStr);
			str.AppendLine();
			return str;
		}

	}
}
