using StockMarketApp.Application.DTOs;
using StockMarketApp.Domain.Entities;

namespace StockMarketApp.Application.Mappers
{
    public static class CommentMapper
    {
        // 1. Método para TRADUCIR de Base de Datos -> DTO (Para mostrar al usuario)
        // El "this" hace que podamos usarlo como comment.ToCommentDto()
        public static CommentDto ToCommentDto(this Comment commentModel)
        {
            return new CommentDto
            {
                Id = commentModel.Id,
                Title = commentModel.Title,
                Content = commentModel.Content,
                CreatedOn = commentModel.CreatedOn,
                StockId = commentModel.StockId
            };
        }

        // 2. Método para TRADUCIR de DTO -> Base de Datos (Para guardar)
        // Necesitamos el stockId extra porque el DTO no lo trae
        public static Comment ToCommentFromCreate(this CreateCommentDto commentDto, int stockId)
        {
            return new Comment
            {
                Title = commentDto.Title,
                Content = commentDto.Content,
                StockId = stockId
            };
        }

public static Comment ToCommentFromUpdate(this UpdateCommentRequestDto commentDto)
{
    return new Comment
    {
        Title = commentDto.Title,
        Content = commentDto.Content
    };
}
    }
    
}