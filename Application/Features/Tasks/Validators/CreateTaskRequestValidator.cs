using Application.DTOs.Tasks;
using FluentValidation;

namespace Application.Features.Tasks.Validators;

public class CreateTaskRequestValidator : AbstractValidator<CreateTaskRequest>
{
    public CreateTaskRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");

        RuleFor(x => x.Difficulty)
            .InclusiveBetween(0, 2).WithMessage("Difficulty must be between 0 and 2");

        RuleFor(x => x.Importance)
            .InclusiveBetween(0, 2).WithMessage("Importance must be between 0 and 2");

        RuleFor(x => x.Priority)
            .InclusiveBetween(0, 2).WithMessage("Priority must be between 0 and 2");

        RuleFor(x => x.DurationMinutes)
            .GreaterThan(0).WithMessage("Duration must be greater than 0");

        RuleFor(x => x.TaskColor)
            .NotEmpty().WithMessage("Task color is required")
            .MaximumLength(50).WithMessage("Task color cannot exceed 50 characters");

        RuleFor(x => x.StatCategory)
            .InclusiveBetween(0, 3).WithMessage("Stat category must be between 0 and 3");

        RuleFor(x => x.RecurrenceType)
            .InclusiveBetween(0, 4).WithMessage("Recurrence type must be between 0 and 4");
    }
}
