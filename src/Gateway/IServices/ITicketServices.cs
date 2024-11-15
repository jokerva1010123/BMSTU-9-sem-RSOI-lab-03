using Microsoft.AspNetCore.Mvc;
using ModelDTO.Response;
using ModelDTO.Ticket;

namespace Gateway.IServices
{
    public interface ITicketServices
    {
        Task<bool> HealthCheck();
        Task<PaginationResponse<IEnumerable<TicketDTO>>> GetAllByUser(string username);
        Task<TicketDTO> GetAllByGuid(Guid guid);
        Task<Ticket> CreateTicket(TicketDTO ticketDTO);
        Task UpdateTicket(int id, TicketDTO ticket);
    }
}
