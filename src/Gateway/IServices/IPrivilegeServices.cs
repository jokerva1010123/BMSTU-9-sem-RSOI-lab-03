using Gateway.Services;
using ModelDTO.Bonus;
using System.Net.Http;

namespace Gateway.IServices
{
    public interface IPrivilegeServices
    {
        void Add(string username, Guid ticketUid);
        Task<bool> HealthCheck();
        Task<PrivilegeDTO> GetByUsername(string username);
        Task UpdatePrivilege(int id, PrivilegeDTO privilege);
        public Task<List<PrivilegeHistoryDTO>> GetAll(int page, int size);
        public Task AddNew(PrivilegeHistoryDTO history);
    }
}
