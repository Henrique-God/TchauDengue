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
                optionsBuilder.UseNpgsql("Host=estagiosv2.pcs.usp.br:65432;Database=db_grupo02;Username=u_grupo02;Password=grupo02");
            }
        }

        public DbSet<User> Users {  get; set; }
    }
}
