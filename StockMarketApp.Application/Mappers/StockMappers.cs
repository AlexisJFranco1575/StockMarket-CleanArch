using StockMarketApp.Application.DTOs;
using StockMarketApp.Domain.Entities;

namespace StockMarketApp.Application.Mappers
{
    public static class StockMappers
    {
        public static StockDto ToStockDto(this Stock stockModel)
        {
            return new StockDto
            {
                Id = stockModel.Id,
                Symbol = stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                PurchasePrice = stockModel.PurchasePrice,
                CurrentPrice = stockModel.CurrentPrice,
                Comments = stockModel.Comments.Select(c => c.ToCommentDto()).ToList()
            };
        }

        public static Stock ToStockFromCreateDTO(this CreateStockRequest stockDto)
        {
            return new Stock
            {
                Symbol = stockDto.Symbol,
                CompanyName = stockDto.CompanyName,
                PurchasePrice = stockDto.PurchasePrice,
                CurrentPrice = stockDto.PurchasePrice,
                Quantity = stockDto.Quantity
            };
        }
    }
}