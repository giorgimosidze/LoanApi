using NLog.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LoanApi
{
	public class Program
	{
		public static void Main(string[] args)
		{
			//var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
			//logger.Debug("init main");

			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				}).ConfigureLogging(logging =>
				{
					logging.ClearProviders();
					logging.SetMinimumLevel(LogLevel.Trace);
				})
				 .UseNLog();
	}
}
