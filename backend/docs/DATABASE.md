# Database Schema

## Tables

### Users
```sql
CREATE TABLE Users (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    Email VARCHAR(255) UNIQUE NOT NULL,
    PasswordHash VARCHAR(255) NOT NULL,
    Name VARCHAR(100) NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
);
```

### Tasks
```sql
CREATE TABLE Tasks (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    Title VARCHAR(200) NOT NULL,
    Description TEXT,
    Status VARCHAR(50) NOT NULL CHECK (Status IN ('Pending', 'InProgress', 'Completed')),
    DueDate DATETIME NOT NULL,
    UserId UNIQUEIDENTIFIER NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME,
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);
```

## Relationships

- A User can have multiple Tasks (one-to-many)
- A Task belongs to exactly one User
- Deleting a User cascades to their Tasks

## Indexes

Recommended indexes for performance:
- Users: Index on Email (unique)
- Tasks: Index on UserId for efficient user-specific queries
- Tasks: Index on Status for filtering

## Migration Strategy

SQL scripts will be provided in the Infrastructure/Data/Scripts folder.
Run scripts manually in order for initial setup.

