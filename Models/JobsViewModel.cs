namespace Linkedin.Models
{
    public class JobsViewModel
    {
        public List<Job>? RecommendedJobs { get; set; }
        public List<Job>? OtherJobs { get; set; }
        public List<Job>? PostedJobs { get; set; }
        public List<Company>? RecruiterCompanies { get; set; }
        public bool IsRecruiter { get; set; }
    }
}