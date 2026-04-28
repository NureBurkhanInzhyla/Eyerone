using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyerone.Application.DTOs
{
    public class AlertDTO
    {
        public int AlertId { get; set; }
        public int UserId { get; set; }
        public int DroneId { get; set; }
        public string DroneName { get; set; }
        public string Type { get; set; }
        public string? Text { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
