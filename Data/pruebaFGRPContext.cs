using Microsoft.EntityFrameworkCore;
using pruebaFGRP.Models;

namespace pruebaFGRP.Data
{
    public class pruebaFGRPContext : DbContext
    {
        public pruebaFGRPContext(DbContextOptions<pruebaFGRPContext> options) : base(options) { }

        public DbSet<Log> Logs { get; set; }
        public DbSet<Config> Config { get; set; }
        public DbSet<UserInfo>? UserInfo { get; set; }
        public DbSet<Orders>? Orders { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Config>()
                .HasIndex(c => c.Key)
                .IsUnique();
            
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Config>().HasData(
                new Config
                {
                    Id = 1, // Nos aseguramos de que no cree un ID que podría causar conflictos
                    Key = "WorkerServiceMaintenanceInterval",
                    Value = "60000",
                    Description = "Intervalo de ejecución de tarea programada de mantenimiento",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = null
                }
            );

            modelBuilder.Entity<UserInfo>(entity =>
            {
                entity.Property(e => e.DisplayName).HasMaxLength(60).IsUnicode(false);
                entity.Property(e => e.UserName).HasMaxLength(30).IsUnicode(false);
                entity.Property(e => e.Email).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.Password).HasMaxLength(20).IsUnicode(false);
                entity.Property(e => e.CreatedAt).IsUnicode(false);
            });


            modelBuilder.Entity<UserInfo>().HasData(
                new UserInfo
                {
                    Id = 1, // Nos aseguramos de que no cree un ID que podría causar conflictos
                    DisplayName = "Usuario de ejemplo",
                    UserName = "admin",
                    Email = "admin@domain.com",
                    Password = "admin$2020",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                }
            );
        }
    }
}