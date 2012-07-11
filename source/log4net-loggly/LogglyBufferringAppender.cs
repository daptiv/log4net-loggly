using log4net.Appender;
using log4net.Core;

namespace log4net.loggly
{
	public class LogglyBufferringAppender : BufferingAppenderSkeleton
	{
		public static readonly string InputKeyProperty = "LogglyInputKey";

		public static ILogglyFormatter Formatter = new LogglyFormatter();
		public static ILogglyClient Client = new LogglyClient();

		private ILogglyAppenderConfig Config = new LogglyAppenderConfig();

		public string RootUrl { set { Config.RootUrl = value; } }
		public string DefaultInputKey { set { Config.DefaultInputKey = value; } }
		public string UserAgent { set { Config.UserAgent = value; } }
		public int TimeoutInSeconds { set { Config.TimeoutInSeconds = value; } }

		protected override void Append(LoggingEvent loggingEvent)
		{
			var data = loggingEvent.GetLoggingEventData(FixFlags.All);
			Formatter.AppendAdditionalLoggingInformation(Config, data);
			if (!loggingEvent.Properties.Contains(InputKeyProperty))
			{
				loggingEvent.Properties[InputKeyProperty] = Config.DefaultInputKey;
			}
			base.Append(loggingEvent);
		}

		protected override void SendBuffer(LoggingEvent[] loggingEvents)
		{
			foreach (var loggingEvent in loggingEvents)
			{
				Client.Send(Config, (string)loggingEvent.Properties[InputKeyProperty], Formatter.ToJson(loggingEvent));
			}
		}
	}
}