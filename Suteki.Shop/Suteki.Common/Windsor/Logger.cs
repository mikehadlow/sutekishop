using System;
using Castle.Core.Logging;

namespace Suteki.Common.Windsor
{
    public class Logger : ILogger   
    {
        public void Debug(string message)
        {
            // TODO
        }

        public void Debug(string format, params object[] args)
        {
            // TODO
        }

        public void DebugFormat(string format, params object[] args)
        {
            // TODO
        }

        public void Info(string message)
        {
            // TODO
        }

        public void Info(string format, params object[] args)
        {
            // TODO
        }

        public void InfoFormat(string format, params object[] args)
        {
            // TODO
        }

        public void Warn(string message)
        {
            // TODO
        }

        public void Warn(string format, params object[] args)
        {
            // TODO
        }

        public void WarnFormat(string format, params object[] args)
        {
            // TODO
        }

        public void Error(string message)
        {
            // TODO
        }

        public void Error(string format, params object[] args)
        {
            // TODO
        }

        public void ErrorFormat(string format, params object[] args)
        {
            // TODO
        }

        public void Fatal(string message)
        {
            // TODO
        }

        public void Fatal(string format, params object[] args)
        {
            // TODO
        }

        public void FatalFormat(string format, params object[] args)
        {
            // TODO
        }

        public void FatalError(string message)
        {
            // TODO
        }

        public void FatalError(string format, params object[] args)
        {
            // TODO
        }

        public ILogger CreateChildLogger(string loggerName)
        {
            return this;
        }

        public bool IsDebugEnabled
        {
            get { return false; }
        }

        public bool IsInfoEnabled
        {
            get { return false; }
        }

        public bool IsWarnEnabled
        {
            get { return false; }
        }

        public bool IsErrorEnabled
        {
            get { return false; }
        }

        public bool IsFatalEnabled
        {
            get { return false; }
        }

        public bool IsFatalErrorEnabled
        {
            get { return false; }
        }

        public void FatalError(string message, Exception exception)
        {
            // TODO
        }

        public void FatalFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
        {
            // TODO
        }

        public void FatalFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            // TODO
        }

        public void FatalFormat(Exception exception, string format, params object[] args)
        {
            // TODO
        }

        public void Fatal(string message, Exception exception)
        {
            // TODO
        }

        public void ErrorFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
        {
            // TODO
        }

        public void ErrorFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            // TODO
        }

        public void ErrorFormat(Exception exception, string format, params object[] args)
        {
            // TODO
        }

        public void Error(string message, Exception exception)
        {
            // TODO
        }

        public void WarnFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
        {
            // TODO
        }

        public void WarnFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            // TODO
        }

        public void WarnFormat(Exception exception, string format, params object[] args)
        {
            // TODO
        }

        public void Warn(string message, Exception exception)
        {
            // TODO
        }

        public void InfoFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
        {
            // TODO
        }

        public void InfoFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            // TODO
        }

        public void InfoFormat(Exception exception, string format, params object[] args)
        {
            // TODO
        }

        public void Info(string message, Exception exception)
        {
            // TODO
        }

        public void DebugFormat(Exception exception, IFormatProvider formatProvider, string format, params object[] args)
        {
            // TODO
        }

        public void DebugFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            // TODO
        }

        public void DebugFormat(Exception exception, string format, params object[] args)
        {
            // TODO
        }

        public void Debug(string message, Exception exception)
        {
            // TODO
        }
    }
}