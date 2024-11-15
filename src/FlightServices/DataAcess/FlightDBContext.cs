using Microsoft.EntityFrameworkCore;
using ModelDTO.Flight;

namespace FlightServices.DataAcess
{
    public class FlightDBContext: DbContext
    {
        public FlightDBContext()
        {
        }
        public FlightDBContext(DbContextOptions options) : base(options)
        {
        }
        public virtual DbSet<Airport> airport { get; set; } = null!;
        public virtual DbSet<Flight> flight { get; set; } = null!;
    }
}
