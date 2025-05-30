using Application.DTOs;
using FluentValidation;

namespace Application.Validators
{
    public class CreateItemDtoValidator : AbstractValidator<CreateItemDto>
    {
        public CreateItemDtoValidator()
        {
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0.");
        }
    }
}