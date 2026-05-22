using Application.DTOs.Auth;
using FluentValidation;

namespace Application.Features.Auth.Validators;

public class UpdateProfileValidator : AbstractValidator<UpdateProfileRequest>
{
    public UpdateProfileValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters")
            .MaximumLength(100).WithMessage("Username cannot exceed 100 characters");

        RuleFor(x => x.AvatarUrl)
            .MaximumLength(500).WithMessage("Avatar URL cannot exceed 500 characters");
    }
}
