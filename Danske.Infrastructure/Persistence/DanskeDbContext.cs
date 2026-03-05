using Danske.Domain.Aggregates.Municipality;
using Danske.Domain.Aggregates.Tax;
using Microsoft.EntityFrameworkCore;

namespace Danske.Infrastructure.Persistence
{
    public class DanskeDbContext : DbContext
    {
        public DanskeDbContext(DbContextOptions<DanskeDbContext> options)
        : base(options)
        {
        }

        public DbSet<Municipality> Municipalities { get; set; }

        public DbSet<Tax> Taxes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Municipality>(entity =>
            {
                entity.HasIndex(e => e.Name)
                      .IsUnique()
                      .HasDatabaseName("IX_Municipalities_Name_Unique");
            });
        }
    }
}
