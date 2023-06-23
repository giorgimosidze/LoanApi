namespace LoanApi.Models.LoanModels
{
	public class LoanModel
	{
		public LoanType LoanType { get; set; }
		public int Amount { get; set; }
		public string Currency { get; set; }
		public int Period { get; set; }
		public LoanStatus LoanStatus { get; set; }
		public int UserID { get; set; }
	}
}
