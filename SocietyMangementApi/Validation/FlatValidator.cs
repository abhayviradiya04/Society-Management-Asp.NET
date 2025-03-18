using FluentValidation;
using SocietyManagementApi.Model;

namespace SocietyManagementApi.Validation
{
    public class FlatValidator : AbstractValidator<FlatModel>
    {
        public FlatValidator()
        {
            RuleFor(f => f.FlatNumber)
                .NotEmpty().WithMessage("Flat Number can't be empty")
                .NotNull().WithMessage("Flat Number can't be null");

            RuleFor(f => f.FlatTypeID)
                .GreaterThan(0).WithMessage("Flat Type ID must be greater than 0");

            RuleFor(f => f.FloorNumber)
                .GreaterThanOrEqualTo(1).WithMessage("Floor Number must be greater than or equal to 1");

            RuleFor(f => f.Block)
                .NotEmpty().WithMessage("Block can't be empty")
                .NotNull().WithMessage("Block can't be null");
        }
    }
}
