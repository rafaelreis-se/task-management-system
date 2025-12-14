# E2E Tests - Task Management System

End-to-end tests using [Cypress](https://www.cypress.io/) to validate the complete user flow.

## Quick Start

From the project root:

```bash
# First time - install all dependencies (includes e2e)
just setup

# Run all E2E tests
just e2e
```

The `just e2e` command will automatically:
1. Start test database (port 5433, isolated from dev)
2. Start backend API
3. Start frontend
4. Run all Cypress tests
5. Clean up everything after tests complete

## Structure

```
e2e/
├── cypress/
│   ├── e2e/                    # Test files
│   │   ├── auth.cy.ts          # Authentication tests
│   │   ├── tasks.cy.ts         # Task CRUD tests
│   │   └── full-flow.cy.ts     # Complete user journey
│   └── support/
│       ├── commands.ts         # Custom Cypress commands
│       └── e2e.ts              # Test setup
├── cypress.config.ts           # Cypress configuration
├── package.json
└── README.md
```

## Test Suites

### Authentication (`auth.cy.ts`)
- Display login form
- Login with valid credentials
- Show error with invalid credentials
- Navigate to register
- Register new user
- Password validation
- Logout

### Tasks (`tasks.cy.ts`)
- View dashboard with tasks
- Create new task
- Edit existing task
- Change task status
- Delete task
- Cancel deletion
- Filter by status

### Full Flow (`full-flow.cy.ts`)
Complete user journey:
1. Register new account
2. Create multiple tasks
3. Edit a task
4. Change task status
5. Delete a task
6. Logout
7. Login again
8. Verify data persisted

## Custom Commands

```typescript
// Login via UI
cy.login('john@example.com', 'TestPassword123');

// Login via API (faster, for setup)
cy.loginApi('john@example.com', 'TestPassword123');

// Register new user
cy.register('Test User', 'test@example.com', 'Password123');

// Logout
cy.logout();

// Create task via API
cy.createTaskApi('Task Title', 'Description');

// Clear test data (localStorage)
cy.clearTestData();
```

## Configuration

### URLs

| Service | URL |
|---------|-----|
| Frontend | http://localhost:5173 |
| Backend API | http://localhost:5001 |

### Cypress Config (`cypress.config.ts`)

```typescript
{
  viewportWidth: 1280,
  viewportHeight: 720,
  video: false,
  screenshotOnRunFailure: true,
  defaultCommandTimeout: 10000,
}
```

## Troubleshooting

### Tests fail to find elements

The tests use flexible selectors that work with:
- `data-testid` attributes
- Aria labels
- MUI component classes
- Button text content

### Database issues

If the test database has issues:

```bash
# Stop and remove test containers
cd backend && docker-compose -f docker-compose.test.yml down -v

# Try again
just e2e
```

### Port already in use

Check if port 5001 is already in use:

```bash
lsof -i :5001
just stop    # Stop all services
just e2e     # Try again
```

## Test Reports

Screenshots are saved on failure to: `cypress/screenshots/`

To enable video recording, update `cypress.config.ts`:

```typescript
video: true,
```

Videos will be saved to: `cypress/videos/`
