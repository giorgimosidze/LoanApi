using LoanApi.Models.UserModels;
using LoanApi.Services.UserModels;

namespace LoanApi.Services.UserService
{
	public interface IUserService
	{
		User Register(User user, string password);
		User Login(LoginModel model);
		User GetById(int id);
	}
}
