using Microsoft.EntityFrameworkCore;
using TchauDengue.Entities;

namespace TchauDengue.Providers
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("postgresql://u_grupo02:grupo02@estagiosv2.pcs.usp.br:65432/db_grupo02");
            }
        }

        public DbSet<User> Users {  get; set; }
    }
}
