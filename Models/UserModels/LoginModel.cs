using System.ComponentModel.DataAnnotations;

namespace LoanApi.Services.UserModels
{
	public class LoginModel
	{
		[Required]
		public string Username { get; set; }

		[Required]
		public string Password { get; set; }
	}
}
