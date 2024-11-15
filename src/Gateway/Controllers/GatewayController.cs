using Gateway.IServices;
using ModelDTO.Response;
using Microsoft.AspNetCore.Mvc;
using ModelDTO.Ticket;
using ModelDTO.Bonus;
using Microsoft.AspNetCore.Server.HttpSys;
using System.Text.Json.Serialization;

namespace Gateway.Controllers
{
    [ApiController]
    [Route("/")]
    public class GatewayController(IFlightServices _flightServices, ITicketServices _ticketServices, IPrivilegeServices _privilegeServices) : ControllerBase
    {
        private readonly IFlightServices flightServices = _flightServices;
        private readonly ITicketServices ticketServices = _ticketServices;
        private readonly IPrivilegeServices privilgeServices = _privilegeServices;

        [HttpGet("manage/health")]
        public ActionResult HealthCheck()
        {
            return Ok();
        }
        [HttpGet("api/v1/flights")]
        public async Task<IActionResult> GetAllFights([FromQuery] int page = 1, [FromQuery] int size = 5)
        {
            var checkFlightService = flightServices.HealthCheck();
            if(!(await checkFlightService))
            {
                var resp = new ErrorResponse()
                {
                    Message = "Flight Service unavailable"
                };
                Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                return new ObjectResult(resp);
            }
            var flights = await flightServices.GetAllFlightAsync(page, size);
            var airports = await flightServices.GetAllAirport();
            PaginationResponse<List<FlightDTORes>> response = new()
            { 
                items = [] 
            };

            response.pageSize = flights.pageSize;
            response.totalElements = flights.totalElements;
            response.page = flights.page;
            foreach (var flight in flights.items)
            {
                var airportFrom = airports.items.FirstOrDefault(x => x.Id == flight.FromAirport);
                var airportTo = airports.items.FirstOrDefault(x => x.Id == flight.ToAirport);
                var to = airportTo.City + " " + airportTo.Name;
                var from = airportFrom.City + " " + airportFrom.Name;
                response.items.Add(new FlightDTORes()
                {
                    FromAirport = from,
                    ToAirport = to,
                    Id = flight.Id,
                    Date = flight.Datetime,
                    Price = flight.Price,
                    FlightNumber = flight.FlightNumber
                });
                
            }
            return Ok(response);
        }
        [HttpGet("api/v1/me")]
        public async Task<IActionResult> GetUserInfoByUsername([FromHeader(Name = "X-User-Name")] string xUserName)
        { 
            var checkFlightService = flightServices.HealthCheck();
            if(!(await checkFlightService))
            {
                var resp = new ErrorResponse()
                {
                    Message = "Flight Service unavailable"
                };
                Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                return new ObjectResult(resp);
            }
            var flights = await flightServices.GetAllFlightAsync(1, 10000);
            var airports = await flightServices.GetAllAirport();
            var checkTicketServices = ticketServices.HealthCheck();
            if(!(await checkTicketServices))
            {
                var resp = new ErrorResponse()
                {
                    Message = "Ticket Service unavailable"
                };
                Response.StatusCode= StatusCodes.Status503ServiceUnavailable;
                return new ObjectResult(resp);
            }
            var tickets = await ticketServices.GetAllByUser(xUserName);
            UserInfoResponse response = new()
            {
                tickets = [],
                privilege = new PrivilegeInfo()
            };
            if (tickets == null)
                return NotFound();
            foreach (var ticket in tickets.items)
            {
                var flight = flights.items.FirstOrDefault(x => x.FlightNumber == ticket.Flightnumber);
                var airportFrom = airports.items.FirstOrDefault(x => x.Id == flight.FromAirport);
                var airportTo = airports.items.FirstOrDefault(x => x.Id == flight.ToAirport);
                string from = airportFrom.City + " " + airportFrom.Name;
                var to = airportTo.City + " " + airportTo.Name ;
                TicketInfoDTO ticketInfoDTO = new TicketInfoDTO()
                {
                    TicketUid = ticket.Ticketuid,
                    FlightNumber = ticket.Flightnumber,
                    Date = flight.Datetime,
                    Price = ticket.Price,
                    Status = ticket.Status,
                    FromAirport = from,
                    ToAirport = to
                };
                response.tickets.Add(ticketInfoDTO);
            }

            var checkBonusServices = privilgeServices.HealthCheck();
            PrivilegeDTO privilege = null;
            if (await checkBonusServices)
            {
                privilege = await privilgeServices.GetByUsername(xUserName);
            }
            if(privilege != null)
            {
                response.privilege.Status = privilege.Status;
                response.privilege.Balance = privilege.Balance;
                return Ok(response);
            }
            UserInfoResponseNull nullresponse = new UserInfoResponseNull()
            {
                privilege = new List<PrivilegeDTO>(),
                tickets = response.tickets
            };
            return Ok(nullresponse);

        }
        [HttpGet("api/v1/tickets")]
        public async Task<IActionResult> GetTicketInfoByUsername([FromHeader(Name = "X-User-Name")] string xUserName)
        {
            var checkFlightService = flightServices.HealthCheck();
            if (!(await checkFlightService))
            {
                var resp = new ErrorResponse()
                {
                    Message = "Flight Service unavailable"
                };
                Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                return new ObjectResult(resp);
            }
            var airports = await flightServices.GetAllAirport();
            var flights = await flightServices.GetAllFlightAsync(1, 10000);
            var checkTicketServices = ticketServices.HealthCheck();
            if (!(await checkTicketServices))
            {
                var resp = new ErrorResponse()
                {
                    Message = "Ticket Service unavailable"
                };
                Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                return new ObjectResult(resp);
            }
            var tickets = await ticketServices.GetAllByUser(xUserName);
            UserInfoResponse response = new()
            {
                tickets = []
            };
            if (tickets == null)
                return NotFound();
            foreach (var ticket in tickets.items)
            {
                var flight = flights.items.FirstOrDefault(x => x.FlightNumber == ticket.Flightnumber);
                var from = airports.items.FirstOrDefault(x => x.Id == flight.FromAirport).Name;
                var to = airports.items.FirstOrDefault(x => x.Id == flight.ToAirport).Name;
                var stringFrom = airports.items.FirstOrDefault(x => x.Id == flight.FromAirport).City + " " + from;
                var stringTo = airports.items.FirstOrDefault(x => x.Id == flight.ToAirport).City + " " + to;
                TicketInfoDTO ticketInfoDTO = new TicketInfoDTO()
                {
                    TicketUid = ticket.Ticketuid,
                    FlightNumber = ticket.Flightnumber,
                    Date = flight.Datetime,
                    Price = ticket.Price,
                    Status = ticket.Status,
                    FromAirport = stringFrom,
                    ToAirport = stringTo
                };
                response.tickets.Add(ticketInfoDTO);
            }
            return Ok(response.tickets);
        }
        [HttpGet("api/v1/privilege")]
        public async Task<IActionResult> GetBonusInfoByUsername([FromHeader(Name = "X-User-Name")] string username)
        { 
            PrivilegeDTO privilege = null;
            List<PrivilegeHistoryDTO> privilegehistory = null;
            var checkBonusServices = await privilgeServices.HealthCheck();
            if (checkBonusServices)
            {
                privilege = await privilgeServices.GetByUsername(username);
                privilegehistory = await privilgeServices.GetAll(1, 1000);
            }
            if (privilege == null && privilegehistory == null)
            {
                var mes = new ErrorResponse()
                {
                    Message = "Bonus Service unavailable"
                };
                return StatusCode(StatusCodes.Status503ServiceUnavailable, mes);
            }
            var history = privilegehistory.Where(x => x.PrivilegeId == privilege.Id);
            PrivilegeAllInfo response = new PrivilegeAllInfo()
            {
                history = []
            };
            response.Status = privilege.Status;
            response.Balance = privilege.Balance;
            foreach (var item in history)
                response.history.Add(new PrivilegeHistoryInfoDTO
                {
                    date = item.Datetime,
                    ticketUid = item.TicketUid,
                    operationType = item.OperationType,
                    balanceDiff = item.BalanceDiff,
                });
            return Ok(response);
        }
        [HttpGet("api/v1/tickets/{ticketUid}")]
        public async Task<IActionResult> GetTicketInfoByUid([FromRoute] Guid ticketUid, [FromHeader(Name = "X-User-Name")] string username)
        {
            var checkTicketServices = ticketServices.HealthCheck();
            if (!(await checkTicketServices))
            {
                var resp = new ErrorResponse()
                {
                    Message = "Ticket Service unavailable"
                };
                Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                return new ObjectResult(resp);
            }
            var info = await ticketServices.GetAllByGuid(ticketUid);
            if (info == null) return NotFound("Not Found");
            var checkFlightService = flightServices.HealthCheck();
            if (!(await checkFlightService))
            {
                var resp = new ErrorResponse()
                {
                    Message = "Flight Service unavailable"
                };
                Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                return new ObjectResult(resp);
            }
            var airports = await flightServices.GetAllAirport();
            var flights = await flightServices.GetAllFlightAsync(1, 10000);
            var flight = flights.items.FirstOrDefault(x => x.FlightNumber == info.Flightnumber);
            var from = airports.items.FirstOrDefault(x => x.Id == flight.FromAirport).Name;
            var to = airports.items.FirstOrDefault(x => x.Id == flight.ToAirport).Name;
            var stringFrom = airports.items.FirstOrDefault(x => x.Id == flight.FromAirport).City + " " + from;
            var stringTo = airports.items.FirstOrDefault(x => x.Id == flight.ToAirport).City + " " + to;
            TicketInfoDTO response = new()
            {
                TicketUid = info.Ticketuid,
                FlightNumber = info.Flightnumber,
                Date = flight.Datetime,
                Price = info.Price,
                Status = info.Status,
                FromAirport = stringFrom,
                ToAirport = stringTo
            };
            return Ok(response);
        }
        [HttpPost("api/v1/tickets")]
        public async Task<IActionResult> BuyTicket([FromHeader(Name = "X-User-Name")] string username, [FromBody] TicketBuy ticket)
        {
            var checkFlightService = flightServices.HealthCheck();
            if (!(await checkFlightService))
            {
                var resp = new ErrorResponse()
                {
                    Message = "Flight Service unavailable"
                };
                Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                return new ObjectResult(resp);
            }
            var flights = await flightServices.GetAllFlightAsync(1, 10000);
            var airports = await flightServices.GetAllAirport();
            TicketBuyInfo response;
            TicketDTO newticket;
            PrivilegeHistoryDTO history;
            var flight = flights.items.FirstOrDefault(x => x.FlightNumber == ticket.flightNumber);
            if (flight == null)
                return StatusCode(500, $"Полет с номером рейса {ticket.flightNumber} не найден");
            var from = airports.items.FirstOrDefault(x => x.Id == flight.FromAirport).Name;
            var to = airports.items.FirstOrDefault(x => x.Id == flight.ToAirport).Name;
            var stringFrom = airports.items.FirstOrDefault(x => x.Id == flight.FromAirport).City + " " + from;
            var stringTo = airports.items.FirstOrDefault(x => x.Id == flight.ToAirport).City + " " + to;
            var paidByMoney = 0;
            var paidByBonuses = 0;
            var checkBonusServices = await privilgeServices.HealthCheck();
            var privilegeUser = new PrivilegeDTO();

            if (checkBonusServices)
                privilegeUser = await privilgeServices.GetByUsername(username);
            else 
            {
                var resp = new ErrorResponse()
                    {
                        Message = "Bonus Service unavailable"
                    };
                    Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                    return new ObjectResult(resp);
            }
            if (ticket.paidFromBalance)
            {
                paidByBonuses = paidByBonuses = privilegeUser.Balance <= flight.Price ? privilegeUser.Balance : flight.Price;
                paidByMoney = ticket.price - paidByBonuses;
                newticket = new()
                {
                    Ticketuid = Guid.NewGuid(),
                    Flightnumber = ticket.flightNumber,
                    Price = ticket.price,
                    Status = "PAID",
                    Username = username
                };
                var checkTicketServices = ticketServices.HealthCheck();
                if (!(await checkTicketServices))
                {
                    var resp = new ErrorResponse()
                    {
                        Message = "Ticket Service unavailable"
                    };
                    Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                    return new ObjectResult(resp);
                }
                var t = await ticketServices.CreateTicket(newticket);
                var paidTickets = await ticketServices.GetAllByGuid(newticket.Ticketuid);
                history = new PrivilegeHistoryDTO()
                {
                    Id = 1,
                    BalanceDiff = -paidByBonuses,
                    Datetime = DateTime.UtcNow,
                    OperationType = "FILL_IN_BALANCE",
                    PrivilegeId = privilegeUser.Id,
                    TicketUid = paidTickets.Ticketuid
                };
                privilegeUser.Balance -= paidByBonuses;
                if (!( checkBonusServices))
                {
                    var resp = new ErrorResponse()
                    {
                        Message = "Bonus Service unavailable"
                    };
                    Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                    return new ObjectResult(resp);
                }
                await privilgeServices.UpdatePrivilege(privilegeUser.Id, privilegeUser);
                if (!(checkBonusServices))
                {
                    var resp = new ErrorResponse()
                    {
                        Message = "Bonus Service unavailable"
                    };
                    Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                    return new ObjectResult(resp);
                }
                await privilgeServices.AddNew(history);
                response = new()
                {
                    FlightNumber = ticket.flightNumber,
                    Price = ticket.price,
                    Status = "PAID",
                    FromAirport = stringFrom,
                    ToAirport = stringTo,
                    TicketUid = paidTickets.Ticketuid,
                    Date = flight.Datetime,
                    paidByBonuses = paidByBonuses,
                    paidByMoney = paidByMoney,
                    privilege = new()
                    {
                        Balance = privilegeUser.Balance,
                        Status = privilegeUser.Status
                    }
                };
                return Ok(response);
            }
            else
            {
                paidByMoney = ticket.price;
                newticket = new TicketDTO
                {
                    Ticketuid = Guid.NewGuid(),
                    Flightnumber = ticket.flightNumber,
                    Price = flight.Price,
                    Status = "PAID",
                    Username = username,
                };
                var checkTicketServices = ticketServices.HealthCheck();
                if (!(await checkTicketServices))
                {
                    var resp = new ErrorResponse()
                    {
                        Message = "Ticket Service unavailable"
                    };
                    Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                    return new ObjectResult(resp);
                }
                var paidTicket = await ticketServices.CreateTicket(newticket);
                history = new PrivilegeHistoryDTO
                {
                    Id = 1,
                    BalanceDiff = ticket.price / 10,
                    Datetime = DateTime.UtcNow,
                    OperationType = "DEBIT_THE_ACCOUNT",
                    PrivilegeId = privilegeUser.Id,
                    TicketUid = paidTicket.ticket_uid
                };
                privilegeUser.Balance += ticket.price / 10;
                if (!checkBonusServices)
                {
                    var resp = new ErrorResponse()
                    {
                        Message = "Bonus Service unavailable"
                    };
                    Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                    return new ObjectResult(resp);
                }
                await privilgeServices.UpdatePrivilege(privilegeUser.Id, privilegeUser);
                if (!checkBonusServices)
                {
                    var resp = new ErrorResponse()
                    {
                        Message = "Bonus Service unavailable"
                    };
                    Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                    return new ObjectResult(resp);
                }
                await privilgeServices.AddNew(history);
                response = new TicketBuyInfo
                {
                    TicketUid = paidTicket.ticket_uid,
                    FlightNumber = ticket.flightNumber,
                    Price = ticket.price,
                    Status = "PAID",
                    FromAirport = stringFrom,
                    ToAirport = stringTo,
                    Date = flight.Datetime,
                    paidByBonuses = paidByBonuses,
                    paidByMoney = paidByMoney,
                    privilege = new()
                    {
                        Balance = privilegeUser.Balance,
                        Status = privilegeUser.Status
                    }
                };
                return Ok(response);
            }
            
        }
        [HttpDelete("api/v1/tickets/{ticketUid}")]
        public async Task<IActionResult> CancleTicket([FromHeader(Name = "X-User-Name")] string username, [FromRoute] Guid ticketUid)
        {
            var checkTicketServices = ticketServices.HealthCheck();
            if (!(await checkTicketServices))
            {
                var resp = new ErrorResponse()
                {
                    Message = "Ticket Service unavailable"
                };
                Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                return new ObjectResult(resp);
            }
            var ticket = await ticketServices.GetAllByGuid(ticketUid);
            if(ticket == null) return NotFound("Билет не найден");
            ticket.Status = "CANCELED";
            await ticketServices.UpdateTicket(ticket.Id, ticket);
            PrivilegeDTO privilege = null;
            List<PrivilegeHistoryDTO> histories = null;
            var checkBonusServices = await privilgeServices.HealthCheck();
            if (checkBonusServices)
                privilege = await privilgeServices.GetByUsername(username);
            if (checkBonusServices)
                histories = await privilgeServices.GetAll(1, 1000);
            if (privilege == null && histories == null)
            {
                privilgeServices.Add(username, ticketUid);
                return StatusCode(204);
            }
            var history = histories.Where(x => x.PrivilegeId == privilege.Id).FirstOrDefault(x => x.TicketUid == ticket.Ticketuid);
            var newHistory = new PrivilegeHistoryDTO
            {
                BalanceDiff = -history.BalanceDiff,
                Datetime = DateTime.UtcNow,
                OperationType = history.OperationType == "FILL_IN_BALANCE" ? "DEBIT_THE_ACCOUNT" : "FILL_IN_BALANCE",
                PrivilegeId = privilege.Id,
                TicketUid = ticket.Ticketuid
            };
            privilege.Balance += newHistory.BalanceDiff;
            if(checkBonusServices)
                await privilgeServices.UpdatePrivilege(privilege.Id, privilege);
            if (checkBonusServices)
                await privilgeServices.AddNew(newHistory);
            return StatusCode(204);
        }
    }
}
