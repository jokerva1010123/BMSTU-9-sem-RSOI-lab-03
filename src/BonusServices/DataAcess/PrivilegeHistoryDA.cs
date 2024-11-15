using BonusServices.Services;
using Microsoft.EntityFrameworkCore;
using ModelDTO.Bonus;

namespace BonusServices.DataAcess
{
    public class PrivilegeHistoryDA : IPrivilegeHistoryDA
    {
        private readonly BonusDBContext bonusDB;

        public PrivilegeHistoryDA(BonusDBContext bonusDBcontext)
        {
            bonusDB = bonusDBcontext;
        }

        public async Task<List<PrivilegeHistory>> FindAll(int page, int size)
        {
            try
            {
                var privilege_histories = await bonusDB.privilege_history.OrderBy(x => x.id).Skip((page - 1) * size).Take(size).ToListAsync();
                return privilege_histories;
            }
            catch
            {
                throw;
            }
        }

        public async Task<PrivilegeHistory> FindByID(int privilege_historyId)
        {
            try
            {
                var privilege_history = await bonusDB.privilege_history.FirstOrDefaultAsync(x => x.id == privilege_historyId);
                return privilege_history;
            }
            catch
            {
                throw;
            }
        }

        public async Task<PrivilegeHistory> Add(PrivilegeHistory privilege_history)
        {
            try
            {
                var id = bonusDB.privilege_history.Count() + 1;
                privilege_history.id = id;
                await bonusDB.privilege_history.AddAsync(privilege_history);
                await bonusDB.SaveChangesAsync();
                return privilege_history;
            }
            catch
            {
                throw;
            }
        }

        public async Task<PrivilegeHistory> Update(PrivilegeHistory privilege_history)
        {
            try
            {
                bonusDB.privilege_history.Update(privilege_history);
                await bonusDB.SaveChangesAsync();
                return privilege_history;
            }
            catch
            {
                return null;
            }
        }

        public async Task<int> DeleteByID(int id)
        {
            try
            {
                PrivilegeHistory privilege_history = await FindByID(id);
                bonusDB.privilege_history.Remove(privilege_history);
                await bonusDB.SaveChangesAsync();
                return 1;
            }
            catch
            {
                return -1;
            }
        }
    }
}
