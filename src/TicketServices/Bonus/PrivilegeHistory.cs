using System.ComponentModel.DataAnnotations;

namespace ModelDTO.Bonus
{
    public class PrivilegeHistory
    {
        [Key]
        public int id { get; set; }
        public int privilege_id { get; set; }
        public Guid ticket_uid { get; set; }
        public DateTime datetime { get; set; }
        public int balance_diff { get; set; }
        public string operation_type { get; set; } = null!;
    }
}
