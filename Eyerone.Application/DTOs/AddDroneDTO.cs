using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyerone.Application.DTOs
{
    public class AddDroneDTO
    {
        public string Name { get; set; }
        public string? Model { get; set; }
        public string SerialNumber { get; set; }

        [Required]
        public int OwnerId { get; set; }
    }
}
