using Microsoft.EntityFrameworkCore;
using StockMarketApp.Domain.Entities;
using StockMarketApp.Domain.Interfaces;
using StockMarketApp.Infrastructure.Persistence;
using StockMarketApp.Application.Helpers;

namespace StockMarketApp.Infrastructure.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly StockDbContext _context;

        public StockRepository(StockDbContext context)
        {
            _context = context;
        }

        // --- MÉTODOS DE LECTURA (Con Include) ---
public async Task<List<Stock>> GetAllAsync(QueryObject query)
{
    // 1. Preparamos la consulta (sin ejecutarla aún)
    var stocks = _context.Stocks.Include(c => c.Comments).AsQueryable();

    // 2. Filtros
    if (!string.IsNullOrWhiteSpace(query.CompanyName))
    {
        stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));
    }

    if (!string.IsNullOrWhiteSpace(query.Symbol))
    {
        stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
    }

    // 3. Ordenamiento
    if (!string.IsNullOrWhiteSpace(query.SortBy))
    {
        if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
        {
            stocks = query.IsDescending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
        }
        else if (query.SortBy.Equals("Price", StringComparison.OrdinalIgnoreCase)) // Ordenar por precio de compra
        {
             stocks = query.IsDescending ? stocks.OrderByDescending(s => s.PurchasePrice) : stocks.OrderBy(s => s.PurchasePrice);
        }
    }

    // 4. Ejecutamos SQL y devolvemos la lista filtrada
    return await stocks.ToListAsync();
}

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(i => i.Id == id);
        }

        // --- MÉTODOS DE ESCRITURA ---
        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            await _context.Stocks.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock?> UpdateAsync(int id, Stock stockModel)
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