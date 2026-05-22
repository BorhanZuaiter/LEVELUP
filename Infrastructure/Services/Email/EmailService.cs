using Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services.Email;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendVerificationEmailAsync(string email, string token, string username)
    {
        var verificationLink = $"{_configuration["AppUrl"]}/verify-email?token={token}";
        var subject = "Verify your email - LevelUp";
        var body = $@"
            <h2>Welcome to LevelUp, {username}!</h2>
            <p>Click the link below to verify your email:</p>
            <a href=""{verificationLink}"">Verify Email</a>
            <p>This link expires in 24 hours.</p>";

        await SendEmailAsync(email, subject, body);
    }

    public async Task SendForgotPasswordEmailAsync(string email, string token, string username)
    {
        var resetLink = $"{_configuration["AppUrl"]}/reset-password?token={token}";
        var subject = "Reset your password - LevelUp";
        var body = $@"
            <h2>Password Reset Request</h2>
            <p>Click the link below to reset your password:</p>
            <a href=""{resetLink}"">Reset Password</a>
            <p>This link expires in 1 hour.</p>";

        await SendEmailAsync(email, subject, body);
    }

    private Task SendEmailAsync(string to, string subject, string body)
    {
        // Implementation depends on email provider (SendGrid, Mailgun, SMTP, etc)
        // For MVP, this is a placeholder that logs the email
        Console.WriteLine($"Email sent to {to}: {subject}");
        return Task.CompletedTask;
    }
}
