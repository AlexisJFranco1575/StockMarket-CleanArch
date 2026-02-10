using Microsoft.AspNetCore.Mvc;
using StockMarketApp.Application.DTOs;
using StockMarketApp.Domain.Entities;
using StockMarketApp.Domain.Interfaces;
using StockMarketApp.Application.Helpers;
using StockMarketApp.Application.Mappers;

namespace StockMarketApp.Api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockRepository _stockRepo;

        //  Inyectamos el Repositorio, NO el DbContext
        public StockController(IStockRepository stockRepo)
        {
            _stockRepo = stockRepo;
        }

[HttpGet]
public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
{
    var stocks = await _stockRepo.GetAllAsync(query);
    
    // ¡Mira qué limpio! Una sola línea hace todo el trabajo sucio
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
public async Task<IActionResult> Create([FromBody] CreateStockRequest stockDto)
{
    // Convertimos DTO a Modelo usando el Mapper
    var stockModel = stockDto.ToStockFromCreateDTO();

    await _stockRepo.CreateAsync(stockModel);

    return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());
}


        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
        {
        
            var stockModel = new Stock
            {
                Symbol = updateDto.Symbol,
                CompanyName = updateDto.CompanyName,
                PurchasePrice = updateDto.PurchasePrice,
                CurrentPrice = updateDto.PurchasePrice, // Asumimos que al editar se resetea o mantiene
                Quantity = updateDto.Quantity
            };

            var stock = await _stockRepo.UpdateAsync(id, stockModel);

            if (stock == null)
            {
                return NotFound();
            }

            return Ok(new 
            {
                stock.Id,
                stock.Symbol,
                stock.CompanyName,
                stock.PurchasePrice
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var stock = await _stockRepo.DeleteAsync(id);

            if (stock == null)
            {
                return NotFound();
            }

            return NoContent(); // 204: Éxito pero no hay nada que devolver
        }
    }
}