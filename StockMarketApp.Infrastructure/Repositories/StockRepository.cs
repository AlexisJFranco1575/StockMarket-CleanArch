using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using StockMarketApp.Domain.Entities;
using StockMarketApp.Domain.Interfaces;
using StockMarketApp.Infrastructure.Persistence;

namespace StockMarketApp.Infrastructure.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly StockDbContext _context;

        public StockRepository(StockDbContext context)
        {
            _context = context;
        }

        public async Task<List<Stock>> GetAllAsync()
        {
            return await _context.Stocks.ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stocks.FindAsync(id);
        }

        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            await _context.Stocks.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock> UpdateAsync(int id, Stock stockModel)
        {
            var existingStock = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingStock == null)
            {
                return null;
            }

            existingStock.Symbol = stockModel.Symbol;
            existingStock.CompanyName = stockModel.CompanyName;
            existingStock.PurchasePrice = stockModel.PurchasePrice;
            existingStock.CurrentPrice = stockModel.CurrentPrice;
            existingStock.Quantity = stockModel.Quantity;

            await _context.SaveChangesAsync();
            return existingStock;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            if (stockModel == null)
            {
                return null;
            }

            _context.Stocks.Remove(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }
        public async Task<bool> StockExists(int id)
        {
            return await _context.Stocks.AnyAsync(s => s.Id == id);
        }
    }
}