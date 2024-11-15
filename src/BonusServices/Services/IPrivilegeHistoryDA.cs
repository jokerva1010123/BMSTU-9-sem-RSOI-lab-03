using ModelDTO.Bonus;

namespace BonusServices.Services
{
    public interface IPrivilegeHistoryDA
    {
        Task<List<PrivilegeHistory>> FindAll(int page, int size);
        Task<PrivilegeHistory> FindByID(int privilege_historyId);
        Task<PrivilegeHistory> Add(PrivilegeHistory privilege_history);
        Task<PrivilegeHistory> Update(PrivilegeHistory privilege_history);
        Task<int> DeleteByID(int id);
    }
}
