using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EyeroneApi.Models
{
    [Table("Telemetry")]
    public class Telemetry
    {
        [Key]
        [Column("telemetry_id")]
        public int TelemetryId { get; set; }

        [Required]
        [Column("flight_id")]
        public int FlightId { get; set; }

        [Required]
        [Column("timestamp")]
        public DateTime Timestamp { get; set; }

        [Required, Column("latitude")]
        public double Latitude { get; set; }

        [Required, Column("longitude")]
        public double Longitude { get; set; }

        [Required, Column("altitude")]
        public double Altitude { get; set; }

        [Required, Column("speed")]
        public double Speed { get; set; }

        [Required, Column("battery_level")]
        public int BatteryLevel { get; set; }

        [Column("connection_status"), StringLength(50)]
        public string? ConnectionStatus { get; set; } 

        [ForeignKey("FlightId")]
        public virtual FlightSession? FlightSession { get; set; }


    }
}
