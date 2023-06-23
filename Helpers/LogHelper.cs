using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NLog;

namespace LoanApi.Helpers
{
	public class LogHelper
	{
		private static Logger InfoLogger = LogManager.GetLogger("info");
		private static Logger ErrorLogger = LogManager.GetLogger("error");

		public static void AuditLog(string message) {
			InfoLogger.Info(message);
		}
		public static void ErrorLog(string message)
		{
			ErrorLogger.Error(message);
		}
	}
}
