using AutoMapper;
using LoanApi.Models.LoanModels;
using LoanApi.Models.UserModels;

namespace LoanApi.Helpers
{
	public class AutoMapper : Profile
	{
		public AutoMapper()
		{
			
			CreateMap<Loan, LoanModel>();
			CreateMap<LoanModel, Loan>();
			CreateMap<LoanUpdateModel, Loan>();

			CreateMap<RegisterModel, User>();
		}
	}
}
