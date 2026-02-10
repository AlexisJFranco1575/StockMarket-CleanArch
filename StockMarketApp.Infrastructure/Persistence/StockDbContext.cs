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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 

            modelBuilder.Entity<Stock>()
                .Property(s => s.PurchasePrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Stock>()
                .Property(s => s.CurrentPrice)
                .HasColumnType("decimal(18,2)");
        }
    }
}