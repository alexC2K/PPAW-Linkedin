using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Linkedin.Models;
using Linkedin.DbModels.Database;
using Linkedin.Services.Users;
using System.Security.Claims;
using Linkedin.Services.Applications;

namespace Linkedin.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUsersService _usersService;
        private readonly IApplicationsService _applicationsService;

        public UsersController(ApplicationDbContext context, IUsersService usersService, IApplicationsService applicationsService)
        {
            _context = context;
            _usersService = usersService;
            _applicationsService = applicationsService;
        }

        [HttpGet("Home")]
        public IActionResult Home()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                var user = _usersService.GetUserById(userId);
                var isRecruiter = user.UserType == UserAccount.Recruiter;
                ViewBag.IsRecruiter = isRecruiter;
                
                var userApplications = isRecruiter ? null : _applicationsService.GetApplicationsByUserId(userId);
                var recommendedJobs = isRecruiter ? null : _applicationsService.GetRecommendedJobs(userId);
                List<Job> recruiterJobs = isRecruiter ? _usersService.GetRecruiterJobs(userId) : null;
                List<CompanyViewModel> recruiterCompanies = isRecruiter ? _usersService.GetRecruiterCompanies(userId) : null;

                var viewModel = new HomeViewModel
                {
                    User = user,
                    RecommendedJobs = recommendedJobs,
                    UserApplications = userApplications,
                    IsRecruiter = isRecruiter,
                    RecruiterJobs = recruiterJobs,
                    RecruiterCompanies = recruiterCompanies,
                    ApplicationsForJobs = new Dictionary<int, List<Application>>()
                };

                if (isRecruiter)
                {
                    foreach (var job in viewModel.RecruiterJobs)
                    {
                        var applications = _applicationsService.GetApplicationsForJob(job.JobId);
                        viewModel.ApplicationsForJobs.Add(job.JobId, applications);
                    }
                }

                return View(viewModel);
            }
            else
            {
                return RedirectToAction("LoginRegister", "Account");
            }
        }

        [HttpPost]
        public IActionResult AcceptApplication(int applicationId)
        {
            _applicationsService.UpdateApplicationStatus(applicationId, ApplicationStatus.Accepted);

            return RedirectToAction("Home");
        }

        [HttpPost]
        public IActionResult RejectApplication(int applicationId)
        {
            _applicationsService.UpdateApplicationStatus(applicationId, ApplicationStatus.Rejected);

            return RedirectToAction("Home");
        }
    }
}
