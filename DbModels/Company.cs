using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Linkedin.Models
{
    public class Company
    {
        [Key]
        [Column("company_id")]
        public int CompanyId { get; set; }

        [Required]
        [Column("recruiter_id")]
        public int RecruiterId { get; set; }

        [Required]
        [MaxLength]
        [Column("company_description")]
        public string? CompanyDescription { get; set; }

        [Required]
        [MaxLength]
        [Column("company_name")]
        public string? CompanyName { get; set; }

        [Required]
        [Column("company_no_employees")]
        public int CompanyNoEmployees { get; set; }

        [ForeignKey("RecruiterId")]
        public User? Recruiter { get; set; }

        public ICollection<Job> Jobs { get; set; } = new List<Job>();

        public override string ToString()
        {
            return $"{CompanyId}, {CompanyDescription}, {CompanyNoEmployees}";
        }
    }

}
