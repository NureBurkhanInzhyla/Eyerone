using Eyerone.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Eyerone.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Drone> Drones { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<FlightSession> FlightSessions { get; set; }
        public DbSet<Telemetry> TelemetryData { get; set; }
        public DbSet<Command> Commands { get; set; }
        public DbSet<Alert> Alerts { get; set; }
    }
}
