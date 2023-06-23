using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoanApi.Helpers;
using LoanApi.Models.LoanModels;

namespace LoanApi.Services.LoanService
{
	public class LoanService : ILoanService
	{
		private DataBaseContext _context;
		public LoanService(DataBaseContext context)
		{
			_context = context;
		}

		public Loan RequestLoan(Loan loan)
		{
			_context.Loans.Add(loan);
			_context.SaveChanges();
			return loan;
		}

		public IEnumerable<Loan> GetUserLoans(int userid)
		{
			return _context.Loans.Where(X => X.UserID == userid);
		}

		public void UpdateLoan(int loanId, int userId, Loan loan)
		{
			var loanToUpdate = _context.Loans.Find(loanId);

			if (loanToUpdate == null)
				throw new CustomException("Loan Not Found");
			if (loanToUpdate.UserID != userId)
				throw new CustomException("You don't permission to modify this loan");
			else if (loanToUpdate.LoanStatus == LoanStatus.Rejected)
				throw new CustomException($"You can't update loan status is : {LoanStatus.Rejected}");
			else if (loanToUpdate.LoanStatus == LoanStatus.Accepted)
				throw new CustomException($"You can't update loan status is : {LoanStatus.Accepted}");
			else
			{
				loanToUpdate.LoanStatus = loan.LoanStatus;
				loanToUpdate.Amount = loan.Amount;
				loanToUpdate.LoanType = loan.LoanType;
				loanToUpdate.Period = loan.Period;
				loanToUpdate.Currency = loan.Currency;
			}
			_context.Loans.Update(loanToUpdate);
			_context.SaveChanges();
		}
	}
}
