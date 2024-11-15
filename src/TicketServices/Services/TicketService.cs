using ModelDTO.Flight;
using ModelDTO.Ticket;

namespace TicketServices.Services
{
    public class TicketService
    {
        private readonly ITicketDA iticketDA;

        public TicketService(ITicketDA iticketDA)
        {
            this.iticketDA = iticketDA;
        }

        public async Task<List<Ticket>> GetAll()
        {
            return await iticketDA.FindAll();
        }

        public async Task<Ticket> GetTicketById(int id)
        {
            return await iticketDA.FindByID(id);
        }
        public async Task<Ticket> GetTicketByGuid(Guid guid)
        {
            return await iticketDA.FindByGuid(guid);
        }

        public async Task<List<Ticket>> GetTicketByUsername(string username)
        {
            return await iticketDA.FindByUsername(username);
        }
        public async Task Update(Ticket ticket)
        {
            await iticketDA.Update(ticket);
        }

        public async Task<int> Delete(int id)
        {
            return await iticketDA.DeleteByID(id);
        }

        public async Task<Ticket> Create(Ticket ticket)
        {
            return await iticketDA.Add(ticket);
        }
    }
}
