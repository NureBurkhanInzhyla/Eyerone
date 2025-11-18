namespace EyeroneApi.DTOs
{
    public class DroneDto
    {
        public int DroneId { get; set; }
        public string Name { get; set; } 
        public string? Model { get; set; }
        public string SerialNumber { get; set; }
        public DateTime AddedAt { get; set; }

        public UserDto? Owner { get; set; }
    }
}