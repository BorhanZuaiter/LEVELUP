using Application.Common.Interfaces;
using Application.Common.Result;
using Domain.Interfaces.User;
using MediatR;

namespace Application.Features.Auth.Commands;

public class ForgotPasswordCommand : IRequest<Result>
{
    public string Email { get; set; } = string.Empty;
}

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;

    public ForgotPasswordCommandHandler(IUserRepository userRepository, IEmailService emailService)
    {
        _userRepository = userRepository;
        _emailService = emailService;
    }

    public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null)
            return Result.Success("If email exists, a reset link has been sent");

        var token = Guid.NewGuid().ToString();
        user.ForgotPasswordToken = token;

        await _userRepository.UpdateAsync(user);
        await _emailService.SendForgotPasswordEmailAsync(user.Email, token, user.Username);

        return Result.Success("If email exists, a reset link has been sent");
    }
}
