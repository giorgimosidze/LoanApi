using System.ComponentModel.DataAnnotations.Schema;

namespace LoanApi.Models.LoanModels
{
	public class Loan
	{
		public int LoanID { get; set; }
		public LoanType LoanType { get; set; }
		public int Amount { get; set; }
		public string Currency { get; set; }
		public int Period { get; set; }
		public LoanStatus LoanStatus { get; set; }
		[ForeignKey("User")]
		public int UserID { get; set; }

	}
}
