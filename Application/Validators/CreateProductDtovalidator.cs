using Application.DTOs;
using FluentValidation;

namespace Application.Validators
{
    public class CreateProductDtovalidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductDtovalidator()
        {
            RuleFor(x => x.ProductName)
                .NotEmpty().WithMessage("Product Name is required.")
                .MaximumLength(255).WithMessage("Name must not exceed 100 characters.");            
            RuleFor(x => x.Items.Count)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");
            RuleFor(x => x.CreatedBy)
                .NotEmpty().WithMessage("CreatedBy is required.")
                .MaximumLength(100);

        }
    }
}
