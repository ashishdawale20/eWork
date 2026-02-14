using CrimePortal.Models;
using Microsoft.EntityFrameworkCore;

namespace CrimePortal.Data
{
    public class CRDbContext : DbContext
    {
        public CRDbContext(DbContextOptions<CRDbContext> options) : base(options)
        {
        }

        public DbSet<CrimeRegister> CrimeRegisters { get; set; }
        public DbSet<Pancha> Panchas { get; set; }
        public DbSet<SeizedItem> SeizedItems { get; set; }
        public DbSet<Sample> Samples { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ✅ UNIQUE CONSTRAINT: Year + CaseNumber + UserId (per user basis)
            modelBuilder.Entity<CrimeRegister>()
                .HasIndex(c => new { c.Year, c.CaseNumber, c.UserId })
                .IsUnique();

            modelBuilder.Entity<Pancha>()
                .HasOne(p => p.CrimeRegister)
                .WithMany(c => c.Panchas)
                .HasForeignKey(p => p.CrimeRegisterId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SeizedItem>()
                .HasOne(s => s.CrimeRegister)
                .WithMany(c => c.SeizedItems)
                .HasForeignKey(s => s.CrimeRegisterId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Sample>()
                .HasOne(s => s.CrimeRegister)
                .WithMany(c => c.Samples)
                .HasForeignKey(s => s.CrimeRegisterId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
