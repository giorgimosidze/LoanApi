using System;
using LoanApi.Models.LoanModels;
using System.Collections.Generic;

namespace LoanApi.Models.UserModels
{
	public class User
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Username { get; set; }
		public DateTime DateOfBirth { get; set; }
		public string  PersonalNumber { get; set; }
		public byte[] PasswordHash { get; set; }
		public byte[] PasswordSalt { get; set; }
		public ICollection<Loan> Loans { get; set; }
	}
}
