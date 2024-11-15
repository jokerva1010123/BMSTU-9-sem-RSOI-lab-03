using FlightServices.Services;
using Microsoft.EntityFrameworkCore;
using ModelDTO.Flight;

namespace FlightServices.DataAcess
{
    public class FlightDA : IFlightDA
    {
        private readonly FlightDBContext flightDB;

        public FlightDA(FlightDBContext flightDBContext)
        {
            flightDB = flightDBContext;
        }

        public async Task<List<Flight>> FindAll()
        {
            try
            {
                return await flightDB.flight.ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<Flight> FindByID(int flightId)
        {
            try
            {
                var flight = await flightDB.flight.FirstOrDefaultAsync(x => x.id == flightId);
                return flight;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Flight> Add(Flight flight)
        {
            try
            {
                var id = flightDB.flight.Count() + 1;
                flight.id = id;
                await flightDB.flight.AddAsync(flight);
                await flightDB.SaveChangesAsync();
                return flight;
            }
            catch
            {
                throw;
            }
        }
        
        public async Task<Flight> Update(Flight flight)
        {
            try
            {
                flightDB.flight.Update(flight);
                await flightDB.SaveChangesAsync();
                return flight;
            }
            catch
            {
                return null;
            }
        }

        public async Task<int> DeleteByID(int id)
        {
            try
            {
                Flight flight = await FindByID(id);
                flightDB.flight.Remove(flight);
                await flightDB.SaveChangesAsync();
                return 1;
            }
            catch
            {
                return -1;
            }
        }
    }
}
