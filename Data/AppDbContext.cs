using Microsoft.EntityFrameworkCore;
using Notes_with_tagging.Models;

namespace Notes_with_tagging.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Inscription> Inscriptions { get; set; }
    }
}
