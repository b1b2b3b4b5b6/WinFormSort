using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormSort.Utility
{
    public class CommonMsgBuffer
    {
        public static short CycleNumber = 0;
        protected int writeIndex = 0;
        protected int ReadIndex = 0;
        public static byte[] _byteBuffer=new byte[240] ;
        public static string CacheHexStr = "";
        public static DateTime AliveTime;

        /// <summary>
        /// 从字节数组中截取需要的片段
        /// </summary>
        /// <param name="startIndex">开始截取的下标</param>
        /// <param name="len">长度</param>
        /// <param name="buffer">截取的结果</param>
        /// <returns></returns>
        public  byte[] Read(byte[] buffer, int len)
        {
            byte[] data = new byte[len];
            Array.Copy(buffer, ReadIndex, data, 0, len);
            ReadIndex = ReadIndex + len;
            return data;
        }
        public short ReadShort(byte [] data)
        {
            return BitConverter.ToInt16(Read(data,2), 0);
        }

        public int ReadInt(byte[] data)
        {
            return BitConverter.ToInt32(Read(data,4), 0);
        }
        /// <summary>
        /// 写入字节数组
        /// </summary>
        /// <param name="data">数据源</param>
        /// <param name="len">写入长度</param>
        /// <returns></returns>
        protected byte[] WriteByte(byte[] data, int len)
        {
            Array.Copy(data, 0, _byteBuffer, writeIndex, len);
            writeIndex = writeIndex + len;
            return _byteBuffer;
        }

        protected void Clear()
        {
            writeIndex = 0;
            ReadIndex = 0;
        }
        protected void SetBufLen(int MaxByteLenth)
        {
            _byteBuffer = new byte[MaxByteLenth];
            Array.Clear(_byteBuffer, 0, _byteBuffer.Length);
        }
    }
}
