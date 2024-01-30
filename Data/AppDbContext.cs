using Microsoft.EntityFrameworkCore;
using SellIntegro.Models;

namespace SellIntegro.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Inscription> Inscriptions { get; set; }
    }
}
