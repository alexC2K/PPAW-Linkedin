using Linkedin.DbModels.Database;
using Linkedin.Models;
using Linkedin.Services.Cache;
using Linkedin.Services.Logger;
using Microsoft.EntityFrameworkCore;

namespace Linkedin.Services.Users
{
    public class UsersService : IUsersService
    {
        private readonly IApplicationDbContext _context;
        private readonly ICacheService _cache;
        private readonly ILoggerService _logger;

        public UsersService(IApplicationDbContext context, ICacheService cache, ILoggerService logger)
        {
            _context = context;
            _cache = cache;
            _logger = logger;
        }

        public List<User> GetAllUsers()
        {
            var users = _cache.GetValue<List<User>>("users");

            if (users == null)
            {
                users = _context.Users.ToList();
                _cache.SetValue("users", users, TimeSpan.FromMinutes(10));
                _logger.LogMessage("Users data retrieved from database and cached.");
            }
            else
            {
                _logger.LogMessage("Users data retrieved from cache.");
            }

            return users;
        }

        public User GetUserById(int userId)
        {
            var user = _context.Users.Find(userId);

            if (user != null)
            {
                _logger.LogMessage($"User with ID {userId} retrieved from database.");
            }
            else
            {
                _logger.LogError($"User with ID {userId} not found in the database.");
            }

            return user;
        }

        public User RegisterUser(string email, string password, string name, string phoneNo, string skills)
        {
            var newUser = new User
            {
                Email = email,
                Password = password,
                Name = name,
                PhoneNo = phoneNo,
                Skills = skills
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            _logger.LogMessage($"User registered with email: {email}");

            return newUser;
        }

        public User LoginUser(string email, string password)
        {
            var user = _context.Users.
                FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user == null)
            {
                _logger.LogMessage($"Failed login attempt with email: {email}");
                return null;
            }

            _logger.LogMessage($"User logged in with email: {email}");
            return user;
        }

        public User GetUserByEmail(string email)
        {
            return _context.Users.
                FirstOrDefault(u => u.Email == email);
        }

        public List<CompanyViewModel> GetRecruiterCompanies(int recruiterId)
        {
            var recruiter = _context.Users
                .Include(u => u.Companies)
                .FirstOrDefault(u => u.UserId == recruiterId && u.UserType == UserAccount.Recruiter);

            if (recruiter != null && recruiter.Companies != null && recruiter.Companies.Any())
            {
                var companiesViewModel = recruiter.Companies
                    .Select(company => new CompanyViewModel
                    {
                        Name = company.CompanyName,
                        Description = company.CompanyDescription,
                        NumberOfEmployees = company.CompanyNoEmployees
                    })
                    .ToList();

                _logger.LogMessage($"Found the following companies for user {recruiterId}: {string.Join("\n", companiesViewModel)}");
                return companiesViewModel;
            }

            return null;
        }

        public List<Job> GetRecruiterJobs(int recruiterId)
        {
            var recruiter = _context.Users
                .Include(u => u.Companies)
                .ThenInclude(c => c.Jobs)
                .FirstOrDefault(u => u.UserId == recruiterId && u.UserType == UserAccount.Recruiter);

            if (recruiter != null && recruiter.Companies != null && recruiter.Companies.Any())
            {
                var recruiterJobs = recruiter.Companies
                    .SelectMany(c => c.Jobs)
                    .ToList();

                return recruiterJobs;
            }

            return null;
        }
    }
}
