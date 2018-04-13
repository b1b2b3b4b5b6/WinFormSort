using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormSort.RecivePacket;
namespace WinFormSort.Utility
{
    public class DataConversion
    {
        /// <summary>
        /// 字节数组转16进制字符串，设定起始位置
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <param name="startIndx">起始位</param>
        /// <param name="length">长度</param>
        /// <returns></returns>
        public static string byteToHexStr(byte[] bytes, int startIndx, int length)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = startIndx; i < startIndx + length; i++)
                {
                    returnStr += bytes[i].ToString("X2") + " ";
                    //"0x"+bytes[i].ToString("X2")+" ";
                }
            }
            return returnStr;
        }

        /// <summary>
        /// 字节数组转为十六进制字符串
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ByteArrayToHexString(byte[] data)
        {
            try
            {
                StringBuilder sb = new StringBuilder(data.Length * 3);
                foreach (byte b in data)
                    sb.Append(Convert.ToString(b, 16).PadLeft(2, '0') + " ");
                return sb.ToString().ToUpper();
            }
            catch (Exception)
            {

                return null;
            }

        }
        /// <summary>
        /// 16进制字符串转字节数组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] StringToHexArray(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        /// <summary>
        /// 将一个字节数组截取需要的片段
        /// </summary>
        /// <param name="Str">需要截取的数组</param>
        /// <param name="StartIndex">截取的起始位置</param>
        /// <param name="Length">截取的长度</param>
        /// <returns></returns>
        public static byte[] CutByteArray(byte[] Str, int StartIndex, int Length)
        {
            byte[] DataFin = new byte[Length];
            for (int i = StartIndex, j = 0; i < StartIndex + Length; i++, j++)
            {
                DataFin[j] = Str[i];
            }
            return DataFin;
        }

        /// <summary>
        /// 字节数组转10进制字符串，规定起始位置和长度，主要用于取地址
        /// 限制条件：十进制数不能大于100，否则会出现错误
        /// </summary>
        /// <param name="myByte">要处理的字节数组</param>
        /// <param name="startIndex">起始位</param>
        /// <param name="length">长度</param>
        /// <returns></returns>
        public static string ByteToStr(byte[] myByte, int startIndex, int length)
        {
            string newStr = null;
            for (int i = startIndex; i < startIndex + length; i++)
            {
                if (myByte[i] < 10)
                {
                    newStr += "0" + myByte[i];//十进制数小于10的前面补零
                }
                else
                {
                    newStr += myByte[i];
                }

            }
            return newStr;
        }
        public static string ReceiveData { get; set; }


        
    }
}
