using Microsoft.AspNetCore.Mvc;
using StockMarketApp.Application.DTOs;
using StockMarketApp.Application.Mappers; // <--- Importante: Traemos nuestros traductores
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
            var commentDtos = comments.Select(c => c.ToCommentDto());

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

            return Ok(comment.ToCommentDto());
        }

        [HttpPost("{stockId}")]
        public async Task<IActionResult> Create([FromRoute] int stockId, [FromBody] CreateCommentDto commentDto)
        {
            if (!await _stockRepo.StockExists(stockId))
            {
                return BadRequest("El Stock no existe");
            }
            
            var commentModel = commentDto.ToCommentFromCreate(stockId);

            await _commentRepo.CreateAsync(commentModel);

            // Devolvemos el resultado traducido a DTO
            return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.ToCommentDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDto updateDto)
        {
            // 1. Convertir DTO a Entidad
            var commentModel = updateDto.ToCommentFromUpdate();

            // 2. Intentar actualizar en BD
            var comment = await _commentRepo.UpdateAsync(id, commentModel);

            if (comment == null)
            {
                return NotFound("Comentario no encontrado");
            }

            // 3. Devolver el resultado convertido a DTO
            return Ok(comment.ToCommentDto());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var comment = await _commentRepo.DeleteAsync(id);

            if (comment == null)
            {
                return NotFound("Comentario no encontrado");
            }

            return Ok(comment);
        }
    }
}