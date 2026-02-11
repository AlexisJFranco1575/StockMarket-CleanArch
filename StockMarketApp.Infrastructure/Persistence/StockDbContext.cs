using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StockMarketApp.Domain.Entities;

namespace StockMarketApp.Infrastructure.Persistence
{
    public class StockDbContext : IdentityDbContext<AppUser>
    {
        public StockDbContext(DbContextOptions<StockDbContext> options) : base(options)
        {
        }

        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ============================================================
            // 1. SEMBRADO DE ROLES (SEEDING) - ¡TODO FIJO!
            // ============================================================
            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = "Admin",               // <--- ID FIJO (Texto)
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = "ADMIN"  // <--- ¡ESTO FALTABA! Debe ser fijo también
                },
                new IdentityRole
                {
                    Id = "User",                // <--- ID FIJO (Texto)
                    Name = "User",
                    NormalizedName = "USER",
                    ConcurrencyStamp = "USER"   // <--- ¡ESTO FALTABA! Debe ser fijo también
                }
            };
            
            // Esto le dice a EF que use estos datos exactos
            modelBuilder.Entity<IdentityRole>().HasData(roles);


            // ============================================================
            // 2. CONFIGURACIÓN PORTFOLIO (MUCHOS A MUCHOS)
            // ============================================================
            modelBuilder.Entity<Portfolio>(x => x.HasKey(p => new { p.AppUserId, p.StockId }));

            modelBuilder.Entity<Portfolio>()
                .HasOne(u => u.AppUser)
                .WithMany(u => u.Portfolios)
                .HasForeignKey(p => p.AppUserId);

            modelBuilder.Entity<Portfolio>()
                .HasOne(u => u.Stock)
                .WithMany(u => u.Portfolios)
                .HasForeignKey(p => p.StockId);


            // ============================================================
            // 3. CONFIGURACIÓN DECIMALES (DINERO)
            // ============================================================
            modelBuilder.Entity<Stock>()
                .Property(s => s.PurchasePrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Stock>()
                .Property(s => s.CurrentPrice)
                .HasColumnType("decimal(18,2)");
        }
    }
}