using Application.Features.Profile.Commands;
using FluentValidation;

namespace Application.Features.Profile.Validators;

public class DeleteAccountValidator : AbstractValidator<DeleteAccountCommand>
{
    public DeleteAccountValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required to delete account")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters");
    }
}
