# Testing Strategy for Listy Backend

**Created:** 2026-05-30  
**Target Coverage:** 70%+ overall, 80%+ critical paths  
**Test Framework:** xUnit + Moq

---

## Overview

This document outlines the comprehensive testing strategy for Listy backend to ensure code quality, security, and reliability before production release.

---

## Testing Pyramid

```
        /\
       /  \  E2E/Integration Tests
      /____\  (20%)
     /      \
    /        \ Integration Tests  
   /          \ (30%)
  /____________\
 /              \
/________________\ Unit Tests (50%)
```

**Distribution:**
- Unit Tests: 50% (fast feedback, high coverage)
- Integration Tests: 30% (database, API endpoints)
- E2E Tests: 20% (full user flows)

---

## Test Structure

### Directory Structure
```
backend/
├── Domain/
├── Application/
├── Infrastructure/
├── API/
└── Tests/
    ├── Domain.Tests/
    ├── Application.Tests/
    ├── Infrastructure.Tests/
    └── API.Tests/
```

### Test File Naming
```
[FeatureName][CommandOrQuery]Tests.cs
Example: CreateTaskCommandTests.cs
```

### Test Method Naming
```
[MethodUnderTest]_[Scenario]_[ExpectedResult]
Example: CreateTask_WithValidInput_ReturnsSuccess
```

---

## Unit Testing Strategy

### Domain Layer Tests (High Priority)

**Entities to Test:**
- User entity
- Task entity
- Journal entity
- Stats entity
- Validation rules

**Test Coverage Goals:**
- ✅ 80%+ coverage
- ✅ All business logic
- ✅ All validation rules
- ✅ Edge cases

**Example Test:**
```csharp
[Fact]
public void CreateTask_WithValidTitle_ReturnsTaskWithId()
{
    // Arrange
    var title = "Complete project";
    var difficulty = Difficulty.Medium;
    
    // Act
    var task = new Task(title, difficulty);
    
    // Assert
    Assert.NotEqual(Guid.Empty, task.Id);
    Assert.Equal(title, task.Title);
    Assert.Equal(difficulty, task.Difficulty);
}
```

### Application Layer Tests (High Priority)

**Command Handlers to Test:**
- RegisterCommand
- LoginCommand
- CreateTaskCommand
- CompleteTaskCommand
- CreateJournalCommand
- DeleteAccountCommand
- ... all other commands

**Query Handlers to Test:**
- GetTasksQuery
- GetJournalEntriesQuery
- GetCurrentUserQuery
- GetStatsQuery
- ... all other queries

**Test Coverage Goals:**
- ✅ 80%+ coverage
- ✅ All happy paths
- ✅ All error scenarios
- ✅ Validation failures
- ✅ Edge cases

**Example Test:**
```csharp
[Fact]
public async Task CreateTaskCommand_WithValidInput_ReturnsSuccess()
{
    // Arrange
    var command = new CreateTaskCommand
    {
        UserId = Guid.NewGuid(),
        Title = "Test task",
        Difficulty = Difficulty.Medium
    };
    
    var mockRepository = new Mock<ITaskRepository>();
    var handler = new CreateTaskCommandHandler(mockRepository.Object);
    
    // Act
    var result = await handler.Handle(command, CancellationToken.None);
    
    // Assert
    Assert.True(result.IsSuccess);
    mockRepository.Verify(x => x.AddAsync(It.IsAny<Task>()), Times.Once);
}
```

### Infrastructure Layer Tests (Medium Priority)

**Repository Tests:**
- UserRepository CRUD operations
- TaskRepository CRUD operations
- JournalRepository CRUD operations
- StatsRepository CRUD operations

**Service Tests:**
- JwtTokenService token generation/validation
- PasswordService hashing/verification
- EmailService (mock)
- AvatarService file validation

**Test Coverage Goals:**
- ✅ 60%+ coverage
- ✅ Critical paths
- ✅ Error handling
- ✅ Database operations

---

## Integration Testing Strategy

### API Endpoint Tests

**Authentication Endpoints:**
- POST /api/auth/register
- POST /api/auth/login
- POST /api/auth/logout
- POST /api/auth/refresh
- POST /api/auth/forgot-password
- POST /api/auth/reset-password

**Task Endpoints:**
- POST /api/tasks/create
- GET /api/tasks/get-all
- PUT /api/tasks/{id}
- DELETE /api/tasks/{id}
- POST /api/tasks/{id}/complete

**Journal Endpoints:**
- POST /api/journals/create
- GET /api/journals/get-all
- PUT /api/journals/{id}
- DELETE /api/journals/{id}

**Profile Endpoints:**
- PUT /api/profile
- POST /api/profile/avatar
- DELETE /api/profile/account

