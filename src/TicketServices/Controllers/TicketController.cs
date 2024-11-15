using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModelDTO.Bonus;
using ModelDTO.Flight;
using ModelDTO.Response;
using ModelDTO.Ticket;
using TicketServices.Services;

namespace TicketServices.Controllers
{
    [ApiController]
    [Route("api/v1/tickets")]
    public class TicketController : ControllerBase
    {
        private readonly TicketService ticketServices;

        public TicketController(TicketService ticketServices)
        {
            this.ticketServices = ticketServices;
        }

        private TicketDTO changeDTO(Ticket ticket)
        {
            TicketDTO ticketDTO = new TicketDTO()
            {
                Id = ticket.id,
                Ticketuid = ticket.ticket_uid,
                Username = ticket.username,
                Flightnumber = ticket.flight_number,
                Price = ticket.price,
                Status = ticket.status
            };
            return ticketDTO;
        }

        private IEnumerable<TicketDTO> listTicketDTO(IEnumerable<Ticket> tickets)
        {
            List<TicketDTO> ticketDTOs = new List<TicketDTO>();
            foreach (var ticket in tickets)
            {
                var flightDTO = changeDTO(ticket);
                ticketDTOs.Add(flightDTO);
            }
            return ticketDTOs;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TicketDTO>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllTicket([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            var tickets = await ticketServices.GetAll();
            int total = tickets.Count();
            var lTicketsDTO = listTicketDTO(tickets.Skip((page - 1) * size).Take(size));
            PaginationResponse<IEnumerable<TicketDTO>> response = new()
            {
                Page = page,
                PageSize = size,
                Items = lTicketsDTO,
                TotalElements = total
            };
            return Ok(lTicketsDTO);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TicketDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> getTicketByID([FromRoute] int id)
        {
            var ticket = await ticketServices.GetTicketById(id);
            var response = changeDTO(ticket);
            if (response == null)
                return NotFound();
            return Ok(response);
        }
        [HttpGet("{guid:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TicketDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> getTicketByGuid([FromRoute]Guid guid)
        {
            var ticket = await ticketServices.GetTicketByGuid(guid);
            var response = changeDTO(ticket);
            if (response == null)
                return NotFound();
            return Ok(response);
        }
        [HttpGet("{username}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TicketDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> getTicketByUsername([FromRoute] string username)
        {
            var ticket = await ticketServices.GetTicketByUsername(username);
            if (ticket == null)
                return NotFound();
            var listTickets = listTicketDTO(ticket);
            PaginationResponse<IEnumerable<TicketDTO>> response = new()
            {
                Items = listTickets
            };
            return Ok(response);
        }
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(TicketDTO))]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            await ticketServices.Delete(id);
            return NoContent();
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddTicket([FromBody] TicketDTO TicketDTO)
        {
            var ticket = TicketDTO.changeToDB(TicketDTO);
            var result = await ticketServices.Create(ticket);

            if (result is null)
            {
                return BadRequest();
            }

            var header = $"api/v1/tickets/{result.id}";
            return Created(header, ticket);
        }
        [HttpPatch]
        [Route("{Id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TicketDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateStatus([FromRoute(Name = "Id")] int id, [FromBody] TicketDTO newticket)
        {
            var ticket = await ticketServices.GetTicketById(id);
            if (ticket is null)
                return NotFound();
            ticket.status = newticket.Status;
            await ticketServices.Update(ticket);
            var updatedPrivilege = changeDTO(ticket);
            return Ok(updatedPrivilege);
        }
    }
}
