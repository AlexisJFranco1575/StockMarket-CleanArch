using System.CodeDom.Compiler;
using System.ComponentModel.DataAnnotations;

namespace StockMarketApp.Application.DTOs
{
    public class UpdateStockRequestDto
    {
        [Required]
        [MaxLength(10, ErrorMessage = "El símbolo no puede tener más de 10 caracteres.")]
        public string Symbol {get; set; } = string.Empty;

        [Required]
        [MaxLength(100, ErrorMessage = "El nombre de acción no puede tener más de 100 caracteres.")]
        public string CompanyName {get; set; } =string.Empty;

        [Required]
        [Range(1, 1000000, ErrorMessage = "El precio debe estar entre 1 y 1.000.000")]
        public decimal PurchasePrice {get; set;}

        [Required]
        [Range(1, 1000, ErrorMessage = "La cantidad debe estar entre 1 y 1000")]
        public int Quantity {get; set;}
    }
}