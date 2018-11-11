/*
Web.config
<configSections>
    <section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler,log4net" />
  </configSections>
 <log4net>
		<logger name="BlogLogger">
			<level value="INFO"/>
			<appender-ref ref="RollingLogFileAppender"/>
		</logger>
		<appender name="RollingLogFileAppender" type="log4net.Appender.RollingFileAppender">
			<file value="C:/logs/agathaApp.txt"/>
			<appendToFile value="true"/>
			<rollingStyle value="Size"/>
			<maxSizeRollBackups value="10"/>
			<maximumFileSize value="10MB"/>
			<staticLogFileName value="true"/>
			<layout type="log4net.Layout.PatternLayout">
				<conversionPattern value="%-5p %d %5rms %-22.22c{1} %-18.18M - %m%n"/>
			</layout>
		</appender>
	</log4net>
 */


using AstWeb.Common.Configuration;
using log4net;
using log4net.Config;
using System;

namespace AstWeb.Common.Logging
{
    public class Log4NetAdapter : ILogger
    {
        private readonly ILog _log;

        public Log4NetAdapter()
        {
            XmlConfigurator.Configure();
            var logName = WebConfigApplicationSettings.GetAppSettings("LoggerName");
            logName = logName == null ? "DefaultLog" : logName;
            _log = LogManager.GetLogger(logName);
        }
        public void Error(string message)
        {
            _log.Error(message);
        }
        public void Error(string message, Exception ex)
        {
            _log.Error(message, ex);
        }

        public void InfoFormat(string format, params object[] args)
        {
            _log.InfoFormat(format, args);
        }

        public void Debug(object message, Exception exception)
        {
            _log.Debug(message, exception);
        }

        public void Debug(object message)
        {
            _log.Debug(message);
        }

        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            _log.DebugFormat(provider, format, args);
        }

        public void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            _log.DebugFormat(format, arg0, arg1, arg2);
        }

        public void DebugFormat(string format, object arg0, object arg1)
        {
            _log.DebugFormat(format, arg0, arg1);
        }

        public void DebugFormat(string format, object arg0)
        {
            _log.DebugFormat(format, arg0);
        }

        public void DebugFormat(string format, params object[] args)
        {
            _log.DebugFormat(format, args);
        }

        public void Error(object message, Exception exception)
        {
            _log.Error(message, exception);
        }

        public void Error(object message)
        {
            _log.Error(message);
        }

        public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            _log.ErrorFormat(provider, format, args);
        }

        public void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            _log.ErrorFormat(format, arg0, arg1, arg2);
        }

        public void ErrorFormat(string format, object arg0, object arg1)
        {
            _log.ErrorFormat(format, arg0, arg1);
        }

        public void ErrorFormat(string format, object arg0)
        {
            _log.ErrorFormat(format, arg0);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            _log.ErrorFormat(format, args);
        }

        public void Fatal(object message, Exception exception)
        {
            _log.Fatal(message, exception);
        }

        public void Fatal(object message)
        {
            _log.Fatal(message);
        }

        public void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            _log.FatalFormat(provider, format, args);
        }

        public void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            _log.FatalFormat(format, arg0, arg1, arg2);
        }

        public void FatalFormat(string format, object arg0, object arg1)
        {
            _log.FatalFormat(format, arg0, arg1);
        }

        public void FatalFormat(string format, object arg0)
        {
            _log.FatalFormat(format, arg0);
        }

        public void FatalFormat(string format, params object[] args)
        {
            _log.FatalFormat(format, args);
        }

        public void Info(object message, Exception exception)
        {
            _log.Info(message, exception);
        }

        public void Info(object message)
        {
            _log.Info(message);
        }

        public void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            _log.InfoFormat(provider, format, args);
        }

        public void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            _log.InfoFormat(format, arg0, arg1, arg2);
        }

        public void InfoFormat(string format, object arg0, object arg1)
        {
            _log.InfoFormat(format, arg0, arg1);
        }

        public void InfoFormat(string format, object arg0)
        {
            _log.InfoFormat(format, arg0);
        }

        public bool IsDebugEnabled
        {
            get { return _log.IsDebugEnabled; }
        }

        public bool IsErrorEnabled
        {
            get { return _log.IsErrorEnabled; }
        }

        public bool IsFatalEnabled
        {
            get { return _log.IsFatalEnabled; }
        }

        public bool IsInfoEnabled
        {
            get { return _log.IsInfoEnabled; }
        }

        public bool IsWarnEnabled
        {
            get { return _log.IsWarnEnabled; }
        }

        public void Warn(object message, Exception exception)
        {
            _log.Warn(message, exception);
        }

        public void Warn(object message)
        {
            _log.Warn(message);
        }

        public void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            _log.WarnFormat(provider, format, args);
        }

        public void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            _log.WarnFormat(format, arg0, arg1, arg2);
        }

        public void WarnFormat(string format, object arg0, object arg1)
        {
            _log.WarnFormat(format, arg0, arg1);
        }

        public void WarnFormat(string format, object arg0)
        {
            _log.WarnFormat(format, arg0);
        }

        public void WarnFormat(string format, params object[] args)
        {
            _log.WarnFormat(format, args);
        }
    }
}
