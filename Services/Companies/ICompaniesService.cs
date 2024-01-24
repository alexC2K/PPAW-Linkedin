using Linkedin.Models;

namespace Linkedin.Services.Companies
{
    public interface ICompaniesService
    {
        /// <summary>
        /// Gets all companies.
        /// </summary>
        List<Company> GetAllCompanies();

        /// <summary>
        /// Gets a company based on it's id.
        /// </summary>
        Company GetCompanyById(int companyId);

        /// <summary>
        /// Gets all the companies which were added by a recruiter.
        /// </summary>
        List<Company> GetCompaniesPostedByRecruiter(int recruiterId);

        /// <summary>
        /// Deletes a company based on its id.
        /// </summary>
        void DeleteCompany(int companyId);
        
        /// <summary>
        /// Edits a company (name, details, employees, etc).
        /// </summary>
        void EditCompany(Company company);
        
        /// <summary>
        /// Adds a new company.
        /// </summary>
        void AddCompany(Company company);
    }
}
