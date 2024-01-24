using Microsoft.AspNetCore.Mvc;
using Linkedin.Models;
using Linkedin.Services.Companies;
using System.Security.Claims;
using Linkedin.Services.Cache;

namespace Linkedin.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly ICompaniesService _companiesService;
        private readonly ICacheService _cacheService;

        public CompaniesController(ICompaniesService companiesService, ICacheService cacheService)
        {
            _companiesService = companiesService;
            _cacheService = cacheService;
        }

        [HttpGet]
        public IActionResult Home()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var recruiterCompanies = _companiesService.GetCompaniesPostedByRecruiter(userId);

            return View(recruiterCompanies);
        }

        public IActionResult Details(int id)
        {
            var company = _companiesService.GetCompanyById(id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("CompanyName,CompanyDescription,CompanyNoEmployees")] Company company)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            company.RecruiterId = userId;
            _companiesService.AddCompany(company);
            _cacheService.Clear("companies");
            return RedirectToAction(nameof(Home), "Companies");
        }

        public IActionResult Edit(int id)
        {
            var company = _companiesService.GetCompanyById(id);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("CompanyId,CompanyName,CompanyDescription,CompanyNoEmployees")] Company company)
        {
            if (id != company.CompanyId)
            {
                return NotFound();
            }

            company.RecruiterId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            _companiesService.EditCompany(company);
            _cacheService.Clear("companies");
            return RedirectToAction(nameof(Home), "Companies");
        }

        public IActionResult Delete(int id)
        {
            var company = _companiesService.GetCompanyById(id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _companiesService.DeleteCompany(id);
            _cacheService.Clear("companies");
            return RedirectToAction(nameof(Home), "Companies");
        }
    }
}
