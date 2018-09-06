using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WHD.Core
{
    public class LogHelper
    {
        public static Logger logger = LogManager.GetLogger("WHD.Core");

        public static void Trace(string s)
        {
            logger.Trace(s);
        }
        public static void Debug(string s)
        {
            logger.Debug(s);
        }
        public static void Info(string s)
        {
            logger.Info(s);
        }
        public static void Warn(string s)
        {
            logger.Warn(s);
        }
        public static void Error(string s)
        {
            logger.Error(s);
        }
        public static void Fatal(string s)
        {
            logger.Fatal(s);
        }
        public static void Log(LogLevel level,string s)
        {
            logger.Log(level, s);
        }
    }
}
