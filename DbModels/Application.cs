using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Linkedin.Models
{
    public class Application
    {
        [Key]
        [Column("application_id")]
        public int ApplicationId { get; set; }

        [Required]
        [Column("job_id")]
        public int JobId { get; set; }

        [Required]
        [Column("job_seeker_id")]
        public int JobSeekerId { get; set; }

        [Required]
        [Column("application_date")]
        public DateTime ApplicationDate { get; set; }

        [Required]
        [Column("status")]
        public ApplicationStatus Status { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("resume")]
        public string? Resume { get; set; }

        [MaxLength(50)]
        [Column("additional_info")]
        public string? AdditionalInfo { get; set; }

        [ForeignKey("JobId")]
        public Job? Job { get; set; }

        [ForeignKey("JobSeekerId")]
        public User? JobSeeker { get; set; }

        public override string ToString()
        {
            return $"{ApplicationId}, {JobId}, {Status}";
        }
    }

    public enum ApplicationStatus
    {
        Pending = 0,
        Accepted,
        Rejected
    }
}
