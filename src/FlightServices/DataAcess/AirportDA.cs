using FlightServices.Services;
using Microsoft.EntityFrameworkCore;
using ModelDTO.Flight;

namespace FlightServices.DataAcess
{
    public class AirportDA: IAirportDA
    {
        private readonly FlightDBContext airportDB;

        public AirportDA(FlightDBContext airportDB)
        {
            this.airportDB = airportDB;
        }

        public async Task<List<Airport>> FindAll()
        {
            try
            {
                return await airportDB.airport.ToListAsync();
            }
            catch
            {
                throw;
            }
        }
    }
}
