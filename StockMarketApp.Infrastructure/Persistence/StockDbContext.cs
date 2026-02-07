using Microsoft.EntityFrameworkCore;
using StockMarketApp.Domain.Entities;

namespace StockMarketApp.Infrastructure.Persistence
{
    public class StockDbContext : DbContext
    {
        public StockDbContext(DbContextOptions<StockDbContext> options) : base(options)
        {
        }

        public DbSet<Stock> Stocks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Stock>()
                .Property(p => p.CurrentPrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Stock>()
                .Property(p => p.PurchasePrice)
                .HasColumnType("decimal(18,2)");
        }
    }
}