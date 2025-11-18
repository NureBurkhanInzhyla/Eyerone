using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EyeroneApi.Models
{
    [Table("User")] 
    public class User
    {
        [Key]
        [Column("user_id")]
        public int UserId { get; set; }

        [Required, StringLength(100)]
        [Column("username")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [Column("email")]
        public string Email { get; set; }

        [Required]
        [Column("password_hash")]
        public string PasswordHash { get; set; }

        [Column("role")]
        public string Role { get; set; } = "operator";

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Drone> Drones { get; set; } = new List<Drone>();
    }
}