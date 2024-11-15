using Microsoft.EntityFrameworkCore;
using ModelDTO.Bonus;

namespace BonusServices.DataAcess
{
    public class BonusDBContext: DbContext
    {
        public BonusDBContext()
        {
        }
        public BonusDBContext(DbContextOptions options) : base(options)
        {
        }
        public virtual DbSet<Privilege> privilege { get; set; } = null!;
        public virtual DbSet<PrivilegeHistory> privilege_history { get; set; } = null!;
    }
}
