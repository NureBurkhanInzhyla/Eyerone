using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EyeroneApi.Models
{
    [Table("Drone")]
    public class Drone
    {
        [Key, Column("drone_id")]
        public int DroneId { get; set; }

        [Required, StringLength(100), Column("name")]
        public string DroneName { get; set; }

        [StringLength(100), Column("model")]
        public string? Model { get; set; }

        [Required,StringLength(100), Column("serial_number")]
        public string SerialNumber{ get; set; }

        [Column("added_at")]
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;

        [Column("owner_id")]
        public int OwnerId { get; set; }

        [ForeignKey("OwnerId")]
        public virtual User? Owner { get; set; }
    }
}
