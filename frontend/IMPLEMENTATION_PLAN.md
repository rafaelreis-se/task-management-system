# Frontend Implementation Plan

## Overview
Simple React frontend with Material UI for task management.
Focus on clean code and working features, not over-engineering.

## Phase 1: Project Setup (15 min)

1. Create React app with Vite + TypeScript
2. Install dependencies:
   - Material UI
   - React Router
   - Axios
   - React Hook Form
3. Setup folder structure
4. Configure ESLint + Prettier

## Phase 2: API Service Layer (20 min)

1. Create API service with Axios
2. Setup base URL and interceptors
3. Add JWT token handling
4. Create auth service (login, register)
5. Create task service (CRUD)

**Files:**
- `src/services/api.ts`
- `src/services/auth.service.ts`
- `src/services/task.service.ts`
- `src/types/api.types.ts`

## Phase 3: Authentication (30 min)

1. Create Auth Context
2. Build Login page
3. Build Register page
4. Add form validation
5. Handle API errors
6. Store JWT token
7. Add protected routes

**Files:**
- `src/context/AuthContext.tsx`
- `src/pages/LoginPage.tsx`
- `src/pages/RegisterPage.tsx`
- `src/components/PrivateRoute.tsx`

## Phase 4: Dashboard & Tasks (45 min)

1. Create Dashboard layout
2. Build task list component
3. Add create task dialog
4. Add edit task dialog
5. Add delete confirmation
6. Handle task status changes
7. Add loading states

**Files:**
- `src/pages/DashboardPage.tsx`
- `src/components/TaskList.tsx`
- `src/components/TaskCard.tsx`
- `src/components/TaskDialog.tsx`
- `src/components/Layout.tsx`

## Phase 5: Polish & Testing (30 min)

1. Make responsive
2. Add error handling
3. Add loading indicators
4. Test all flows
5. Fix any bugs
6. Add README

## Total Time: ~2.5 hours

## Key Features

Must have:
- Login and registration
- Create, read, update, delete tasks
- Task status (Pending, InProgress, Completed)
- Logout functionality
- Responsive design
- Error handling

Nice to have:
- Filter tasks by status
- Sort by due date
- Search tasks

## API Compatibility

Login Request:
```json
{ "email": "john@example.com", "password": "TestPassword123" }
```

Login Response:
```json
{
  "token": "jwt_token_here",
  "user": { "id": "guid", "name": "John", "email": "john@example.com" }
}
```

Task Response:
```json
{
  "id": "guid",
  "title": "Task title",
  "description": "Optional description",
  "status": "Pending",
  "dueDate": "2024-12-20T00:00:00Z",
  "createdAt": "2024-12-13T10:00:00Z",
  "updatedAt": null
}
```

## Design Decisions

Simple approach:
- MUI for consistent look
- React Context only for auth
- Local state for UI
- No complex routing
- No complex state management

Focus on:
- Clean code
- Working features
- Good UX
- Responsive layout
- Error handling

Avoid:
- Over-engineering
- Complex patterns
- Unnecessary abstractions
- Too many dependencies

## Testing Strategy

Manual testing of:
- Login with valid/invalid credentials
- Register new user
- Create task
- Edit task
- Delete task
- Change task status
- Logout and login again
- Responsive on mobile/tablet


