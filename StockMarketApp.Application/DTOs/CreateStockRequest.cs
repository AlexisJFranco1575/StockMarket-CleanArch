namespace StockMarketApp.Application.DTOs
{
    public record CreateStockRequest(
        string Symbol,
        string CompanyName,
        decimal PurchasePrice,
        int Quantity
    );
}

namespace StockMarketApp.Application.DTOs
{
    public record UpdateStockRequestDto(
        string Symbol,
        string CompanyName,
        decimal PurchasePrice,
        int Quantity
    );
}