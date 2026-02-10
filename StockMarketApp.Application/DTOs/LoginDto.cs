using System.ComponentModel.DataAnnotations;

namespace StockMarketApp.Application.DTOs
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; } 

        [Required]
        public string Password { get; set; }
    }
}