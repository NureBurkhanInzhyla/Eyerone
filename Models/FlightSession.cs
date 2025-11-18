using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EyeroneApi.Models
{
    [Table("FlightSession")]
    public class FlightSession
    {
        [Key, Column("session_id")]
        public int SessionId { get; set; }

        [Required, Column("started_at")]
        public DateTime StartedAt { get; set; }

        [Column("ended_at")]
        public DateTime? EndedAt { get; set; }

        [Column("metadata")]
        public string? Metadata { get; set; }

        [Required, Column("drone_id")]
        public int DroneId { get; set; }

        [ForeignKey("DroneId")]
        public virtual Drone? Drone { get; set; }

    }
}
