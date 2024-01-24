using Microsoft.AspNetCore.Mvc;
using Linkedin.Models;
using Linkedin.Services.Jobs;
using Linkedin.Services.Applications;
using System.Security.Claims;
using Linkedin.Services.Users;
using Linkedin.Services.Companies;
using Microsoft.AspNetCore.Authorization;
using Linkedin.Services.Logger;
using Linkedin.DbModels.LoginRegister;
using Linkedin.Services.Cache;

namespace Linkedin.Controllers
{
    public class JobsController : Controller
    {
        private readonly IApplicationsService _applicationsService;
        private readonly IJobsService _jobsService;
        private readonly IUsersService _usersService;
        private readonly ICompaniesService _companiesService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILoggerService _logger;
        private readonly ICacheService _cacheService;

        public JobsController(IApplicationsService applicationsService, IJobsService jobsService, IUsersService usersService, ICompaniesService companiesService, ILoggerService logger, IWebHostEnvironment webHostEnvironment, ICacheService cacheService)
        {
            _applicationsService = applicationsService;
            _jobsService = jobsService;
            _usersService = usersService;
            _companiesService = companiesService;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _cacheService = cacheService;
        }
        
        public IActionResult Home()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                var appliedJobIds = _applicationsService.GetApplicationsByUserId(userId)
                    .Select(app => app.JobId)
                    .ToList();

                var recommendedJobs = _applicationsService.GetRecommendedJobs(userId)
                    .Where(job => !appliedJobIds.Contains(job.JobId))
                    .ToList();

                var allJobs = _jobsService.GetAllJobs();
                var jobIds = recommendedJobs
                    .Select(job => job.JobId)
                    .ToList();

                var otherJobs = allJobs
                    .Where(job => !jobIds.Contains(job.JobId) && !appliedJobIds.Contains(job.JobId))
                    .ToList();

                var isRecruiter = _usersService.GetUserById(userId).UserType == UserAccount.Recruiter;
                var postedJobs = isRecruiter ? _jobsService.GetAllJobs()
                    .Where(job => job.RecruiterId == userId)
                    .ToList() : new List<Job>();

                var recruiterCompanies = isRecruiter ? _companiesService.GetCompaniesPostedByRecruiter(userId) : new List<Company>();
                ViewBag.IsRecruiter = isRecruiter;

                var viewModel = new JobsViewModel
                {
                    RecommendedJobs = recommendedJobs,
                    OtherJobs = otherJobs,
                    PostedJobs = postedJobs,
                    IsRecruiter = isRecruiter,
                    RecruiterCompanies = recruiterCompanies
                };

                return View(viewModel);
            }
            else
            {
                return RedirectToAction("LoginRegister", "Account");
            }
        }

        [HttpPost]
        public IActionResult ApplyForJob([FromForm] JobApplicationModel model)
        {
            try
            {
                if (model.Resume != null)
                {
                    var cvFileName = SaveUploadedCV(model.Resume, _webHostEnvironment);
                    var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                    _applicationsService.ApplyForJob(userId, model.JobId, cvFileName, model.AdditionalInfo);
                    _cacheService.Clear("jobs");
                    _cacheService.Clear("jobs_recommended");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while appling for a job: {ex.ToString()}");
            }

            return RedirectToAction("Home");
        }

        private string SaveUploadedCV(IFormFile cv, IWebHostEnvironment webHostEnvironment)
        {
            var uploadsDirectory = Path.Combine(webHostEnvironment.ContentRootPath, "uploads");

            if (!Directory.Exists(uploadsDirectory))
            {
                Directory.CreateDirectory(uploadsDirectory);
            }

            var cvFileName = Guid.NewGuid().ToString() + "_" + cv.FileName;
            var filePath = Path.Combine(uploadsDirectory, cvFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                cv.CopyTo(fileStream);
            }

            return cvFileName;
        }
    }
}