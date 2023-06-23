using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using LoanApi.Helpers;
using LoanApi.Services.UserService;
using LoanApi.Services.LoanService;

namespace LoanApi
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{

			services.AddDbContext<DataBaseContext>();
			services.AddCors();
			services.AddControllers();
			services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "LoanApi", Version = "v1" });
			});

			var appSettingsSection = Configuration.GetSection("AppSettings");
			services.Configure<AppSettings>(appSettingsSection);
			// configure jwt authentication
			var appSettings = appSettingsSection.Get<AppSettings>();
			var key = Encoding.ASCII.GetBytes(appSettings.Secret);
			services.AddAuthentication(x =>
			{
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
				   .AddJwtBearer(x =>
				   {
					   x.Events = new JwtBearerEvents
					   {
						   OnTokenValidated = context =>
						   {
							   var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
							   var userId = int.Parse(context.Principal.Identity.Name);
							   var user = userService.GetById(userId);
							   if (user == null)
							   {
								   // return unauthorized if user no longer exists
								   context.Fail("Unauthorized");
							   }
							   return Task.CompletedTask;
						   }
					   };
					   x.RequireHttpsMetadata = false;
					   x.SaveToken = true;
					   x.TokenValidationParameters = new TokenValidationParameters
					   {
						   ValidateIssuerSigningKey = true,
						   IssuerSigningKey = new SymmetricSecurityKey(key),
						   ValidateIssuer = false,
						   ValidateAudience = false
					   };
				   });


			// Dependency Injection Configuration
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<ILoanService, LoanService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LoanApi v1"));
			}

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
