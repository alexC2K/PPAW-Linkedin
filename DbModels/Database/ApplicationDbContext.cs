using Linkedin.Models;
using Microsoft.EntityFrameworkCore;

namespace Linkedin.DbModels.Database
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public DbSet<Application> Applications { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Company> Company { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        void IApplicationDbContext.SaveChanges()
        {
            base.SaveChanges();
        }
    }
}
