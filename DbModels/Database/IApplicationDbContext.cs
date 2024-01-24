using Linkedin.Models;
using Microsoft.EntityFrameworkCore;

namespace Linkedin.DbModels.Database
{
    public interface IApplicationDbContext
    {
        DbSet<Application> Applications { get; set; }
        DbSet<Company> Company { get; set; }
        DbSet<Job> Jobs { get; set; }
        DbSet<User> Users { get; set; }
        void SaveChanges();
    }
}