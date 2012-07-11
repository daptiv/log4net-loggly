using log4net.Core;

namespace log4net.loggly
{
	public interface ILogglyFormatter
	{
		void AppendAdditionalLoggingInformation(ILogglyAppenderConfig unknown, LoggingEventData loggingEvent);
		string ToJson(LoggingEvent loggingEvent);
	}
}