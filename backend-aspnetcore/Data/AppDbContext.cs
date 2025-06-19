using Microsoft.EntityFrameworkCore;
using backend_aspnetcore.Models;

namespace backend_aspnetcore.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        // DbSet doit s'appeler "Utilisateurs" (pluriel)
        public DbSet<Utilisateur> Utilisateurs { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Utilisateur>(entity =>
            {
                entity.HasIndex(u => u.Email).IsUnique();
            });
        }
    }
}