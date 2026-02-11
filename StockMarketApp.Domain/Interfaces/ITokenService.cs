using StockMarketApp.Domain.Entities;

namespace StockMarketApp.Domain.Interfaces
{
    public interface ITokenService
    {
        // Añadimos "IList<string> roles" como parámetro
        string CreateToken(AppUser user, IList<string> roles);
    }
}