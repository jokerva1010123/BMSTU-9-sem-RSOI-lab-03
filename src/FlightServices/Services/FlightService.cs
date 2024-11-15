using ModelDTO.Flight;

namespace FlightServices.Services
{
    public class FlightService
    {
        private readonly IFlightDA iflightDA;

        public FlightService(IFlightDA iflightDA)
        {
            this.iflightDA = iflightDA;
        }

        public async Task<List<Flight>> GetAll()
        {
            return await iflightDA.FindAll();
        }

        public async Task<Flight> GetFlightByID(int id)
        {
            return await iflightDA.FindByID(id);
        }
        public async Task Update(Flight flight)
        {
            await iflightDA.Update(flight);
        }

        public async Task<int> Delete(int id)
        {
            return await iflightDA.DeleteByID(id);
        }

        public async Task<Flight> Create(Flight flight)
        {
            return await iflightDA.Add(flight);
        }
    }
}
