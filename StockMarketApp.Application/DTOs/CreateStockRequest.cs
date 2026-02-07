namespace StockMarketApp.Application.DTOs
{
    public record CreateStockRequest(
        string Symbol,
        string CompanyName,
        decimal PurchasePrice,
        int Quantity
    );
}