using Microsoft.EntityFrameworkCore;
using ModelDTO.Ticket;

namespace TicketServices.DataAcess
{
    public class TicketDBContext: DbContext
    {
        public TicketDBContext()
        {
        }
        public TicketDBContext(DbContextOptions options) : base(options)
        {
        }
        public virtual DbSet<Ticket> ticket { get; set; } = null!;
    }
}
