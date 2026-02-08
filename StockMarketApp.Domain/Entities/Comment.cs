namespace StockMarketApp.Domain.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        // --- RELACIÓN (Foreign Key) ---
        public int? StockId { get; set; } 
        
        // Propiedad de Navegación (Para poder acceder al objeto Stock desde el comentario)
        public Stock? Stock { get; set; }
    }
}