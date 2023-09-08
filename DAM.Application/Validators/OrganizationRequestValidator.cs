using DAM.Application.DTO.Requests;
using FluentValidation;

namespace DAM.Application.Validators
{
    public class OrganizationRequestValidator : AbstractValidator<OrganizationRequest>
    {
        public OrganizationRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name field should not be empty")
                .NotNull().WithMessage("Name should not be null")
                .Matches("^[a-zA-Z0-9 ]*$").WithMessage("Name should be alphanumeric")
                .MaximumLength(50).WithMessage("Name should not be more than 50 characters")
                .MinimumLength(5).WithMessage("Name should be atleast 5 characters");

            RuleFor(x => x.IsActive)
                .NotNull().WithMessage("IsActive should not be null");

            RuleFor(x => x.AccessType)
                .NotNull().WithMessage("AccessType should not be null");
        }
    }
}
