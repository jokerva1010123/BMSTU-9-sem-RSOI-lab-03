using ModelDTO.Bonus;

namespace BonusServices.Services
{
    public enum ExitCode
    {
        Success,
        Constraint,
        Error
    }
    public interface IPrivilegeDA
    {
        Task<List<Privilege>> FindAll(int page, int size);
        Task<Privilege> FindByID(int privilegeId);
        Task<Privilege> FindByUsername(string username);
        Task<Privilege> Add(Privilege privilege);
        Task<Privilege> Update(Privilege privilege);
        Task<int> DeleteByID(int id);
        Task<Privilege> Create(Privilege privilege);
        Task BackBonuses(AddReq addReq);
    }
}
