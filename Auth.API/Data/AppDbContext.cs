using Auth.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Email único
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Seed inicial de roles
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Nombre = "Admin" },
                new Role { Id = 2, Nombre = "User" }
            );
        }
    }
}
