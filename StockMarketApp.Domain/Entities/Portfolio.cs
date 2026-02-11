using System.ComponentModel.DataAnnotations.Schema;

namespace StockMarketApp.Domain.Entities
{
    [Table("Portfolios")] 
    public class Portfolio
    {
        // Clave Foránea 1: El Usuario
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        // Clave Foránea 2: La Acción
        public int StockId { get; set; }
        public Stock Stock { get; set; }
    }
}