using ModelDTO.Response;
using ModelDTO.Flight;

namespace Gateway.IServices
{
    public interface IFlightServices
    {
        Task<bool> HealthCheck();
        Task<PaginationResponse<IEnumerable<FlightDTO>>> GetAllFlightAsync(int page, int pageSize);
        Task<PaginationResponse<IEnumerable<AirportDTO>>> GetAllAirport();
    }
}
