using FluentValidation;
using StockMarketApp.Application.DTOs;

namespace StockMarketApp.Application.Validations
{
    public class CreateStockRequestValidator : AbstractValidator<CreateStockRequest>
    {
        public CreateStockRequestValidator()
        {
            RuleFor(x => x.Symbol)
                .NotEmpty().WithMessage("El símbolo es obligatorio")
                .Length(1, 5).WithMessage("El símbolo debe tener entre 1 y 5 caracteres (Ej: AAPL)");

            RuleFor(x => x.CompanyName)
                .NotEmpty().WithMessage("El nombre de la empresa es obligatorio")
                .MaximumLength(50).WithMessage("El nombre no puede exceder 50 caracteres");

            RuleFor(x => x.PurchasePrice)
                .GreaterThan(0).WithMessage("El precio de compra debe ser mayor a 0");

            RuleFor(x => x.Quantity)
                .GreaterThanOrEqualTo(1).WithMessage("Debes comprar al menos 1 acción")
                .LessThan(100000).WithMessage("No puedes comprar más de 100,000 acciones por transacción");
        }
    }
}