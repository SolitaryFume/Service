using System.Reflection;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System.Diagnostics;

namespace Core
{   
    public static class Log
    {
        private static ILog _log;
        private static ILog loginfo
        {
            get
            {
                if (_log == null)
                {
                    _log = InitLog("serverlog");
                }
                return _log;
            }
        }

        private static ILog InitLog(string name)
        {
            if (LogManager.Exists(name) == null)
            {
                PatternLayout patternLayout = new PatternLayout();
                patternLayout.ConversionPattern = "%date %thread %level - %message %newline";
                patternLayout.ActivateOptions();

                RollingFileAppender appender = new RollingFileAppender();
                appender.Name = name;
                appender.AppendToFile = true;
                appender.File = $"Log\\{name}.log";
                appender.StaticLogFileName = true;
                appender.PreserveLogFileNameExtension = true;
                appender.LockingModel = new FileAppender.MinimalLock();
                appender.Layout = patternLayout;
                appender.MaxSizeRollBackups = 512;
                appender.MaximumFileSize = "100MB";
                appender.RollingStyle = RollingFileAppender.RollingMode.Composite;
                appender.ActivateOptions();

                Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();
                var loger = hierarchy.GetLogger(name, hierarchy.LoggerFactory); //!!! 此处写法是重点，不容更改
                loger.Hierarchy = hierarchy;
                loger.AddAppender(appender);
                loger.Level = Level.All;

                BasicConfigurator.Configure();//!!! 此处写法是重点，不容更改

                var appname = Assembly.GetEntryAssembly().GetName().Name;
                var version = Assembly.GetEntryAssembly().GetName().Version;
                Print("日志模块启动!");
            }

            var log = LogManager.GetLogger(name);
            return log;
        }

        [DebuggerHidden]
        public static void Print(object message)
        {
            if (loginfo == null)
            {
                System.Console.WriteLine("loginfo is null !");
            }
            loginfo.Debug(message);
        }

        public static void PrintFormat(string format, params object[] args)
        {
            loginfo.DebugFormat(format, args);
        }

        public static void Warning(object message)
        {
            loginfo.Warn(message);
        }
        public static void WarningFormat(string format, params object[] args)
        {
            loginfo.WarnFormat(format, args);
        }

        public static void Error(string message)
        {
            loginfo.Error(message);
        }

        public static void ErrorFormat(string format, params object[] args)
        {
            loginfo.ErrorFormat(format, args);
        }

        public static void Assert(bool condition)
        {
            if (!condition)
            {
                loginfo.Fatal("Assertion failed");
            }
        }

        public static void Assert(bool condition, object message)
        {
            if (!condition)
            {
                loginfo.Fatal(message);
            }
        }

        public static void AssertFormat(bool condition, string format, params object[] args)
        {
            if (!condition)
            {
                loginfo.FatalFormat(format, args);
            }
        }
    }
}