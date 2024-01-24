using Linkedin.DbModels.Database;
using Linkedin.Models;
using Linkedin.Services.Cache;
using Linkedin.Services.Logger;
using Microsoft.EntityFrameworkCore;

namespace Linkedin.Services.Jobs
{
    public class JobsService : IJobsService
    {
        private readonly IApplicationDbContext _context;
        private readonly ICacheService _cache;
        private readonly ILoggerService _logger;

        public JobsService(IApplicationDbContext context, ICacheService cache, ILoggerService logger)
        {
            _context = context;
            _cache = cache;
            _logger = logger;
        }

        public List<Job> GetAllJobs()
        {
            var jobs = _cache.GetValue<List<Job>>("jobs");

            if (jobs == null)
            {
                jobs = _context.Jobs
                    .Include(c => c.Company)
                    .ToList();

                _cache.SetValue("jobs", jobs, TimeSpan.FromMinutes(10));
                _logger.LogMessage("Jobs data retrieved from database and cached.");
            }
            else
            {
                _logger.LogMessage("Jobs data retrieved from cache.");
            }

            return jobs;
        }

        public Job GetJobById(int jobId)
        {
            var job = _context.Jobs.Find(jobId);

            if (job != null)
            {
                _logger.LogMessage($"Job with ID {jobId} retrieved from database.");
                return job;
            }
            else
            {
                _logger.LogError($"Job with ID {jobId} not found in the database.");
                return null;
            }
        }

        public DateTime GetJobDeadline(int jobId)
        {
            var job = _context.Jobs.Find(jobId);
            if (job != null)
            {
                _logger.LogMessage($"Job with ID {jobId} retrieved from database.");
                if (job.DateDeadline != null)
                {
                    return (DateTime)job.DateDeadline;
                }
                else
                {
                    return DateTime.MinValue;
                }
            }
            else
            {
                _logger.LogError($"Job with ID {jobId} not found in the database.");
                return DateTime.MinValue;
            }
        }
    }
}
