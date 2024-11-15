using Microsoft.AspNetCore.Mvc;
using FlightServices.Services;
using ModelDTO.Flight;
using ModelDTO.Response;

namespace FlightServices.Controllers
{
    [ApiController]
    [Route("api/v1/flights")]
    public class FlightController : ControllerBase
    {
        private readonly FlightService flightServices;

        public FlightController(FlightService flightServices)
        {
            this.flightServices = flightServices;
        }

        private FlightDTO changeDTO(Flight flight)
        {
            FlightDTO flightDTO = new FlightDTO()
            {
                Id = flight.id,
                FlightNumber = flight.flight_number,
                Datetime = flight.datetime,
                FromAirport = flight.from_airport_id,
                ToAirport = flight.to_airport_id,
                Price = flight.price
            };
            return flightDTO;
        }

        private List<FlightDTO> listFlightDTO(IEnumerable<Flight> flights)
        {
            List<FlightDTO> flightDTOs = new List<FlightDTO>();
            foreach (var flight in flights)
            {
                var flightDTO = changeDTO(flight);
                flightDTOs.Add(flightDTO);
            }
            return flightDTOs;
        }

        private Flight changeToDB(FlightDTO flightDTO)
        {
            return new Flight()
            {
                flight_number = flightDTO.FlightNumber,
                datetime = flightDTO.Datetime,
                from_airport_id = flightDTO.FromAirport,
                to_airport_id = flightDTO.ToAirport,
                price = flightDTO.Price,
            };
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<FlightDTO>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllFlights([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var flights = await flightServices.GetAll();
            int total = flights.Count;
            var lFlightsDTO = listFlightDTO(flights.Skip((page - 1) * size).Take(size));
            PaginationResponse<IEnumerable<FlightDTO>> response = new()
            {
                Page = page,
                PageSize = size,
                Items = lFlightsDTO,
                TotalElements = total
            };
            return Ok(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FlightDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> getFlightByID(int id)
        {
            var flight = await flightServices.GetFlightByID(id);
            var response = changeDTO(flight);
            if (response == null)
                return NotFound();
            return Ok(response);
        }
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(FlightDTO))]
        public async Task<IActionResult> DeleteFlight(int id)
        {
            await flightServices.Delete(id);
            return NoContent();
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddFlight([FromBody] FlightDTO flightDTO)
        {
            var flight = changeToDB(flightDTO);
            var result = await flightServices.Create(flight);

            if (result is null)
            {
                return BadRequest();
            }

            var header = $"api/v1/flights/{result.id}";
            return Created(header, flight);
        }
    }
}
