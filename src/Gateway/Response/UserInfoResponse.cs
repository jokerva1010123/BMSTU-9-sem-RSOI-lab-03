using ModelDTO.Bonus;

namespace ModelDTO.Response
{
    public class TicketInfoDTO
    {
        public Guid TicketUid { get; set; }
        public string FlightNumber { get; set; }
        public string FromAirport { get; set; }
        public string ToAirport { get; set; }
        public DateTime Date { get; set; }
        public int Price { get; set; }
        public string Status { get; set; }
    }

    public class AddReq
    {
        public string username { get; set; }
        public Guid ticketUid { get; set; }
    }
    public class PrivilegeInfo
    {
        public int Balance { get; set; }
        public string Status { get; set; }
    }

    public class PrivilegeHistoryInfoDTO
    {
        public DateTime date { get; set; }
        public Guid ticketUid { get; set; }
        public int balanceDiff { get; set; }
        public string operationType { get; set; }
    }

    public class UserInfoResponse
    {
        public List<TicketInfoDTO> tickets { get; set; } = null;
        public PrivilegeInfo privilege { get; set; } = null;
    }
    public class UserInfoResponseNull
    {
        public List<TicketInfoDTO> tickets { get; set; } = null;
        public List<PrivilegeDTO> privilege { get; set; } = null;
    }
    public class PrivilegeAllInfo
    {
        public int Balance { get; set; }
        public string Status { get; set; }
        public List<PrivilegeHistoryInfoDTO> history { get; set; } = null;
    }
    public class TicketBuy
    {
        public string flightNumber { get; set; }
        public int price { get; set; }
        public Boolean paidFromBalance { get; set; }
    }
    public class TicketBuyInfo
    {
        public Guid TicketUid { get; set; }
        public string FlightNumber { get; set; }
        public string FromAirport { get; set; }
        public string ToAirport { get; set; }
        public DateTime Date { get; set; }
        public int Price { get; set; }
        public int paidByMoney { get; set; }
        public int paidByBonuses { get; set; }
        public string Status { get; set; }
        public PrivilegeInfo privilege { get; set; } = null;
    }
   
    public class FlightDTORes
    {
        public int Id { get; set; }
        public string FlightNumber { get; set; } = null!;
        public DateTime Date { get; set; }
        public string FromAirport { get; set; }
        public string ToAirport { get; set; }
        public int Price { get; set; }
    }

}
