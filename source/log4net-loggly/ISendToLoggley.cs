using log4net.Core;

namespace log4net.loggly
{
	public interface ISendToLoggley
	{
		void AppendAdditionalLoggingInformation(ILogglyAppenderConfig unknown, LoggingEvent loggingEvent);
		void SendBuffer(ILogglyAppenderConfig logglyAppender, LoggingEvent[] events);
	}
}