using StockMarketApp.Domain.Entities;

namespace StockMarketApp.Domain.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}