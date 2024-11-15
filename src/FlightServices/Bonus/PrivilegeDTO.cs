namespace ModelDTO.Bonus
{
    public class PrivilegeDTO
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Status { get; set; } = null!;
        public int Balance { get; set; }

        public Privilege changeToDB(PrivilegeDTO privilegeDTO)
        {
            return new Privilege()
            {
                username = privilegeDTO.Username,
                status = privilegeDTO.Status,
                balance = privilegeDTO.Balance
            };
        }
    }
}
