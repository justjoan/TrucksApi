using Microsoft.EntityFrameworkCore;

namespace TrucksApi.Models
{
    public class TrucksContext : DbContext
    {
        public TrucksContext(DbContextOptions<TrucksContext> options)
            : base(options)
        {
        }

        public DbSet<Truck> Trucks { get; set; }
    }
}
