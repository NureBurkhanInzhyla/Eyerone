using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EyeroneApi.Models
{
    [Table("Command")]
    public class Command
    {
        [Key, Column("command_id")]
        public int CommandId { get; set; }

        [Required, Column("user_id")]
        public int UserId { get; set; }

        [Required, Column("drone_id")]
        public int DroneId { get; set; }

        [Required, StringLength(100),Column("command_type")]
        public string CommandType { get; set; }

        [Column("parameters", TypeName = "jsonb")]
        public string? Parameters { get; set; } 

        [Required, StringLength(50), Column("status")]
        public string Status { get; set; } = "pending"; 

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        [ForeignKey("DroneId")]
        public virtual Drone? Drone { get; set; }
    }
}
