using LoanApi.Models.LoanModels;
using LoanApi.Models.UserModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LoanApi.Helpers
{
	public class DataBaseContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DataBaseContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {

            options.UseSqlServer(Configuration.GetConnectionString("LoanApiDatabase"));
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Loan> Loans { get; set; }
    }
}
