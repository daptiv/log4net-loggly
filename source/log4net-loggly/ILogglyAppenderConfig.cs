namespace log4net.loggly
{
	public interface ILogglyAppenderConfig
	{
		string RootUrl { get; set; }
		string DefaultInputKey { get; set; }
		string UserAgent { get; set; }
		int TimeoutInSeconds { get; set; }
	}
}