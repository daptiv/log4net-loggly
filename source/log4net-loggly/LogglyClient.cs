using System;
using System.Net;
using System.Text;

namespace log4net.loggly
{
	public class LogglyClient : ILogglyClient
	{
		public virtual void Send(ILogglyAppenderConfig config, string inputKey, string message)
		{
			var bytes = Encoding.UTF8.GetBytes(message);
			var request = CreateWebRequest(config, string.IsNullOrWhiteSpace(inputKey) ? config.InputKey : inputKey);
			using (var dataStream = request.GetRequestStream())
			{
				dataStream.Write(bytes, 0, bytes.Length);
				dataStream.Flush();
				dataStream.Close();
			}
			var response = request.GetResponse();
			response.Close();
		}

		protected virtual HttpWebRequest CreateWebRequest(ILogglyAppenderConfig config, string inputKey)
		{
			var url = String.Concat(config.RootUrl, inputKey);
			var request = (HttpWebRequest)WebRequest.Create(url);
			request.Method = "POST";
			request.ReadWriteTimeout = request.Timeout = config.TimeoutInSeconds * 1000;
			request.UserAgent = config.UserAgent;
			request.KeepAlive = true;
			request.ContentType = "application/json";
			return request;
		}
	}
}