namespace StockMarketApp.Application.DTOs
{
    public class StockDto
    {
        public int Id { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public decimal PurchasePrice { get; set; }
        public decimal CurrentPrice { get; set; }
        
        public List<CommentDto> Comments { get; set; } 
    }
}