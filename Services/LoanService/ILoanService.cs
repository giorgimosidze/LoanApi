using LoanApi.Models.LoanModels;
using System.Collections.Generic;

namespace LoanApi.Services.LoanService
{
	public interface ILoanService
	{
		Loan RequestLoan(Loan loan);
		IEnumerable<Loan> GetUserLoans(int userid);
		void UpdateLoan(int loanId,int userId,Loan loan);
	}
}
