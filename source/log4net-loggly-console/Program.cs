using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using log4net;

namespace log4net_loggly_console
{
	class Program
	{
		static void Main(string[] argArray)
		{
			log4net.Config.XmlConfigurator.Configure();
			
			var log = LogManager.GetLogger(typeof (Program));

			var args = ParseArgs(argArray);
			for (int i = 0; i < args.Repeat; i++)
			{
				try
				{
					log.Error("oops", new ArgumentOutOfRangeException("argArray"));
					log.Warn("hmmm", new ApplicationException("app exception"));
					log.Info("yawn");
					log.Debug("zzzz");
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
				}
			}

			Thread.Sleep(10000);
		}

		private static Args ParseArgs(string[] argArray)
		{
			var args = new Args{Repeat = 1};
			Action<string> nextArg = null;
			foreach (var a in argArray)
			{
				if (nextArg != null)
				{
					nextArg(a);
					nextArg = null;
					continue;
				}
				if (a.StartsWith("-") || a.StartsWith("/"))
				{
					switch (a.Substring(1))
					{
						case "r":
							nextArg = s => args.Repeat = int.Parse(s);
							break;
						default:
							throw new ArgumentOutOfRangeException("unknown arg: " + a);
					}
				}
			}
			return args;
		}

		public class Args
		{
			public int Repeat { get; set; }
		}
	}
}
