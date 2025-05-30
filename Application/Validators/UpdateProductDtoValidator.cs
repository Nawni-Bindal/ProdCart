using Application.DTOs;
using FluentValidation;

namespace Application.Validators
{
    public class UpdateProductDtoValidator : AbstractValidator<UpdateProductDto>
    {
        public UpdateProductDtoValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.ProductName)
                .NotEmpty()
                .MaximumLength(100);
            RuleForEach(x => x.Items).SetValidator(new UpdateItemDtoValidator());
        }
    }

    public class UpdateItemDtoValidator : AbstractValidator<UpdateItemDto>
    {
        public UpdateItemDtoValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Quantity).GreaterThanOrEqualTo(0);
        }
    }
}