**Test Coverage Goals:**
- ✅ All endpoints tested
- ✅ Success and failure cases
- ✅ Authentication/authorization
- ✅ Error response formats
- ✅ Database state verification

### Example Integration Test:
```csharp
[Fact]
public async Task CreateTask_WithValidData_CreatesTaskInDatabase()
{
    // Arrange
    using var dbContext = new LevelUpDbContext(_options);
    var repository = new TaskRepository(dbContext);
    var handler = new CreateTaskCommandHandler(repository);
    
    var userId = Guid.NewGuid();
    var command = new CreateTaskCommand
    {
        UserId = userId,
        Title = "Integration test task",
        Difficulty = Difficulty.Hard
    };
    
    // Act
    var result = await handler.Handle(command, CancellationToken.None);
    
    // Assert
    Assert.True(result.IsSuccess);
    var savedTask = await dbContext.Tasks
        .FirstOrDefaultAsync(t => t.UserId == userId);
    Assert.NotNull(savedTask);
    Assert.Equal("Integration test task", savedTask.Title);
}
```

---

## Security Testing

### Security Test Cases

**Authentication Security:**
- [ ] Login with invalid password fails
- [ ] Login attempt rate limiting works
- [ ] JWT token expiration enforced
- [ ] Refresh token rotation works
- [ ] Logout invalidates tokens

**Authorization Security:**
- [ ] Users cannot access other users' data
- [ ] Users cannot modify other users' tasks
- [ ] Users cannot delete other users' journals
- [ ] Admin features require admin role (if applicable)

**Data Security:**
- [ ] Passwords are hashed, not stored plaintext
- [ ] Passwords are never logged
- [ ] Sensitive data not in error messages
- [ ] SQL injection prevention works
- [ ] XSS prevention works (API)

**Input Validation:**
- [ ] SQL injection attempts blocked
- [ ] XSS payloads rejected
- [ ] File upload validation works
- [ ] Size limits enforced
- [ ] Type validation strict

**Example Security Test:**
```csharp
[Fact]
public async Task Login_WithWrongPassword_ReturnsFailed()
{
    // Arrange
    var email = "test@example.com";
    var correctPassword = "CorrectPassword@123";
    var wrongPassword = "WrongPassword@123";
    
    // Create user with correct password
    var user = new User { Email = email, PasswordHash = HashPassword(correctPassword) };
    var repository = new Mock<IUserRepository>();
    repository.Setup(x => x.GetByEmailAsync(email)).ReturnsAsync(user);
    
    var passwordService = new PasswordService();
    var handler = new LoginCommandHandler(repository.Object, passwordService, ...);
    
    var command = new LoginCommand { Email = email, Password = wrongPassword };
    
    // Act
    var result = await handler.Handle(command, CancellationToken.None);
    
    // Assert
    Assert.False(result.IsSuccess);
    Assert.Contains("Invalid credentials", result.Message);
}
```

---

## Performance Testing

### Load Testing Scenarios

**Baseline Performance Targets:**
- Authentication endpoint: <200ms p95
- Task creation: <300ms p95
- Task listing: <500ms p95
- Journal creation: <300ms p95
- Database query: <100ms average

**Load Testing Setup:**
```bash
# Using Apache JMeter or similar
- Concurrent users: 100, 500, 1000
- Ramp-up time: 1 minute
- Test duration: 5 minutes per scenario
- Success rate: ≥99%
- Response time p95: <1 second
```

**Key Metrics:**
- Throughput (requests/second)
- Response time (average, min, max, p95, p99)
- Error rate
- CPU usage
- Memory usage
- Database connections

**Example JMeter Test Plan:**
```yaml
Thread Group:
  Number of Threads: 100
  Ramp-up Period: 60 seconds
  Loop Count: 10

Requests:
  - POST /api/auth/login (every thread)
  - GET /api/tasks/get-all (every thread)
  - POST /api/tasks/create (every other thread)
  - POST /api/journals/create (25% of threads)

Assertions:
  - Response code = 200 or 201
  - Response time < 1000ms
  - Content contains expected data
```

---

## Test Data Strategy

### Test Fixtures

**Fixture Examples:**
```csharp
public class TestDataBuilder
{
    public static User CreateTestUser(string email = "test@example.com")
    {
        return new User 
        { 
            Id = Guid.NewGuid(),
            Email = email,
            Username = "testuser",
            PasswordHash = HashPassword("Password@123")
        };
    }
    
    public static Task CreateTestTask(Guid userId, Difficulty difficulty = Difficulty.Medium)
    {
        return new Task
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Title = "Test task",
            Difficulty = difficulty,
            TaskStatus = TaskStatus.Pending
        };
    }
}
```

### Database Fixtures

