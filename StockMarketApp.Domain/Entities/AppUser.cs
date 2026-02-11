using Microsoft.AspNetCore.Identity;

namespace StockMarketApp.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        // Relación: Un usuario tiene muchos items en su portafolio
    public List<Portfolio> Portfolios { get; set; } = new List<Portfolio>();
    }
}