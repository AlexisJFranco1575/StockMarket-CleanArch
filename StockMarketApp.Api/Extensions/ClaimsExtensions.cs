using System.Security.Claims;

namespace StockMarketApp.Api.Extensions
{
    public static class ClaimsExtensions
    {
        // Este método extiende la funcionalidad del objeto "User"
        public static string GetUsername(this ClaimsPrincipal user)
        {
            // Busca el Claim que tiene el nombre (GivenName) según nuestro TokenService
            return user.Claims.SingleOrDefault(x => x.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname")).Value;
        }
    }
}