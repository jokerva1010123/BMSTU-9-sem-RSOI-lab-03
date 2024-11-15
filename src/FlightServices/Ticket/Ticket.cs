using System.ComponentModel.DataAnnotations;

namespace ModelDTO.Ticket
{
    public class Ticket
    {
        [Key]
        public int id { get; set; }
        public Guid ticket_uid { get; set; }
        public string username { get; set; } = null!;
        public string flight_number { get; set; } = null!;
        public int price { get; set; }
        public string status { get; set; } = null!;
    }
}
