using System.ComponentModel.DataAnnotations;

namespace ModelDTO.Bonus
{
    public class Privilege
    {
        [Key]
        public int id { get; set; }
        public string username { get; set; } = null!;
        public string status { get; set; } = null!;
        public int balance { get; set; }
    }
}
