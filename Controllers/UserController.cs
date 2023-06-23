using System;
using AutoMapper;
using System.Text;
using LoanApi.Helpers;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using LoanApi.Models.UserModels;
using LoanApi.Services.UserModels;
using LoanApi.Services.UserService;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;


namespace LoanApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UserController : ControllerBase
	{

		private IUserService _userService;
		private IMapper _mapper;
		private readonly AppSettings _appSettings;
		public UserController(
		IUserService userService,
		IMapper mapper,
		IOptions<AppSettings> appSettings)
		{
			_userService = userService;
			_mapper = mapper;
			_appSettings = appSettings.Value;
		}

		[AllowAnonymous]
		[HttpPost("register")]
		public IActionResult Register([FromBody] RegisterModel registerModel)
		{
			var user = _mapper.Map<User>(registerModel);
			try
			{

				_userService.Register(user, registerModel.Password);
				LogHelper.AuditLog($"New User was created. Username -  {registerModel.Username}");
				return Ok();
			}
			catch (CustomException e)
			{
				LogHelper.ErrorLog($"{e.Message}");
				return BadRequest(new { message = e.Message });
			}
		}

		[AllowAnonymous]
		[HttpPost("login")]
		public IActionResult Login([FromBody] LoginModel model)
		{
			try
			{
				var user = _userService.Login(model);

				if (user == null)
					return BadRequest(new { message = "Username or Password is incorrect" });
				string tokenString = GenerateToken(user);
				LogHelper.AuditLog($"User logged in. Username -  {model.Username}");
				return Ok(new
				{
					Id = user.Id,
					Username = user.Username,
					FirstName = user.FirstName,
					LastName = user.LastName,
					Token = tokenString
				});
			}
			catch (Exception e)
			{

				LogHelper.ErrorLog($"{e.Message}");
				return BadRequest(new { message = e.Message });
			}
		}


		[HttpGet("{id}")]
		public IActionResult GetById(int id)
		{
			var user = _userService.GetById(id);
			var model = _mapper.Map<UserModel>(user);
			LogHelper.AuditLog($"get data about user - {model.Username}");
			return Ok(model);
		}
		private string GenerateToken(User user)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Name, user.Id.ToString())
				}),
				Expires = DateTime.UtcNow.AddDays(7),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			var tokenString = tokenHandler.WriteToken(token);
			return tokenString;
		}
	}
}
