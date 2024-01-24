using Linkedin.DbModels.Database;
using Linkedin.Models;
using Linkedin.Services.Cache;
using Linkedin.Services.Jobs;
using Linkedin.Services.Logger;
using Microsoft.EntityFrameworkCore;

namespace Linkedin.Services.Applications
{
    public class ApplicationsService : IApplicationsService
    {
        private readonly IApplicationDbContext _context;
        private readonly ILoggerService _logger;
        private readonly ICacheService _cache;
        private readonly IJobsService _jobsService;

        public ApplicationsService(IApplicationDbContext context, ILoggerService logger, ICacheService cacheService, IJobsService jobsService)
        {
            _context = context;
            _logger = logger;
            _cache = cacheService;
            _jobsService = jobsService;
        }

        public List<Application> GetApplicationsByUserId(int userId)
        {
            var applications = _context.Applications
                .Where(a => a.JobSeekerId == userId)
                .Include(app => app.Job)
                .ThenInclude(j => j.Company)
                .ToList();

            if (applications == null)
            {
                _logger.LogMessage($"Tried to get job applications for user {userId} but there are none.");
                return null;
            }

            return applications;
        }

        public void ApplyForJob(int jobSeekerId, int jobId, string resume, string additionalInfo)
        {
            var dateDeadline = _jobsService.GetJobDeadline(jobId);
            if (dateDeadline != DateTime.MinValue &&  DateTime.Now > dateDeadline) return;

            var application = new Application
            {
                JobId = jobId,
                JobSeekerId = jobSeekerId,
                ApplicationDate = DateTime.Now,
                Status = ApplicationStatus.Pending,
                Resume = resume,
                AdditionalInfo = additionalInfo
            };

            _context.Applications.Add(application);
            _context.SaveChanges();

            _logger.LogMessage($"Added a new job application {application.ApplicationId}.");
        }

        public void UpdateApplicationStatus(int applicationId, ApplicationStatus newStatus)
        {
            var application = _context.Applications.Find(applicationId);
            if (application == null)
            {
                _logger.LogError($"Tried to update the status for an application that doesn't exist.");
                return;
            }

            application.Status = newStatus;
             _context.SaveChanges();
        }

        public List<Job> GetRecommendedJobs(int userId)
        {
            var cachedRecommendedJobs = _cache.GetValue<List<Job>>("jobs_recommended");
            if (cachedRecommendedJobs == null)
            {
                var userSkills = _context.Users
                    .Where(u => u.UserId == userId)
                    .Select(u => u.Skills)
                    .FirstOrDefault();

                if (!string.IsNullOrEmpty(userSkills))
                {
                    var userSkillsList = userSkills.ToLower().Split(',').Select(skill => skill.Trim()).ToList();

                    var recommendedJobs = _context.Jobs
                        .Include(c => c.Company)
                        .ToList()
                        .Select(job =>
                        {
                            var jobSkillsList = job.JobDescription.ToLower().Split(' ').Select(skill => skill.Trim()).ToList();
                            var matchingSkills = jobSkillsList.Count(skill => userSkillsList.Contains(skill));
                            var matchingPercentage = (double)matchingSkills / userSkillsList.Count * 100;

                            return new
                            {
                                Job = job,
                                MatchingPercentage = matchingPercentage
                            };
                        })
                        .Where(result => result.MatchingPercentage > 0)
                        .OrderByDescending(result => result.MatchingPercentage)
                        .Take(10)
                        .Select(result => result.Job)
                        .ToList();

                    _cache.SetValue("jobs_recommended", recommendedJobs, TimeSpan.FromMinutes(10));
                    _logger.LogMessage($"Added new job recommendation in cache for user {userId}: {string.Join("\n", recommendedJobs)}");
                    return recommendedJobs;
                }
                else
                {
                    _logger.LogMessage($"No skills found for user {userId}. Unable to provide job recommendations.");
                    cachedRecommendedJobs = new List<Job>();
                }
            }

            _logger.LogMessage($"Retrieved job recommendation from cache for user {userId}: {string.Join("\n", cachedRecommendedJobs)}");
            return cachedRecommendedJobs;
        }

        public List<Application> GetApplicationsForJob(int jobId)
        {
            var applications = _context.Applications
                .Include(a => a.JobSeeker)
                .Where(a => a.JobId == jobId)
                .ToList();

            return applications;
        }
    }
}
