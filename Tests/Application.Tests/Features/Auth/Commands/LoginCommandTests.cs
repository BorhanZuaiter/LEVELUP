using Application.Common.Interfaces;
using Application.Common.Result;
using Application.Features.Auth.Commands;
using Application.Features.Auth.Validators;
using Domain.Interfaces.User;
using Infrastructure.Services;
using Moq;
using Xunit;

namespace Application.Tests.Features.Auth.Commands;

public class LoginCommandTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IJwtTokenService> _mockJwtTokenService;
    private readonly IPasswordService _passwordService;
    private readonly LoginCommandHandler _handler;

    public LoginCommandTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockJwtTokenService = new Mock<IJwtTokenService>();
        _passwordService = new PasswordService();
        _handler = new LoginCommandHandler(
            _mockUserRepository.Object,
            _mockJwtTokenService.Object,
            _passwordService
        );
    }

    [Fact]
    public async Task Handle_WithValidCredentials_ReturnsAuthTokens()
    {
        // Arrange
        var email = "test@example.com";
        var password = "CorrectPassword@123";
        var passwordHash = _passwordService.HashPassword(password);

        var command = new LoginCommand
        {
            Email = email,
            Password = password
        };

        var user = new Domain.Entities.User.User
        {
            Id = Guid.NewGuid(),
            Email = email,
            Username = "testuser",
            PasswordHash = passwordHash
        };

        _mockUserRepository.Setup(x => x.GetByEmailAsync(email))
            .ReturnsAsync(user);

        _mockJwtTokenService.Setup(x => x.GenerateAccessToken(It.IsAny<Domain.Entities.User.User>()))
            .Returns("access_token");

        _mockJwtTokenService.Setup(x => x.GenerateRefreshToken())
            .Returns("refresh_token");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.NotNull(result.Data.AccessToken);
        Assert.NotNull(result.Data.RefreshToken);
    }

    [Fact]
    public async Task Handle_WithInvalidPassword_ReturnsFailure()
    {
        // Arrange
        var email = "test@example.com";
        var correctPassword = "CorrectPassword@123";
        var wrongPassword = "WrongPassword@123";
        var passwordHash = _passwordService.HashPassword(correctPassword);

        var command = new LoginCommand
        {
            Email = email,
            Password = wrongPassword
        };

        var user = new Domain.Entities.User.User
        {
            Id = Guid.NewGuid(),
            Email = email,
            Username = "testuser",
            PasswordHash = passwordHash
        };

        _mockUserRepository.Setup(x => x.GetByEmailAsync(email))
            .ReturnsAsync(user);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("Invalid", result.Message);
    }

    [Fact]
    public async Task Handle_WithNonExistentUser_ReturnsFailure()
    {
        // Arrange
        var command = new LoginCommand
        {
            Email = "nonexistent@example.com",
            Password = "SomePassword@123"
        };

        _mockUserRepository.Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((Domain.Entities.User.User)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("not found", result.Message.ToLower());
    }

    [Fact]
    public async Task Handle_WithEmptyEmail_FailsValidation()
    {
        // Arrange
        var command = new LoginCommand
        {
            Email = "",
            Password = "Password@123"
        };

        var validator = new LoginValidator();

        // Act
        var validationResult = await validator.ValidateAsync(command);

        // Assert
        Assert.False(validationResult.IsValid);
        Assert.Contains(validationResult.Errors, e => e.PropertyName == "Email");
    }

    [Fact]
    public async Task Handle_WithInvalidEmailFormat_FailsValidation()
    {
        // Arrange
        var command = new LoginCommand
        {
            Email = "invalid-email",
            Password = "Password@123"
        };

        var validator = new LoginValidator();

        // Act
        var validationResult = await validator.ValidateAsync(command);

        // Assert
        Assert.False(validationResult.IsValid);
        Assert.Contains(validationResult.Errors, e => e.PropertyName == "Email");
    }

    [Fact]
    public async Task Handle_WithEmptyPassword_FailsValidation()
    {
        // Arrange
        var command = new LoginCommand
        {
            Email = "test@example.com",
            Password = ""
        };

        var validator = new LoginValidator();

        // Act
        var validationResult = await validator.ValidateAsync(command);

        // Assert
        Assert.False(validationResult.IsValid);
        Assert.Contains(validationResult.Errors, e => e.PropertyName == "Password");
    }

    [Theory]
    [InlineData("user@example.com")]
    [InlineData("test.email@domain.co.uk")]
    [InlineData("firstname.lastname@company.net")]
    public async Task Handle_WithVariousValidEmails_ReturnsAuthTokens(string email)
    {
        // Arrange
        var password = "ValidPassword@123";
        var passwordHash = _passwordService.HashPassword(password);

        var command = new LoginCommand
        {
            Email = email,
            Password = password
        };

        var user = new Domain.Entities.User.User
        {
            Id = Guid.NewGuid(),
            Email = email,
            Username = "testuser",
            PasswordHash = passwordHash
        };

        _mockUserRepository.Setup(x => x.GetByEmailAsync(email))
            .ReturnsAsync(user);

        _mockJwtTokenService.Setup(x => x.GenerateAccessToken(It.IsAny<Domain.Entities.User.User>()))
            .Returns("access_token");

        _mockJwtTokenService.Setup(x => x.GenerateRefreshToken())
            .Returns("refresh_token");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
    }
}
