using Linkedin.DbModels.Database;
using Linkedin.Models;
using Linkedin.Services.Cache;
using Linkedin.Services.Logger;
using Microsoft.EntityFrameworkCore;

namespace Linkedin.Services.Companies
{
    public class CompaniesService : ICompaniesService
    {
        private readonly IApplicationDbContext _context;
        private readonly ICacheService _cache;
        private readonly ILoggerService _logger;

        public CompaniesService(IApplicationDbContext context, ICacheService cache, ILoggerService logger)
        {
            _context = context;
            _cache = cache;
            _logger = logger;
        }

        public void AddCompany(Company company)
        {
            _context.Company.Add(company);
            _context.SaveChanges();
        }

        public void EditCompany(Company company)
        {
            _context.Company.Update(company);
            _context.SaveChanges();
        }

        public void DeleteCompany(int companyId)
        {
            var companyToDelete = _context.Company.Find(companyId);
            if (companyToDelete != null)
            {
                _context.Company.Remove(companyToDelete);
                _context.SaveChanges();
            }
            else
            {
                _logger.LogError("Tried to delete a company which doesn't exist.");
            }
        }

        public List<Company> GetAllCompanies()
        {
            var companies = _cache.GetValue<List<Company>>("companies");

            if (companies == null)
            {
                companies = _context.Company.ToList();
                _cache.SetValue("companies", companies, TimeSpan.FromMinutes(10));
                _logger.LogMessage("Companies data retrieved from database and cached.");
            }
            else
            {
                _logger.LogMessage("Companies data retrieved from cache.");
            }

            return companies;
        }

        public List<Company> GetCompaniesPostedByRecruiter(int recruiterId)
        {
            var companies = _context.Company
                .Include(c => c.Jobs)
                .Where(c => c.RecruiterId == recruiterId)
                .ToList();

            return companies;
        }

        public Company GetCompanyById(int companyId)
        {
            var company = _context.Company.Find(companyId);

            if (company != null)
            {
                _logger.LogMessage($"Company with ID {companyId} retrieved from database.");
            }
            else
            {
                _logger.LogError($"Company with ID {companyId} not found in the database.");
            }

            return company;
        }
    }
}
