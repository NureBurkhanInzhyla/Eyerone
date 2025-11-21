using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Eyerone.Domain.Models
{
    [Table("Alert")]
    public class Alert
    {
        [Key, Column("alert_id")]
        public int AlertId { get; set; }

        [Required, Column("user_id")]
        public int UserId { get; set; }

        [Required, Column("drone_id")]
        public int DroneId { get; set; }

        [Required, StringLength(100), Column("type")]
        public string Type { get; set; }

        [Column("text")]
        public string? Text { get; set; } 

        [Column("is_read")]
        public bool IsRead { get; set; } = false; 

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        [ForeignKey("DroneId")]
        public virtual Drone? Drone { get; set; }
    }
}
