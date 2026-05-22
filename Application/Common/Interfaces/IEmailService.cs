namespace Application.Common.Interfaces;

public interface IEmailService
{
    Task SendVerificationEmailAsync(string email, string token, string username);
    Task SendForgotPasswordEmailAsync(string email, string token, string username);
}
