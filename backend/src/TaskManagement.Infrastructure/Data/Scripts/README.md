# Database Scripts

## Setup Instructions

### 1. Start PostgreSQL with Docker
```bash
docker-compose up -d postgres
```

### 2. Run Migration Scripts

Connect to the database and run scripts in order:

```bash
# Connect to database
docker exec -it taskmanagement-db psql -U postgres -d taskmanagement

# Or from your local machine if you have psql installed
psql -h localhost -U postgres -d taskmanagement
```

Then execute the scripts:

```sql
\i src/TaskManagement.Infrastructure/Data/Scripts/001_create_tables.sql
\i src/TaskManagement.Infrastructure/Data/Scripts/002_seed_data.sql
```

Or run them directly:

```bash
docker exec -i taskmanagement-db psql -U postgres -d taskmanagement < src/TaskManagement.Infrastructure/Data/Scripts/001_create_tables.sql
docker exec -i taskmanagement-db psql -U postgres -d taskmanagement < src/TaskManagement.Infrastructure/Data/Scripts/002_seed_data.sql
```

## Test Credentials

All test users have the same password: **TestPassword123**

- **John Doe**: john@example.com
- **Jane Smith**: jane@example.com
- **Bob Wilson**: bob@example.com

## Database Schema

### Users Table
- id (UUID, Primary Key)
- name (VARCHAR)
- email (VARCHAR, Unique)
- password_hash (VARCHAR)
- created_at (TIMESTAMP)

### Tasks Table
- id (UUID, Primary Key)
- user_id (UUID, Foreign Key to Users)
- title (VARCHAR)
- description (TEXT)
- status (VARCHAR: Pending, InProgress, Completed)
- due_date (TIMESTAMP)
- created_at (TIMESTAMP)
- updated_at (TIMESTAMP)

## Indexes

- users.email - for quick login lookups
- tasks.user_id - for efficient user task queries
- tasks.status - for status-based filtering

