using BonusServices.DataAcess;
using ModelDTO.Bonus;

namespace BonusServices.Services
{
    public class PrivilegeService
    {
        private readonly IPrivilegeDA iPrivilegeDA;

        public PrivilegeService(IPrivilegeDA iPrivilegeDA)
        {
            this.iPrivilegeDA = iPrivilegeDA;
        }

        public async Task<List<Privilege>> GetAll(int page, int size)
        {
            return await iPrivilegeDA.FindAll(page, size);
        }

        public async Task<Privilege> GetPrivilegeById(int id)
        {
            return await iPrivilegeDA.FindByID(id);
        }

        public async Task<Privilege> GetPrivilegeByUsername(string username)
        {
            return await iPrivilegeDA.FindByUsername(username);
        }
        public async Task Update(Privilege privilege)
        {
            await iPrivilegeDA.Update(privilege);
        }

        public async Task<int> Delete(int id)
        {
            return await iPrivilegeDA.DeleteByID(id);
        }
        public async Task<Privilege> Create(Privilege privilege)
        {
            return await iPrivilegeDA.Add(privilege);
        }
        public async Task BackBonuses(AddReq addReq)
        {
            await iPrivilegeDA.BackBonuses(addReq);
        }
    }
}
