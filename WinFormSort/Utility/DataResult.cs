using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormSort.Utility
{
   public class DataResult
    {
        public object DataModel { set; get; }
        public int ExeResult { set; get; }
        /// <summary>
        /// 1  B(a)类型消息
        /// 2  B(b)类型消息
        /// 3  S类型消息
        /// </summary>
        public int MsgType { get; set; }
    }
}
