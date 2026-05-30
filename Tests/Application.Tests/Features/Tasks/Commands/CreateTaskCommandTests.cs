using Application.Common.Interfaces;
using Application.Common.Result;
using Application.Features.Tasks.Commands;
using Application.Features.Tasks.Validators;
using Domain.Entities.Task;
using Domain.Interfaces.Task;
using Domain.Interfaces.User;
using Moq;
using Xunit;

namespace Application.Tests.Features.Tasks.Commands;

public class CreateTaskCommandTests
{
    private readonly Mock<ITaskRepository> _mockTaskRepository;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly CreateTaskCommandHandler _handler;

    public CreateTaskCommandTests()
    {
        _mockTaskRepository = new Mock<ITaskRepository>();
        _mockUserRepository = new Mock<IUserRepository>();
        _handler = new CreateTaskCommandHandler(_mockTaskRepository.Object, _mockUserRepository.Object);
    }

    [Fact]
    public async Task Handle_WithValidInput_CreatesTaskSuccessfully()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new CreateTaskCommand
        {
            UserId = userId,
            Title = "Complete project",
            Description = "Finish the project deliverables",
            Difficulty = Domain.Enums.Difficulty.Hard,
            Importance = Domain.Enums.Importance.High,
            Duration = 120,
            StatCategory = Domain.Enums.StatCategory.Power,
            IsRequired = true
        };

        var user = new Domain.Entities.User.User
        {
            Id = userId,
            Email = "test@example.com",
            Username = "testuser"
        };

        _mockUserRepository.Setup(x => x.GetByIdAsync(userId))
            .ReturnsAsync(user);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        _mockTaskRepository.Verify(x => x.AddAsync(It.IsAny<Domain.Entities.Task.Task>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithInvalidUserId_ReturnsFailure()
    {
        // Arrange
        var command = new CreateTaskCommand
        {
            UserId = Guid.NewGuid(),
            Title = "Test task",
            Difficulty = Domain.Enums.Difficulty.Easy
        };

        _mockUserRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Domain.Entities.User.User)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains("User not found", result.Message);
    }

    [Fact]
    public async Task Handle_WithEmptyTitle_ReturnsFailure()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new CreateTaskCommand
        {
            UserId = userId,
            Title = "",
            Difficulty = Domain.Enums.Difficulty.Medium
        };

        var validator = new CreateTaskValidator();

        // Act
        var validationResult = await validator.ValidateAsync(command);

        // Assert
        Assert.False(validationResult.IsValid);
        Assert.Contains(validationResult.Errors, e => e.PropertyName == "Title");
    }

    [Fact]
    public async Task Handle_WithNullTitle_ReturnsFailure()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new CreateTaskCommand
        {
            UserId = userId,
            Title = null,
            Difficulty = Domain.Enums.Difficulty.Medium
        };

        var validator = new CreateTaskValidator();

        // Act
        var validationResult = await validator.ValidateAsync(command);

        // Assert
        Assert.False(validationResult.IsValid);
    }

    [Fact]
    public async Task Handle_WithVeryLongTitle_ReturnsFailure()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new CreateTaskCommand
        {
            UserId = userId,
            Title = new string('a', 256), // Assuming max length is 255
            Difficulty = Domain.Enums.Difficulty.Easy
        };

        var validator = new CreateTaskValidator();

        // Act
        var validationResult = await validator.ValidateAsync(command);

        // Assert
        Assert.False(validationResult.IsValid);
    }

    [Theory]
    [InlineData(Domain.Enums.Difficulty.Easy)]
    [InlineData(Domain.Enums.Difficulty.Medium)]
    [InlineData(Domain.Enums.Difficulty.Hard)]
    public async Task Handle_WithVariousDifficulties_CreatesSuccessfully(Domain.Enums.Difficulty difficulty)
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new CreateTaskCommand
        {
            UserId = userId,
            Title = "Test task",
            Difficulty = difficulty
        };

        var user = new Domain.Entities.User.User
        {
            Id = userId,
            Email = "test@example.com",
            Username = "testuser"
        };

        _mockUserRepository.Setup(x => x.GetByIdAsync(userId))
            .ReturnsAsync(user);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
    }
}
