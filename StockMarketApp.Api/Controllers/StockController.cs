using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockMarketApp.Application.DTOs;
using StockMarketApp.Domain.Entities;
using StockMarketApp.Infrastructure.Persistence;

namespace StockMarketApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockController : ControllerBase
    {
        private readonly StockDbContext _context;

        //Inyectamos el contexto de la base de datos a través del constructor
        public StockController(StockDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stocks = await _context.Stocks.ToListAsync();
            return Ok(stocks);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequest request)
        {
            var newStock = new Stock
                {
                Symbol = request.Symbol.ToUpper(),
                CompanyName = request.CompanyName,
                PurchasePrice = request.PurchasePrice,
                CurrentPrice = request.PurchasePrice, 
                Quantity = request.Quantity
                };

            _context.Stocks.Add(newStock);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAll), new { id = newStock.Id }, newStock);
        }
    }
}