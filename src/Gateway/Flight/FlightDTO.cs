namespace ModelDTO.Flight
{
    public class FlightDTO
    {
        public int Id { get; set; }
        public string FlightNumber { get; set; } = null!;
        public DateTime Datetime { get; set; }
        public int? FromAirport { get; set; }
        public int? ToAirport { get; set; }
        public int Price { get; set; }
    }

}
