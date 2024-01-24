namespace Linkedin.Models
{
    public class HomeViewModel
    {
        public User User { get; set; }
        public List<Job>? RecommendedJobs { get; set; }
        public List<Application>? UserApplications { get; set; }
        public List<Job>? RecruiterJobs { get; set; }
        public List<CompanyViewModel>? RecruiterCompanies { get; set; }
        public Dictionary<int, List<Application>>? ApplicationsForJobs { get; set; }
        public bool IsRecruiter { get; set; }
    }

}
