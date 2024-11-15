using System.ComponentModel.DataAnnotations;

namespace ModelDTO.Bonus
{
    public class PrivilegeHistoryDTO
    {
        [Key]
        public int Id { get; set; }
        public int PrivilegeId { get; set; }
        public Guid TicketUid { get; set; }
        public DateTime Datetime { get; set; }
        public int BalanceDiff { get; set; }
        public string OperationType { get; set; } = null!;

        public PrivilegeHistory changeToDB(PrivilegeHistoryDTO dto)
        {
            return new PrivilegeHistory()
            {
                id = dto.Id,
                privilege_id = dto.PrivilegeId,
                ticket_uid = dto.TicketUid,
                datetime = dto.Datetime,
                balance_diff = dto.BalanceDiff,
                operation_type = dto.OperationType
            };
        }
    }
}
