using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Linkedin.Models
{
    public class User
    {
        [Key]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("email")]
        public string? Email { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("password")]
        public string? Password { get; set; }

        [Required]
        [Column("user_type")]
        public UserAccount UserType { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("name")]
        public string? Name { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("phone_no")]
        public string? PhoneNo { get; set; }

        [MaxLength]
        [Column("skills")]
        public string? Skills { get; set; }

        public ICollection<Application> Applications { get; set; } = new List<Application>();

        public ICollection<Company> Companies { get; set; } = new List<Company>();

        public ICollection<Job> Jobs { get; set; } = new List<Job>();

        public override string ToString()
        {
            return $"{UserId}, {Email}, {Name}, {UserType}";
        }
    }

    public enum UserAccount
    {
        Seeker = 0,
        Recruiter
    }
}
