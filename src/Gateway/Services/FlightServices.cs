using Gateway.IServices;
using ModelDTO.Response;
using ModelDTO.Flight;
using System.Net;

namespace Gateway.Services
{
    public class FlightServices: IFlightServices
    {
        private readonly HttpClient _httpClient = new()
        {
            BaseAddress = new Uri("http://flightservice:8080")
            // BaseAddress = new Uri("http://localhost:8070")
            //  BaseAddress = new Uri("http://host.docker.internal:8070")
            //BaseAddress = new Uri("http://localhost:5008")
        };

        public async Task<bool> HealthCheck()
        {
            using var req = new HttpRequestMessage(HttpMethod.Get, "manage/health");
            using var res = await _httpClient.SendAsync(req);
            return res.StatusCode == HttpStatusCode.OK;
        }

        public async Task<PaginationResponse<IEnumerable<FlightDTO>>> GetAllFlightAsync(int page, int pageSize)
        {
            using HttpRequestMessage req = new(HttpMethod.Get, $"api/v1/flights?page={page}&size={pageSize}");
            using var res = await _httpClient.SendAsync(req);
            var responses = await res.Content.ReadAsStringAsync();
            Console.WriteLine(responses);
            var response = await res.Content.ReadFromJsonAsync<PaginationResponse<IEnumerable<FlightDTO>>>();
            return response;
        }
        public async Task<PaginationResponse<IEnumerable<AirportDTO>>> GetAllAirport()
        {
            using HttpRequestMessage req = new(HttpMethod.Get, $"api/v1/airports");
            using var res = await _httpClient.SendAsync(req);
            var responses = await res.Content.ReadAsStringAsync();
            Console.WriteLine(responses);
            var response = await res.Content.ReadFromJsonAsync<PaginationResponse<IEnumerable<AirportDTO>>>();
            return response;
        }
    }
}
