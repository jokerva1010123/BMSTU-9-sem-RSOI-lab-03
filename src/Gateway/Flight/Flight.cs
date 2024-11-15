using System.ComponentModel.DataAnnotations;

namespace ModelDTO.Flight
{
    public class Flight
    {
        [Key]
        public int id { get; set; }
        public string flight_number { get; set; } = null!;
        public DateTime datetime { get; set; }
        public int? from_airport_id { get; set; }
        public int? to_airport_id { get; set; }
        public int price { get; set; }
    }
}
