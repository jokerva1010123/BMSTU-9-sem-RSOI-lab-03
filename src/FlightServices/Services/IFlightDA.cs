using ModelDTO.Flight;

namespace FlightServices.Services
{
    public interface IFlightDA
    {
        Task<List<Flight>> FindAll();
        Task<Flight> FindByID(int flightId);
        Task<Flight> Add( Flight flight);
        Task<Flight> Update(Flight flight);
        Task<int> DeleteByID(int id);
    }
}
