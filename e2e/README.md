# E2E Tests - Task Management System

End-to-end tests using [Cypress](https://www.cypress.io/) to validate the complete user flow.

## Quick Start

### Option 1: Full Automated Run (Recommended)

From the monorepo root:

```bash
# First time only - install dependencies
just e2e-install

# Run all E2E tests (starts db, backend, frontend automatically)
just e2e
```

This command will:
1. Start test database (reuses `backend/docker-compose.test.yml`, port 5433)
2. Database schema and seed data auto-loaded via docker volume
3. Start backend API (pointing to test database)
4. Start frontend
5. Run all Cypress tests
6. Clean up everything after tests complete

### Option 2: Interactive Mode

If you want to run tests interactively while developing:

```bash
# Terminal 1: Start the dev environment
just dev

# Terminal 2: Open Cypress UI
just e2e-open
```

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

# Database is managed by backend/docker-compose.test.yml (port 5433)
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

### Environment Variables

| Variable | Default | Description |
|----------|---------|-------------|
| `baseUrl` | `http://localhost:5173` | Frontend URL |
| `apiUrl` | `http://localhost:5000/api` | Backend API URL |

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

If tests fail, you may need to add `data-testid` attributes to your components:

```tsx
<IconButton data-testid="edit-button" onClick={handleEdit}>
  <EditIcon />
</IconButton>
```

### Database issues

If the test database has issues:

```bash
# Stop and remove test containers
cd backend && docker-compose -f docker-compose.test.yml down -v

# Try again
just e2e
```

### Backend won't start

Check if port 5000 is already in use:

```bash
lsof -i :5000
```

## Test Reports

Screenshots are saved on failure to: `cypress/screenshots/`

To enable video recording, update `cypress.config.ts`:

```typescript
video: true,
```

Videos will be saved to: `cypress/videos/`
