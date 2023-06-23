using System;

namespace LoanApi.Models.UserModels
{
	public class UserModel
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Username { get; set; }
		public DateTime DateOfBirth { get; set; }
		public string PersonalNumber { get; set; }
	}
}
