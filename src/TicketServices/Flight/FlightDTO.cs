namespace ModelDTO.Flight
{
    public class FlightDTO
    {
        public int Id { get; set; }
        public string Flightnumber { get; set; } = null!;
        public DateTime Datetime { get; set; }
        public int? Fromairportid { get; set; }
        public int? Toairportid { get; set; }
        public int Price { get; set; }
    }

}
