namespace LoanApi.Models.LoanModels
{
	public enum LoanType
	{
		FastLoan = 1,
		AutoLoan = 2,
		Installment = 3

	}
	public enum LoanStatus
	{
		Forwarded = 0,
		Processing = 1,
		Accepted = 2,
		Rejected = 3
	}
}
