using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Payment.Persistence.DAO;

namespace Payment.Persistence
{
    [ExcludeFromCodeCoverage]
    public class PaymentDbContext : DbContext
    {
        public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options)
        {
        }

        public DbSet<Bank> Banks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bank>()
                .HasIndex(b => b.IfscCode)
                .IsUnique();
        }
    }
}