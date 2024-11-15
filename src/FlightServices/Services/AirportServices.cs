using FlightServices.DataAcess;
using ModelDTO.Flight;

namespace FlightServices.Services
{
    public class AirportServices
    {
        private readonly IAirportDA airportDA;
        public AirportServices(IAirportDA airportDA)
        {
            this.airportDA = airportDA;
        }
        public async Task<List<Airport>> GetAll()
        {
            return await airportDA.FindAll();
        }
    }
}
