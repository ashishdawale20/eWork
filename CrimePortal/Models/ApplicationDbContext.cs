using CrimePortal.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CrimePortal.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Officer> Officers { get; set; }
        public DbSet<OfficeInfo> Offices { get; set; }

        // नए Crime और Pancha DbSet add करें
        public DbSet<Crime> Crimes { get; set; }
        
    }
}
