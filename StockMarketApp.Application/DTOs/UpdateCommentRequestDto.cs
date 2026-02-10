using System.ComponentModel.DataAnnotations;

namespace StockMarketApp.Application.DTOs
{
    public class UpdateCommentRequestDto
    {
        [Required]
        [MinLength(5, ErrorMessage = "El título debe tener 5 caracteres")]
        [MaxLength(280, ErrorMessage = "El título no puede superar los 280 caracteres")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MinLength(5, ErrorMessage = "El contenido debe tener 5 caracteres")]
        [MaxLength(280, ErrorMessage = "El contenido no puede superar los 280 caracteres")]
        public string Content { get; set; } = string.Empty;
    }
}