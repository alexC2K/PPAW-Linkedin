using Linkedin.Models;

namespace Linkedin.Services.Jobs
{
    public interface IJobsService
    {
        /// <summary>
        /// Gets all available jobs.
        /// </summary>
        List<Job> GetAllJobs();

        /// <summary>
        /// Gets a job based on it's id.
        /// </summary>
        Job GetJobById(int jobId);
        
        /// <summary>
        /// Gets the deadline of a job.
        /// </summary>
        DateTime GetJobDeadline(int jobId);
    }
}
