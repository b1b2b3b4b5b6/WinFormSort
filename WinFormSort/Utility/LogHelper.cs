using System;
using log4net;
using log4net.Config;
using System.IO;

namespace WinFormSort.Utility
{
    /// <summary>
    /// 日志类别
    /// </summary>
    public  enum Level
    {
        /// <summary>
        /// 致命错误
        /// </summary>
        FATAL,
        /// <summary>
        /// 一般错误
        /// </summary>
        ERROR,
        /// <summary>
        /// 警告
        /// </summary>
        WARN,
        /// <summary>
        /// 一般信息
        /// </summary>
        INFO,
        /// <summary>
        /// 调试信息
        /// </summary>
        DEBUG
    }

    /// <summary>
    /// 日志记录辅助类
    /// zhuaiguo
    /// </summary>
    public static class LogHelper
    {
        /// <summary>
        /// 日志记录
        /// </summary>
        /// <param name="t">类型对象</param>
        /// <param name="msg">消息</param>
        /// <param name="level">日志级别</param>
        public static void WriteLog4(string msg, Level level = Level.INFO)
        { 
            ILog log = LogManager.GetLogger("Manual.Logging");
            switch (level)
            {
                case Level.FATAL:
                    log.Fatal(msg);
                    break;
                case Level.ERROR:
                    log.Error(msg);
                    break;
                case Level.WARN:
                    log.Warn(msg);
                    break;
                case Level.INFO:
                    log.Info(msg);
                    break;
                case Level.DEBUG:
                    log.Debug(msg);
                    break;
                default:
                    log.Info(msg);
                    break;
            }
        }

        /// <summary>
        /// 记录错误日志
        /// zhuaiguo
        /// </summary>
        /// <param name="t">类型对象</param>
        /// <param name="msg">消息</param>
        /// <param name="ex">异常消息</param>
        public static void WriteLog4Ex(string msg, Exception ex)
        {
            ILog log = LogManager.GetLogger("Manual.Logging");
            log.Error(msg, ex);
        }
    }
}
