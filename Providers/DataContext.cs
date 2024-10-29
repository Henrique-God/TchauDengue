using Microsoft.EntityFrameworkCore;
using TchauDengue.Entities;

namespace TchauDengue.Providers
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<User> Users {  get; set; }
    }
}
