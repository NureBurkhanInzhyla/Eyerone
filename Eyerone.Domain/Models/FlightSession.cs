using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Eyerone.Domain.Models
{
    [Table("FlightSession")]
    public class FlightSession
    {
        [Key, Column("session_id")]
        public int SessionId { get; set; }

        [Required, Column("started_at")]
        public DateTime StartedAt { get; set; } = DateTime.UtcNow;

        [Column("ended_at")]
        public DateTime? EndedAt { get; set; }

        [NotMapped]
        public Dictionary<string, object>? Metadata { get; set; }

        [Column("metadata", TypeName = "json")]
        public string? MetadataJson
        {
            get => Metadata == null ? null : JsonSerializer.Serialize(Metadata);
            set => Metadata = value == null ? null : JsonSerializer.Deserialize<Dictionary<string, object>>(value);
        }

        [Required, Column("drone_id")]
        public int DroneId { get; set; }

    }
}
