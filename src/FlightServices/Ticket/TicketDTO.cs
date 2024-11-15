namespace ModelDTO.Ticket
{
    public class TicketDTO
    {
        public int Id { get; set; }
        public Guid Ticketuid { get; set; }
        public string Username { get; set; }
        public string Flightnumber { get; set; }
        public int Price { get; set; }
        public string Status { get; set; }
        public Ticket changeToDB(TicketDTO ticketDTO)
        {
            return new Ticket()
            { 
                id = ticketDTO.Id,
                ticket_uid = ticketDTO.Ticketuid,
                username = ticketDTO.Username,
                flight_number = ticketDTO.Flightnumber,
                price = ticketDTO.Price,
                status = ticketDTO.Status,
            };
        }
    }
}
