using Application.Common.Result;
using Domain.Interfaces.User;
using MediatR;

namespace Application.Features.Auth.Commands;

public class VerifyEmailCommand : IRequest<Result>
{
    public string Token { get; set; } = string.Empty;
}

public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, Result>
{
    private readonly IUserRepository _userRepository;

    public VerifyEmailCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
    {
        // In production, search user by EmailVerificationToken
        // This is simplified for MVP
        return Result.Failure("Verification token invalid or expired");
    }
}
