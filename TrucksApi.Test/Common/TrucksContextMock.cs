using Microsoft.EntityFrameworkCore;
using TrucksApi.Models;

namespace TrucksApi.Test
{
    public class TrucksContextMock : TrucksContext
    {

        public TrucksContextMock(DbContextOptions<TrucksContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase("SampleMemoryDatabase");
            }
        }

    }
}
