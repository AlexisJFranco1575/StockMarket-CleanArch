namespace StockMarketApp.Domain.Entities
{
    public class Stock
    {
        public int Id { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public decimal PurchasePrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public int Quantity { get; set; }
    }
}