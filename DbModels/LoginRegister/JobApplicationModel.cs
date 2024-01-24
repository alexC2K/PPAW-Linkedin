namespace Linkedin.DbModels.LoginRegister
{
    public class JobApplicationModel
    {
        public int JobId { get; set; }
        public IFormFile Resume {  get; set; }
        public string AdditionalInfo { get; set; }
    }
}
