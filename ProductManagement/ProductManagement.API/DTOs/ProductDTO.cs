using FluentValidation;

namespace ProductManagement.API.DTOs
{
    public class ProductDTO
    {
        public string? Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string Supplier { get; set; } = null!;
        public bool Active { get; set; }
        public DateTime RegisteredAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }

    public class ProductDTOValidator : AbstractValidator<ProductDTO>
    {
        public ProductDTOValidator()
        {
            RuleFor(product => product.Name)
                .NotEmpty()
                .WithMessage("O Nome do produto é obrigatório e deve ser informado.");
            RuleFor(product => product.Price)
                .GreaterThan(0m)
                .WithMessage("O Preço do produto deve ser maior que 0.");
            RuleFor(product => product.Supplier)
                .NotEmpty()
                .WithMessage("O Fornecedor do produto é obrigatório e deve ser informado.");
        }
    }
}