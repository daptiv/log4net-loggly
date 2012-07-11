using log4net.Appender;
using log4net.Core;

namespace log4net.loggly
{
	public class LogglyAppender : BufferingAppenderSkeleton, ILogglyAppenderConfig
	{
		public static ISendToLoggley Template = new SendToLoggley();

		private string _rootUrl;
		public string RootUrl
		{
			get { return _rootUrl; }
			set
			{
				//TODO: validate http and uri
				_rootUrl = value;
				if (!_rootUrl.EndsWith("/"))
				{
					_rootUrl += "/";
				}
			}
		}

		public string DefaultInputKey { get; set; }

		public string UserAgent { get; set; }

		public int TimeoutInSeconds { get; set; }

		public LogglyAppender()
		{
			UserAgent = "loggly-log4net-appender";
			TimeoutInSeconds = 30;
		}

		protected override void SendBuffer(LoggingEvent[] events)
		{
			Template.SendBuffer(this, events);
		}

		protected override void Append(LoggingEvent loggingEvent)
		{
			Template.AppendAdditionalLoggingInformation(this, loggingEvent);
			base.Append(loggingEvent);
		}
	}
}
