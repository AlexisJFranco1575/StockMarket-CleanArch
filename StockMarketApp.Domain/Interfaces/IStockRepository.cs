using StockMarketApp.Domain.Entities;

namespace StockMarketApp.Domain.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync();
        Task<Stock?> GetByIdAsync(int id);
        Task<Stock> CreateAsync(Stock stockModel);
        Task<Stock> UpdateAsync(int id, Stock stockModel);
        Task<Stock> DeleteAsync(int id);
    }
}