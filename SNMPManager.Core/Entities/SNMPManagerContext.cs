using Microsoft.EntityFrameworkCore;

namespace SNMPManager.Core.Entities
{
    public class SnmpManagerContext : DbContext
    {
        public SnmpManagerContext(DbContextOptions<SnmpManagerContext> options)
            :base(options)
        {
            
        }

        public DbSet<Sonar> Sonars { get; set; }
    }
}
