using System;
using System.ComponentModel.DataAnnotations;

namespace LoanApi.Models.UserModels
{
	public class RegisterModel
	{
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
		public string Password { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string PersonalNumber { get; set; }

    }
}
