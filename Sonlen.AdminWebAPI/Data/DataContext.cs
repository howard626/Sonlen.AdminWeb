using Microsoft.EntityFrameworkCore;
using Sonlen.WebAdmin.Model;

namespace Sonlen.AdminWebAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Employee> Employee { get; set; }

        public DbSet<ResetPassword> ResetPassword { get; set; }

    }
}
