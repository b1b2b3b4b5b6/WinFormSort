using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RouteDirector.Utility;
namespace RouteDirector.PacketProcess
{
	class DivertReq : MessageBase
	{
		static private  Int16 messageId = (Int16)MessageType.DivertReq; 
		static private Int16 codeStrLen = 10;
		public Int16 nodeId;
		public Int16 cartSeq;
		public Int32 attribute;
		public string codeStr;
		static public int len = 20;

		/// <summary>
		/// 解析消息数组至单个消息对象
		/// </summary>
		/// <param name="buf">消息数组</param>
		/// <param name="offset">数组偏移量</param>
		public DivertReq(byte[] buf, int offset) : base(messageId)
		{
			offset += 2;
			offset += DataConversion.ByteToNum(buf, offset, ref nodeId, false);
			offset += DataConversion.ByteToNum(buf, offset, ref cartSeq, false);
			offset += DataConversion.ByteToNum(buf, offset, ref attribute, false);
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
			str.AppendLine("attribute = " + attribute.ToString());
			str.AppendLine("codeStr = " + codeStr);
			str.AppendLine();
			return str;
		}
	}
}
