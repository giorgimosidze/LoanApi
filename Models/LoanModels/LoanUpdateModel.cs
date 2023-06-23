using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoanApi.Models.LoanModels
{
	public class LoanUpdateModel
	{
		public LoanType LoanType { get; set; }
		public int Amount { get; set; }
		public string Currency { get; set; }
		public int Period { get; set; }
		public LoanStatus LoanStatus { get; set; }
	}
}
