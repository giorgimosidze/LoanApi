﻿using System;

namespace LoanApi.Helpers
{
	public class CustomException:Exception
	{
		public CustomException(string message) : base(message) { }
	}
}
