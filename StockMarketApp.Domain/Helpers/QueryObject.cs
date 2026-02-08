namespace StockMarketApp.Application.Helpers
{
    public class QueryObject
    {
        public string? Symbol { get; set; } = null;
        public string? CompanyName { get; set; } = null;
        
        public string? SortBy { get; set; } = null; // Ej: "Symbol", "Price"
        public bool IsDescending { get; set; } = false;

        //Paginación
        public int PageNumber {get; set;} = 1;
        public int PageSize {get; set; } = 20;
    }
}