using ModelDTO.Flight;
using ModelDTO.Ticket;

namespace TicketServices.Services
{
    public interface ITicketDA
    {
        Task<List<Ticket>> FindAll();
        Task<Ticket> FindByID(int ticketId);
        Task<Ticket> FindByGuid(Guid ticketId);
        Task<List<Ticket>> FindByUsername(string username);
        Task<Ticket> Add(Ticket ticket);
        Task<Ticket> Update(Ticket ticket);
        Task<int> DeleteByID(int id);
    }
}
