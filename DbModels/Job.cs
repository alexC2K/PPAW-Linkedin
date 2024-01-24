using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Linkedin.Models
{
    public class Job
    {
        [Key]
        [Column("job_id")]
        public int JobId { get; set; }

        [Required]
        [Column("recruiter_id")]
        public int RecruiterId { get; set; }

        [Required]
        [Column("company_id")]
        public int CompanyId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("job_title")]
        public string? JobTitle { get; set; }

        [Required]
        [MaxLength]
        [Column("job_description")]
        public string? JobDescription { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("job_salary")]
        public string? JobSalary { get; set; }

        [Required]
        [Column("date_posted")]
        public DateTime DatePosted { get; set; }

        [Column("date_deadline")]
        public DateTime? DateDeadline { get; set; }

        [ForeignKey("RecruiterId")]
        public User? Recruiter { get; set; }

        [ForeignKey("CompanyId")]
        public Company? Company { get; set; }

        public ICollection<Application> Applications { get; set; } = new List<Application>();

        public override string ToString()
        {
            return $"{JobId}, {JobTitle}, {JobDescription}";
        }
    }
}
