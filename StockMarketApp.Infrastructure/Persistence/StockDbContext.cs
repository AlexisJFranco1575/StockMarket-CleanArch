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

            // --- 2. CONFIGURACIÓN DE RELACIÓN MUCHOS A MUCHOS (PORTFOLIO) ---
            
            // Definimos que la llave primaria es compuesta (UsuarioId + StockId)
            modelBuilder.Entity<Portfolio>(x => x.HasKey(p => new { p.AppUserId, p.StockId }));

            // Relación: Portafolio -> Usuario
            modelBuilder.Entity<Portfolio>()
                .HasOne(u => u.AppUser)
                .WithMany(u => u.Portfolios)
                .HasForeignKey(p => p.AppUserId);

            // Relación: Portafolio -> Stock
            modelBuilder.Entity<Portfolio>()
                .HasOne(u => u.Stock)
                .WithMany(u => u.Portfolios)
                .HasForeignKey(p => p.StockId);
            // -------------------------------------------------------------

            modelBuilder.Entity<Stock>()
                .Property(s => s.PurchasePrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Stock>()
                .Property(s => s.CurrentPrice)
                .HasColumnType("decimal(18,2)");
        }
    }
}