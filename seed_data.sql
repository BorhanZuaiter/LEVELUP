-- Clear existing test data
DECLARE @UserId UNIQUEIDENTIFIER = 'D5C5F5F5-5F5F-5F5F-5F5F-5F5F5F5F5F5F';

DELETE FROM [Journal] WHERE [UserId] = @UserId;
DELETE FROM [Task] WHERE [UserId] = @UserId;
DELETE FROM [Stats] WHERE [UserId] = @UserId;
DELETE FROM [Users] WHERE [Id] = @UserId;

-- Insert test user
INSERT INTO [Users]
(
    [Id],
    [Username],
    [Email],
    [PasswordHash],
    [EmailVerified],
    [JoinDate],
    [UpdatedAt],
    [Level],
    [XP],
    [CurrentHP],
    [MaxHP],
    [CurrentStreak],
    [ShieldCount]
)
VALUES
(
    @UserId,
    'TestPlayer',
    'test@levelup.com',
    -- Password: Test@1234 (hashed with PBKDF2)
    '0x0100A0D920E83BA1E9E62FB936B1C2F0F0E0E0E0E0E0E0E0E0E0E0E0E0E0E0E0E0E0E0E0ECD0D30E0E0E0E0E0E0E0E0E0E0E0E0E0E0E0E0E0E0E0E0E0E0E0E0E0E0E0E0E0E0E0E0E0E0E0E',
    1,
    DATEADD(DAY, -30, GETUTCDATE()),
    GETUTCDATE(),
    5,
    250,
    85,
    100,
    7,
    0
);

-- Insert test tasks
INSERT INTO [Task]
(
    [Id],
    [UserId],
    [Title],
    [Description],
    [Difficulty],
    [Importance],
    [Priority],
    [DurationMinutes],
    [TaskColor],
    [StatCategory],
    [RecurrenceType],
    [TaskStatus],
    [CreatedAt],
    [UpdatedAt],
    [CompletedAt],
    [IsRequired],
    [ReminderEnabled],
    [ReminderTime]
)
VALUES
(
    NEWID(),
    @UserId,
    'Morning Meditation',
    'Meditate for 15 minutes',
    0, -- Easy
    2, -- High Importance
    2, -- High Priority
    15,
    '#FF6B6B',
    1, -- Wisdom
    0, -- Daily Recurrence
    1, -- Completed
    DATEADD(DAY, -5, GETUTCDATE()),
    GETUTCDATE(),
    DATEADD(HOUR, -2, GETUTCDATE()),
    1,
    1,
    '06:00:00'
),
(
    NEWID(),
    @UserId,
    'Complete coding project',
    'Finish the authentication module',
    2, -- Hard
    2, -- High Importance
    2, -- High Priority
    120,
    '#4ECDC4',
    3, -- Determination
    2, -- Once Recurrence
    0, -- Pending
    DATEADD(DAY, -3, GETUTCDATE()),
    GETUTCDATE(),
    NULL,
    0,
    1,
    '09:00:00'
),
(
    NEWID(),
    @UserId,
    'Exercise',
    'Go for a 30 minute run',
    1, -- Medium
    1, -- Medium Importance
    1, -- Medium Priority
    30,
    '#95E1D3',
    0, -- Power
    0, -- Daily Recurrence
    1, -- Completed
    DATEADD(DAY, -2, GETUTCDATE()),
    GETUTCDATE(),
    DATEADD(HOUR, -4, GETUTCDATE()),
    1,
    0,
    NULL
),
(
    NEWID(),
    @UserId,
    'Read a chapter',
    'Read Chapter 5 of Clean Code',
    0, -- Easy
    1, -- Medium Importance
    0, -- Low Priority
    45,
    '#F4A261',
    1, -- Wisdom
    1, -- Weekly Recurrence
    0, -- Pending
    DATEADD(DAY, -1, GETUTCDATE()),
    GETUTCDATE(),
    NULL,
    0,
    0,
    NULL
),
(
    NEWID(),
    @UserId,
    'Drink water',
    'Stay hydrated throughout the day',
    0, -- Easy
    0, -- Low Importance
    1, -- Medium Priority
    5,
    '#2A9D8F',
    2, -- Luck
    0, -- Daily Recurrence
    1, -- Completed
    GETUTCDATE(),
    GETUTCDATE(),
    DATEADD(HOUR, -1, GETUTCDATE()),
    1,
    1,
    '12:00:00'
);

-- Insert test journals
INSERT INTO [Journal]
(
    [Id],
    [UserId],
    [Title],
    [Content],
    [Category],
    [Date],
    [CreatedAt],
    [UpdatedAt]
)
VALUES
(
    NEWID(),
    @UserId,
    'Great Day!',
    'Had an amazing day! Completed all my tasks and felt very productive. The meditation session really helped calm my mind.',
    1, -- Positives
    DATEADD(DAY, -2, CAST(GETUTCDATE() AS DATE)),
    DATEADD(DAY, -2, GETUTCDATE()),
    DATEADD(DAY, -2, GETUTCDATE())
),
(
    NEWID(),
    @UserId,
    'Challenges today',
    'Found it hard to focus on coding today. Maybe I need more breaks during work sessions.',
    2, -- Negatives
    DATEADD(DAY, -1, CAST(GETUTCDATE() AS DATE)),
    DATEADD(DAY, -1, GETUTCDATE()),
    DATEADD(DAY, -1, GETUTCDATE())
),
(
    NEWID(),
    @UserId,
    'Learning Goals',
    'This month I want to: 1. Finish the authentication module 2. Learn advanced Flutter concepts 3. Improve my morning routine consistency 4. Build a habit of daily reflection',
    3, -- Goals
    DATEADD(DAY, -5, CAST(GETUTCDATE() AS DATE)),
    DATEADD(DAY, -5, GETUTCDATE()),
    DATEADD(DAY, -5, GETUTCDATE())
),
(
    NEWID(),
    @UserId,
    'Childhood Memory',
    'Remembered the time when I first learned to code. It was challenging but so rewarding. Now I''m building an app that could help millions!',
    0, -- Memories
    CAST(GETUTCDATE() AS DATE),
    GETUTCDATE(),
    GETUTCDATE()
);

PRINT 'Test data created successfully!';
PRINT 'User: test@levelup.com';
PRINT 'Password: Test@1234';
