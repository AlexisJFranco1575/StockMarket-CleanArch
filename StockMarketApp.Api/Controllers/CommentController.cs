using Microsoft.AspNetCore.Mvc;
using StockMarketApp.Application.DTOs;
using StockMarketApp.Domain.Entities;
using StockMarketApp.Domain.Interfaces;

namespace StockMarketApp.Api.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;

        public CommentController(ICommentRepository commentRepo, IStockRepository stockRepo)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _commentRepo.GetAllAsync();
            
            // Convertir a DTO
            var commentDtos = comments.Select(c => new CommentDto
            {
                Id = c.Id,
                Title = c.Title,
                Content = c.Content,
                CreatedOn = c.CreatedOn,
                StockId = c.StockId
            });

            return Ok(commentDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var comment = await _commentRepo.GetByIdAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return Ok(new CommentDto
            {
                Id = comment.Id,
                Title = comment.Title,
                Content = comment.Content,
                CreatedOn = comment.CreatedOn,
                StockId = comment.StockId
            });
        }

        [HttpPost("{stockId}")]
        public async Task<IActionResult> Create([FromRoute] int stockId, [FromBody] CreateCommentDto commentDto)
        {
            // 1. Validar que el Stock exista
            if (!await _stockRepo.StockExists(stockId))
            {
                return BadRequest("El Stock no existe");
            }

            // 2. Crear el modelo
            var commentModel = new Comment
            {
                Title = commentDto.Title,
                Content = commentDto.Content,
                StockId = stockId
            };

            await _commentRepo.CreateAsync(commentModel);

            // 3. Devolver DTO
            return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, new CommentDto
            {
                Id = commentModel.Id,
                Title = commentModel.Title,
                Content = commentModel.Content,
                CreatedOn = commentModel.CreatedOn,
                StockId = commentModel.StockId
            });
        }
    }
}