**In-Memory Database for Unit/Integration Tests:**
```csharp
public class DatabaseFixture : IAsyncLifetime
{
    private const string ConnectionString = 
        "Server=(localdb)\\mssqllocaldb;Database=TestLevelUpDb;Integrated Security=true;";
    
    public DbContextOptions<LevelUpDbContext> Options { get; private set; }
    
    public async Task InitializeAsync()
    {
        var options = new DbContextOptionsBuilder<LevelUpDbContext>()
            .UseSqlServer(ConnectionString)
            .Options;
        
        using var context = new LevelUpDbContext(options);
        await context.Database.EnsureDeletedAsync();
        await context.Database.MigrateAsync();
        
        Options = options;
    }
    
    public async Task DisposeAsync()
    {
        using var context = new LevelUpDbContext(Options);
        await context.Database.EnsureDeletedAsync();
    }
}
```

---

## Continuous Integration Testing

### CI/CD Pipeline Tests

**On Every Commit:**
- [ ] Build succeeds
- [ ] All unit tests pass
- [ ] Code analysis passes
- [ ] No security vulnerabilities

**On Pull Requests:**
- [ ] All tests from commit pass
- [ ] Integration tests pass
- [ ] Code coverage doesn't decrease
- [ ] Performance benchmarks met

**Before Release:**
- [ ] All tests pass
- [ ] Security audit passed
- [ ] Load testing passed
- [ ] Manual UAT passed

### GitHub Actions Example:
```yaml
name: Tests

on: [push, pull_request]

jobs:
  test:
    runs-on: ubuntu-latest
    
    steps:
      - uses: actions/checkout@v2
      - uses: actions/setup-dotnet@v1
        with:
          dotnet-version: '10.0.x'
      
      - name: Restore
        run: dotnet restore
      
      - name: Build
        run: dotnet build
      
      - name: Unit Tests
        run: dotnet test --filter Category=Unit
      
      - name: Integration Tests
        run: dotnet test --filter Category=Integration
      
      - name: Code Coverage
        run: dotnet test /p:CollectCoverage=true
      
      - name: Security Scan
        run: dotnet list package --vulnerable
```

---

## Test Coverage Goals

### By Layer

| Layer | Target | Priority |
|-------|--------|----------|
| Domain | 80%+ | High |
| Application | 80%+ | High |
| Infrastructure | 60%+ | Medium |
| API | 70%+ | Medium |
| **Overall** | **70%+** | **High** |

### By Type

| Type | Target | Priority |
|------|--------|----------|
| Unit | 50% | High |
| Integration | 30% | High |
| E2E | 20% | Medium |

---

## Testing Checklist

### Unit Tests
- [ ] Domain layer tests written
- [ ] Application layer tests written
- [ ] Infrastructure layer tests written
- [ ] All critical paths covered
- [ ] Edge cases tested
- [ ] Error scenarios tested
- [ ] Coverage > 70%

### Integration Tests
- [ ] Database fixture created
- [ ] API endpoint tests created
- [ ] Authentication flow tested
- [ ] Authorization tested
- [ ] Error responses tested
- [ ] Database state verified
- [ ] All endpoints covered

### Security Tests
- [ ] Authentication security tested
- [ ] Authorization security tested
- [ ] Input validation tested
- [ ] SQL injection prevention verified
- [ ] Password security verified
- [ ] Token security verified
- [ ] XSS prevention verified

### Performance Tests
- [ ] Load testing completed
- [ ] Performance baselines established
- [ ] Response times acceptable
- [ ] Error rates acceptable
- [ ] Resource usage acceptable
- [ ] Scaling characteristics known

---

## Test Execution

### Running Tests Locally

```bash
# All tests
dotnet test

# Unit tests only
dotnet test --filter Category=Unit

# Integration tests only
dotnet test --filter Category=Integration

# Specific test class
dotnet test --filter FullyQualifiedName~CreateTaskCommandTests

# With coverage
dotnet test /p:CollectCoverage=true /p:CoverageFormat=cobertura

# With verbose output
dotnet test --verbosity normal
```

---

## Success Criteria

✅ **Phase 3 Testing is complete when:**
- [ ] All unit tests written (>70% coverage)
- [ ] All integration tests written
- [ ] Security audit passed
- [ ] Load testing completed
- [ ] Performance benchmarks met
- [ ] CI/CD pipeline configured
- [ ] All tests passing on main branch

---

## Timeline

| Task | Time | Status |
|------|------|--------|
| Unit Tests | 15-20 hours | ⏳ In progress |
| Integration Tests | 10-15 hours | ⏳ Next |
| Security Audit | 8-10 hours | ⏳ Next |
| Load Testing | 4-6 hours | ⏳ Next |
| **Total** | **37-51 hours** | **⏳** |

---

**Status:** Testing Strategy Complete  
**Next:** Implement unit tests for domain and application layers
