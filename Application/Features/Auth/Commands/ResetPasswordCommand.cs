using Application.Common.Interfaces;
using Application.Common.Result;
using Domain.Interfaces.User;
using MediatR;

namespace Application.Features.Auth.Commands;

public class ResetPasswordCommand : IRequest<Result>
{
    public string Token { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;

    public ResetPasswordCommandHandler(IUserRepository userRepository, IPasswordService passwordService)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
    }

    public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var users = (await _userRepository.GetByIdAsync(Guid.Empty)); // Placeholder - need to implement search by token
        // This is a simplified implementation - in production, you'd search by token

        return Result.Failure("Password reset token invalid or expired");
    }
}
