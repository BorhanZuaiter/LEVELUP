using Application.Common.Interfaces;
using Application.Common.Result;
using Application.DTOs.Auth;
using Domain.Interfaces.User;
using MediatR;

namespace Application.Features.Auth.Commands;

public class UpdateProfileCommand : IRequest<Result>
{
    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
}

public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, Result>
{
    private readonly IUserRepository _userRepository;

    public UpdateProfileCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
            return Result.Failure("User not found");

        if (await _userRepository.UsernameExistsAsync(request.Username) && user.Username != request.Username)
            return Result.Failure("Username already taken");

        user.Username = request.Username;
        if (!string.IsNullOrEmpty(request.AvatarUrl))
            user.AvatarUrl = request.AvatarUrl;

        await _userRepository.UpdateAsync(user);

        return Result.Success("Profile updated successfully");
    }
}
