using AutoMapper;
using System.Linq;
using LoanApi.Helpers;
using Microsoft.AspNetCore.Mvc;
using LoanApi.Models.LoanModels;
using System.Collections.Generic;
using LoanApi.Services.LoanService;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace LoanApi.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class LoanController : ControllerBase
	{
	
		private ILoanService _loanService;
		private IMapper _mapper;
		private readonly ILogger<LoanController> _logger;
		public LoanController(
		ILoanService loanService,
		IMapper mapper, 
		ILogger<LoanController> logger)

		{
			_loanService = loanService;
			_mapper = mapper;
			_logger = logger;
			_logger.LogDebug(1, "NLog injected into LoanController");
		}
		

		
		[HttpPost("addloan")]
		public IActionResult RequestLoan([FromBody] LoanModel model)
		{
			model.UserID = GetUserIdFromToken();
			var loan = _mapper.Map<Loan>(model);
			try
			{
				_loanService.RequestLoan(loan);
				LogHelper.AuditLog($"User Added Loan. UserId {model.UserID}");
				return Ok();
			}
			catch (CustomException e)
			{
				LogHelper.ErrorLog($"{e.Message}");
				return BadRequest(new { message = e.Message });
			}
		}

		[HttpGet("userloans")]
		public IActionResult GetUserLoanS()
		{
			int userId = GetUserIdFromToken();

			var loans = _loanService.GetUserLoans(userId);
			var model = _mapper.Map<IList<LoanModel>>(loans);
			
			LogHelper.AuditLog($"Read all loans. UserId {userId}");
			
			return Ok(model);
		}

		[HttpPut("{id}")]
		public IActionResult Update(int id,[FromBody] LoanUpdateModel model)
		{
			int userId = GetUserIdFromToken();
			var loan = _mapper.Map<Loan>(model);
			try
			{
				_loanService.UpdateLoan(id,userId,loan);
				LogHelper.AuditLog($"Loan #{id} was updated. UserId {userId}.");
				return Ok();
			}
			catch (CustomException e)
			{
				LogHelper.ErrorLog($"{e.Message}");
				return BadRequest(new { message = e.Message });
			}
		}

		private int GetUserIdFromToken()
		{
			Request.Headers.TryGetValue("Authorization", out var headerValue);
			var token = headerValue.ToString().Substring(7);

			var handler = new JwtSecurityTokenHandler();
			var jsontoken = handler.ReadToken(token);
			var tokens = jsontoken as JwtSecurityToken;
			var userId = int.Parse(tokens.Claims.First(claim => claim.Type == "unique_name").Value);
			return userId;
		}
	}
}
