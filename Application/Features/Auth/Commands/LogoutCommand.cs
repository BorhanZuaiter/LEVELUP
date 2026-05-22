using Application.Common.Result;
using Domain.Interfaces.User;
using MediatR;

namespace Application.Features.Auth.Commands;

public class LogoutCommand : IRequest<Result>
{
    public Guid UserId { get; set; }
}

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result>
{
    private readonly IUserRepository _userRepository;

    public LogoutCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
            return Result.Failure("User not found");

        user.RefreshToken = null;
        user.RefreshTokenExpiry = null;

        await _userRepository.UpdateAsync(user);

        return Result.Success("Logged out successfully");
    }
}
