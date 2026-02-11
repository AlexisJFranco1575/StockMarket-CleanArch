namespace StockMarketApp.Application.DTOs
{
    public class CreateStockRequest
    {
        public string Symbol { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public decimal PurchasePrice { get; set; }
        public int Quantity { get; set; }
    }
}