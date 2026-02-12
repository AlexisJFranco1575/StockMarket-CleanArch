using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StockMarketApp.Api.Extensions; // <--- Nuestra nueva herramienta
using StockMarketApp.Application.Mappers;
using StockMarketApp.Domain.Entities;
using StockMarketApp.Domain.Interfaces;

namespace StockMarketApp.Api.Controllers
{
    [Route("api/portfolio")]
    [ApiController]
    [Authorize] // <--- ¡Solo usuarios con Token pueden entrar aquí!
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepo;
        private readonly IPortfolioRepository _portfolioRepo;

        public PortfolioController(UserManager<AppUser> userManager, IStockRepository stockRepo, IPortfolioRepository portfolioRepo)
        {
            _userManager = userManager;
            _stockRepo = stockRepo;
            _portfolioRepo = portfolioRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserPortfolio()
        {
            // 1. Obtener el nombre del usuario desde el Token
            var username = User.GetUsername();
            
            // 2. Buscar al usuario completo en la BD
            var appUser = await _userManager.FindByNameAsync(username);

            // 3. Buscar su portafolio
            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);

            // 4. Convertir a DTO y devolver
            return Ok(userPortfolio.Select(s => s.ToStockDto()));
        }

        [HttpPost]
        public async Task<IActionResult> AddPortfolio(string symbol)
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var stock = await _stockRepo.GetBySymbolAsync(symbol);

            if (stock == null) return BadRequest("Stock no encontrado");

            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);

            // Validación si ya lo tiene agregado
            if (userPortfolio.Any(e => e.Symbol.ToLower() == symbol.ToLower())) 
                return BadRequest("Ya tienes este stock en tu portafolio");

            var portfolioModel = new Portfolio
            {
                StockId = stock.Id,
                AppUserId = appUser.Id
            };

            await _portfolioRepo.CreateAsync(portfolioModel);

            if (portfolioModel == null)
            {
                return StatusCode(500, "No se pudo crear");
            }

            return Created();
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePortfolio(string symbol)
        {
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);

            // Buscar el portafolio filtrando por usuario y símbolo
            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);

            // Filtrar para encontrar el elemento exacto
            var filteredStock = userPortfolio.Where(s => s.Symbol.ToLower() == symbol.ToLower()).ToList();

            if (filteredStock.Count() == 1)
            {
                await _portfolioRepo.DeletePortfolio(appUser, symbol);
            }
            else
            {
                return BadRequest("Stock no está en tu portafolio");
            }

            return Ok();
        }
    }
}