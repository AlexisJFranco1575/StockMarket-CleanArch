using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using StockMarketApp.Application.DTOs;
using StockMarketApp.Application.Mappers;
using StockMarketApp.Domain.Entities;
using StockMarketApp.Domain.Interfaces;
using StockMarketApp.Domain;

namespace StockMarketApp.Api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockRepository _stockRepo;

        public StockController(IStockRepository stockRepo)
        {
            _stockRepo = stockRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
            var stocks = await _stockRepo.GetAllAsync(query);
            var stockDtos = stocks.Select(s => s.ToStockDto());
            return Ok(stockDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var stock = await _stockRepo.GetByIdAsync(id);

            if (stock == null)
            {
                return NotFound();
            }

            return Ok(stock.ToStockDto());
        }

        [HttpPost]
        [Authorize] // 🔒 Protegido
        public async Task<IActionResult> Create([FromBody] CreateStockRequest stockDto)
        {
            var stockModel = stockDto.ToStockFromCreateDTO();
            await _stockRepo.CreateAsync(stockModel);
            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());
        }

        [HttpPut("{id}")]
        [Authorize] // 🔒 Protegido
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
        {
            // Mapeo manual (está bien por ahora, luego podríamos hacer un Mapper para Update)
            var stockModel = new Stock
            {
                Symbol = updateDto.Symbol,
                CompanyName = updateDto.CompanyName,
                PurchasePrice = updateDto.PurchasePrice,
                CurrentPrice = updateDto.PurchasePrice,
                Quantity = updateDto.Quantity
            };

            var stock = await _stockRepo.UpdateAsync(id, stockModel);

            if (stock == null)
            {
                return NotFound();
            }

            // MEJORA: Devolvemos el DTO completo usando el Mapper, igual que en GetById
            return Ok(stock.ToStockDto()); 
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // Protegido
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var stock = await _stockRepo.DeleteAsync(id);

            if (stock == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}