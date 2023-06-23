using System;
using System.Linq;
using LoanApi.Helpers;
using LoanApi.Models.UserModels;
using LoanApi.Services.UserModels;

namespace LoanApi.Services.UserService
{
	public class UserService : IUserService
	{
		private DataBaseContext _context;
		public UserService(DataBaseContext context)
		{
			_context = context;
		}
		public User Register(User user, string password)
		{

			if (string.IsNullOrEmpty(password))
				throw new CustomException("Password is required");

			if (_context.Users.Any(x => x.Username == user.Username))
				throw new CustomException($"Username { user.Username} is already registered");

			if (user.PersonalNumber.Length != 11)
				throw new CustomException($"Length of Personal Number must be 11");


			byte[] passwordHash, passwordSalt;
			CreatePasswordHash(password, out passwordHash, out passwordSalt);

			user.PasswordHash = passwordHash;
			user.PasswordSalt = passwordSalt;

			_context.Users.Add(user);
			_context.SaveChanges();

			return user;
		}
		public User Login(LoginModel loginModel)
		{
			if (string.IsNullOrEmpty(loginModel.Username) || string.IsNullOrEmpty(loginModel.Password))
				return null;

			var user = _context.Users.SingleOrDefault(x => x.Username == loginModel.Username);

			if (user == null)
				return null;

			if (!VerifyPasswordHash(loginModel.Password, user.PasswordHash, user.PasswordSalt))
				return null;

			return user;
		}
		public User GetById(int id)
		{
			return _context.Users.Find(id);
		}

		private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
		{
			if (password == null) throw new ArgumentNullException("password");
			if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

			using (var hmac = new System.Security.Cryptography.HMACSHA512())
			{
				passwordSalt = hmac.Key;
				passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
			}
		}

		private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
		{
			if (password == null) throw new ArgumentNullException("password");
			if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
			if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
			if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

			using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
			{
				var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
				for (int i = 0; i < computedHash.Length; i++)
				{
					if (computedHash[i] != storedHash[i]) return false;
				}
			}

			return true;
		}
	}
}
