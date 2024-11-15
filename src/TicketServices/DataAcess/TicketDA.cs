using TicketServices.Services;
using Microsoft.EntityFrameworkCore;
using ModelDTO.Ticket;

namespace TicketServices.DataAcess
{
    public class TicketDA : ITicketDA
    {
        private readonly TicketDBContext ticketDB;

        public TicketDA(TicketDBContext ticketDBcontext)
        {
            ticketDB = ticketDBcontext;
        }

        public async Task<List<Ticket>> FindAll()
        {
            try
            {
                return await ticketDB.ticket.ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<Ticket> FindByID(int ticketId)
        {
            try
            {
                var ticket = await ticketDB.ticket.FirstOrDefaultAsync(x => x.id == ticketId);
                return ticket;
            }
            catch
            {
                throw;
            }
        }
        public async Task<Ticket> FindByGuid(Guid ticketId)
        {
            try
            {
                var ticket = await ticketDB.ticket.FirstOrDefaultAsync(x => x.ticket_uid == ticketId);
                return ticket;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<Ticket>> FindByUsername(string username)
        {
            try
            {
                var tickets = await ticketDB.ticket.ToListAsync();
                List<Ticket> result = new List<Ticket>();
                foreach (var ticket in tickets)
                    if(ticket.username== username)
                        result.Add(ticket);
                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Ticket> Add(Ticket ticket)
        {
            try
            {
                var id = ticketDB.ticket.Count() + 1;
                ticket.id = id;
                await ticketDB.ticket.AddAsync(ticket);
                await ticketDB.SaveChangesAsync();
                return ticket;
            }
            catch
            {
                throw;
            }
        }
        
        public async Task<Ticket> Update(Ticket ticket)
        {
            try
            {
                ticketDB.ticket.Update(ticket);
                await ticketDB.SaveChangesAsync();
                return ticket;
            }
            catch
            {
                return null;
            }
        }

        public async Task<int> DeleteByID(int id)
        {
            try
            {
                Ticket ticket = await FindByID(id);
                ticketDB.ticket.Remove(ticket);
                await ticketDB.SaveChangesAsync();
                return 1;
            }
            catch
            {
                return -1;
            }
        }
    }
}
