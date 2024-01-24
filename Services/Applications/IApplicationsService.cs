using Linkedin.Models;

namespace Linkedin.Services.Applications
{
    public interface IApplicationsService
    {
        /// <summary>
        /// Gets all the applications done by a user.
        /// </summary>
        List<Application> GetApplicationsByUserId(int userId);
        
        /// <summary>
        /// Applies for a job.
        /// </summary>
        void ApplyForJob(int jobId, int jobSeekerId, string resume, string additionalInfo);
        
        /// <summary>
        /// Updates the application status of a job.
        /// </summary>
        void UpdateApplicationStatus(int applicationId, ApplicationStatus newStatus);
        
        /// <summary>
        /// Gets a list of recommended jobs based on the user's skills.
        /// </summary>
        List<Job> GetRecommendedJobs(int userId);

        /// <summary>
        /// Returns a list of job applications required for the recruiters.
        /// </summary>
        List<Application> GetApplicationsForJob(int jobId);
    }
}
