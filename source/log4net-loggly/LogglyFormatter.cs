using ServiceStack.Text;
using log4net.Core;

namespace log4net.loggly
{
	public class LogglyFormatter : ILogglyFormatter
	{
		public virtual void AppendAdditionalLoggingInformation(ILogglyAppenderConfig config, LoggingEventData loggingEvent)
		{
		}

		public virtual string ToJson(LoggingEvent loggingEvent)
		{
			return new
			       	{
						level = loggingEvent.Level.DisplayName,
			       		message = loggingEvent.MessageObject, 
						ex = loggingEvent.ExceptionObject,
						timeStamp = loggingEvent.TimeStamp,
						threadName = loggingEvent.ThreadName,
			       	}.ToJson();
		}
	}
}