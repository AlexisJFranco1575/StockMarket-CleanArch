using StockMarketApp.Domain.Entities;
using StockMarketApp.Domain;

namespace StockMarketApp.Domain.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync(QueryObject query);
        Task<Stock?> GetByIdAsync(int id);
        Task<Stock?> GetBySymbolAsync(string symbol);
        Task<Stock> CreateAsync(Stock stockModel);
        Task<Stock> UpdateAsync(int id, Stock stockModel);
        Task<Stock> DeleteAsync(int id);
        Task<bool> StockExists(int id);
    }
}