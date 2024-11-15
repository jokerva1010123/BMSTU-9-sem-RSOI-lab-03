using Gateway.IServices;
using ModelDTO.Response;
using ModelDTO.Ticket;
using System.Net;

namespace Gateway.Services
{
    public class TicketServices: ITicketServices
    {
        private readonly HttpClient _httpClient = new()
        {
            BaseAddress = new Uri("http://ticketservice:8080")
            // BaseAddress = new Uri("http://localhost:8050")
            //    BaseAddress = new Uri("http://host.docker.internal:8050")
            //BaseAddress = new Uri("http://localhost:5004")
        };

        public async Task<bool> HealthCheck()
        {
            using var req = new HttpRequestMessage(HttpMethod.Get, "manage/health");
            using var res = await _httpClient.SendAsync(req);
            return res.StatusCode == HttpStatusCode.OK;
        }

        public async Task<PaginationResponse<IEnumerable<TicketDTO>>> GetAllByUser(string username)
        {
            using HttpRequestMessage req = new(HttpMethod.Get, $"api/v1/tickets/{username}");
            using var res = await _httpClient.SendAsync(req);
            var responses = await res.Content.ReadAsStringAsync();
            var response = await res.Content.ReadFromJsonAsync<PaginationResponse<IEnumerable<TicketDTO>>>();
            return response;
        }
        public async Task<TicketDTO> GetAllByGuid(Guid guid)
        {
            using HttpRequestMessage req = new(HttpMethod.Get, $"api/v1/tickets/{guid}");
            using var res = await _httpClient.SendAsync(req);
            var responses = await res.Content.ReadAsStringAsync();
            var response = await res.Content.ReadFromJsonAsync<TicketDTO>();
            return response;
        }
        public async Task<Ticket> CreateTicket(TicketDTO ticketDTO)
        {
            using HttpRequestMessage req = new(HttpMethod.Post, $"api/v1/tickets/");
            req.Content = JsonContent.Create(ticketDTO, typeof(TicketDTO));
            using var res = await _httpClient.SendAsync(req);
            res.EnsureSuccessStatusCode();
            var response = await res.Content.ReadFromJsonAsync<Ticket>();
            return response;
        }
        public async Task UpdateTicket(int id, TicketDTO ticket)
        {
            using HttpRequestMessage req = new(HttpMethod.Patch, $"api/v1/tickets/{id}");
            req.Content = JsonContent.Create(ticket, typeof(TicketDTO));
            using var res = await _httpClient.SendAsync(req);
            res.EnsureSuccessStatusCode();
        }
    }
}
