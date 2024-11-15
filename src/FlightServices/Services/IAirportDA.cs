using ModelDTO.Flight;

namespace FlightServices.Services
{
    public interface IAirportDA
    {
        Task<List<Airport>> FindAll();
    }
}
