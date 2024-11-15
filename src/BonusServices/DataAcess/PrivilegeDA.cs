using BonusServices.Services;
using Microsoft.EntityFrameworkCore;
using ModelDTO.Bonus;

namespace BonusServices.DataAcess
{
    public class PrivilegeDA : IPrivilegeDA
    {
        private readonly BonusDBContext bonusDB;

        public PrivilegeDA(BonusDBContext bonusDBcontext)
        {
            bonusDB = bonusDBcontext;
        }

        public async Task<List<Privilege>> FindAll(int page, int size)
        {
            try
            {
                var privileges = await bonusDB.privilege.OrderBy(x => x.id).Skip((page - 1) * size).Take(size).ToListAsync();
                return privileges;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Privilege> FindByID(int privilegeId)
        {
            try
            {
                var privilege = await bonusDB.privilege.FirstOrDefaultAsync(x => x.id == privilegeId);
                return privilege;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Privilege> FindByUsername(string username)
        {
            try
            {
                var privilege = await bonusDB.privilege.FirstOrDefaultAsync(x => x.username == username);
                return privilege;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Privilege> Add(Privilege privilege)
        {
            try
            {
                var id = bonusDB.privilege.Count() + 1;
                privilege.id = id;
                await bonusDB.privilege.AddAsync(privilege);
                await bonusDB.SaveChangesAsync();
                return privilege;
            }
            catch
            {
                throw;
            }
        }
        
        public async Task<Privilege> Update(Privilege privilege)
        {
            try
            {
                bonusDB.privilege.Update(privilege);
                await bonusDB.SaveChangesAsync();
                return privilege;
            }
            catch
            {
                return null;
            }
        }
        public async Task<Privilege> Create(Privilege privilege)
        {
            try
            {
                bonusDB.privilege.Add(privilege);
                await bonusDB.SaveChangesAsync();
                return privilege;
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
                Privilege privilege = await FindByID(id);
                bonusDB.privilege.Remove(privilege);
                await bonusDB.SaveChangesAsync();
                return 1;
            }
            catch
            {
                return -1;
            }
        }
        public async Task BackBonuses(AddReq addReq)
        {
            Privilege privilege = await FindByUsername(addReq.username);
            int old_balance = privilege.balance;
            int new_balance = 0;
            int id = privilege.id;
            PrivilegeHistory privilegeHistory = new PrivilegeHistory();
            foreach (var item in bonusDB.privilege_history)
                if(item.privilege_id == id && item.ticket_uid == addReq.ticketUid)
                {
                    privilegeHistory = item;
                    break;
                }
            int balance_diff = privilegeHistory.balance_diff;
            string operator_type = privilegeHistory.operation_type;
            Console.WriteLine(old_balance.ToString() + " " + balance_diff.ToString() + " " + operator_type);
            if(operator_type == "DEBIT_THE_ACCOUNT")
            {
                new_balance = old_balance - balance_diff;
            }
            else
            {
                new_balance = old_balance + balance_diff;
            }
            privilege.balance = new_balance;
            await Update(privilege);
        }
    }
}
