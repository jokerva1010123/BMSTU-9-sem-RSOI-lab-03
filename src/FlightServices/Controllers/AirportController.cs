using Microsoft.AspNetCore.Mvc;
using FlightServices.Services;
using ModelDTO.Flight;
using ModelDTO.Response;

namespace FlightServices.Controllers
{
    [ApiController]
    [Route("api/v1/airports/")]
    public class AirportController : ControllerBase
    {
        private readonly AirportServices airportServices;
        public AirportController(AirportServices airportServices)
        {
            this.airportServices = airportServices;
        }
        private AirportDTO changeDTO(Airport airport)
        {
            AirportDTO airportDTO = new AirportDTO()
            {
                Id = airport.id,
                Name = airport.name,
                Country = airport.country,
                City = airport.city,
            };
            return airportDTO;
        }

        private List<AirportDTO> listAirportDTO(IEnumerable<Airport> airports)
        {
            List<AirportDTO> airportDTOs = new List<AirportDTO>();
            foreach (var airport in airports)
            {
                var airportDTO = changeDTO(airport);
                airportDTOs.Add(airportDTO);
            }
            return airportDTOs;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AirportDTO>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAirport([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var airports = await airportServices.GetAll();
            int total = airports.Count;
            var lFlightsDTO = listAirportDTO(airports.Skip((page - 1) * size).Take(size));
            PaginationResponse<IEnumerable<AirportDTO>> response = new()
            {
                Page = page,
                PageSize = size,
                Items = lFlightsDTO,
                TotalElements = total
            };
            return Ok(response);
        }
    }
}
