using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using ServiceStack.Text;
using log4net.Core;

namespace log4net.loggly
{
	public class SendToLoggley : ISendToLoggley
	{
		public static readonly string InputKeyProperty = "LogglyInputKey";

		public virtual void AppendAdditionalLoggingInformation(ILogglyAppenderConfig config, LoggingEvent loggingEvent)
		{
			if (!loggingEvent.Properties.Contains(InputKeyProperty))
			{
				loggingEvent.Properties[InputKeyProperty] = config.DefaultInputKey;
			}
		}

		public virtual void SendBuffer(ILogglyAppenderConfig config, LoggingEvent[] events)
		{
			var eventsByInput = events.ToDictionaryOfLists(e => e.Properties[InputKeyProperty]);
			foreach (var events4Input in eventsByInput)
			{
				foreach (var @event in events4Input.Value)
				{
					var json = ToJson(@event);
					var bytes = Encoding.UTF8.GetBytes(json);	
					var request = CreateWebRequest(config, events4Input);
					using (var dataStream = request.GetRequestStream())
					{
						dataStream.Write(bytes, 0, bytes.Length);
						dataStream.Flush();
						dataStream.Close();
					}
					var response = request.GetResponse();
					response.Close();
				}
			}
		}

		private static HttpWebRequest CreateWebRequest(ILogglyAppenderConfig config, KeyValuePair<object, List<LoggingEvent>> events4Input)
		{
			var inputKey = events4Input.Key;
			var url = String.Concat(config.RootUrl, "inputs/", (string) inputKey);
			var request = (HttpWebRequest) WebRequest.Create(url);
			request.Method = "POST";
			request.ReadWriteTimeout = request.Timeout = config.TimeoutInSeconds*1000;
			request.UserAgent = config.UserAgent;
			request.KeepAlive = true;
			request.ContentType = "application/json";
			return request;
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