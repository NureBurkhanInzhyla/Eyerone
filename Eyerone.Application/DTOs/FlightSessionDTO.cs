namespace Eyerone.Application.DTOs
{
    public class FlightSessionDto
    {
        public int SessionId { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
        public TimeSpan? Duration { get; set; }
        public double AverageSpeed { get; set; }
        public List<string> Recommendations { get; set; }
    }

}
