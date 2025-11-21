using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyerone.Application.DTOs
{
    public class CommandDTO
    {
        public int CommandId { get; set; }
        public int DroneId { get; set; }
        public string CommandParameters { get; set; } 
        public string Status { get; set; } 
        public DateTime CreatedAt { get; set; }
    }
}
