using Microsoft.AspNetCore.Mvc;
using StockMarketApp.Application.DTOs;
using StockMarketApp.Domain.Entities;
using StockMarketApp.Domain.Interfaces;
using StockMarketApp.Application.Helpers;

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
    // Pasamos el query al repositorio
    var stocks = await _stockRepo.GetAllAsync(query);
    
    // El mapeo a DTO sigue igual...
    var stockDtos = stocks.Select(s => new
    {
        s.Id,
        s.Symbol,
        s.CompanyName,
        s.PurchasePrice,
        s.CurrentPrice,
        Comments = s.Comments.Select(c => new 
        {
            c.Id,
            c.Title,
            c.Content,
            c.CreatedOn
        }).ToList()
    });

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

    // Mapeo individual
    return Ok(new 
    {
        stock.Id,
        stock.Symbol,
        stock.CompanyName,
        stock.PurchasePrice,
        stock.CurrentPrice,
        Comments = stock.Comments.Select(c => new 
        {
            c.Id,
            c.Title,
            c.Content,
            c.CreatedOn
        }).ToList()
    });
}

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequest stockDto)
        {
            var stockModel = new Stock
            {
                Symbol = stockDto.Symbol,
                CompanyName = stockDto.CompanyName,
                PurchasePrice = stockDto.PurchasePrice,
                CurrentPrice = stockDto.PurchasePrice,
                Quantity = stockDto.Quantity
            };

            await _stockRepo.CreateAsync(stockModel);

            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel);
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