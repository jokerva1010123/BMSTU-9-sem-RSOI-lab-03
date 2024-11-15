using System.ComponentModel.DataAnnotations;

namespace ModelDTO.Flight
{
    public class Airport
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string city { get; set; }
        public string country { get; set; }
    }
}
