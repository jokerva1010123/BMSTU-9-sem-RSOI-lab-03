using BonusServices.DataAcess;
using ModelDTO.Bonus;

namespace BonusServices.Services
{
    public class PrivilegeHistoryService
    {
        private readonly IPrivilegeHistoryDA iPrivilegeHistoryDA;

        public PrivilegeHistoryService(IPrivilegeHistoryDA iPrivilegeHistoryDA)
        {
            this.iPrivilegeHistoryDA = iPrivilegeHistoryDA;
        }

        public async Task<List<PrivilegeHistory>> GetAll(int page, int size)
        {
            return await iPrivilegeHistoryDA.FindAll(page, size);
        }

        public async Task<PrivilegeHistory> GetPrivilegeHistoryById(int id)
        {
            return await iPrivilegeHistoryDA.FindByID(id);
        }
        public async Task Update(PrivilegeHistory privilege_history)
        {
            await iPrivilegeHistoryDA.Update(privilege_history);
        }

        public async Task<int> Delete(int id)
        {
            return await iPrivilegeHistoryDA.DeleteByID(id);
        }

        public async Task<PrivilegeHistory> Create(PrivilegeHistory privilege_history)
        {
            return await iPrivilegeHistoryDA.Add(privilege_history);
        }
    }
}
