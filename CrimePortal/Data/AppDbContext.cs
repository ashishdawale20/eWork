using Microsoft.EntityFrameworkCore;
using CrimePortal.Models;

namespace CrimePortal.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<OfficeInfo> Offices { get; set; }  // ← यहाँ OfficeInfo लिखा है
        public DbSet<Officer> Officers { get; set; }
        public DbSet<Jawan> Jawans { get; set; }
    }
}
