# Task Management System - Frontend

Modern React frontend for the Task Management System, built with TypeScript and Material-UI.

## Tech Stack

- **React 18** - UI library
- **TypeScript** - Type safety
- **Vite** - Build tool and dev server
- **Material-UI (MUI)** - Component library
- **React Router** - Client-side routing
- **Axios** - HTTP client
- **React Hook Form** - Form handling and validation
- **date-fns** - Date formatting

## Project Structure

```
src/
├── components/          # Reusable UI components
│   ├── Layout.tsx       # App layout with header
│   ├── TaskList.tsx     # Task list container
│   ├── TaskCard.tsx     # Individual task card
│   ├── TaskDialog.tsx   # Create/edit task form
│   ├── DeleteConfirmDialog.tsx  # Delete confirmation
│   └── PrivateRoute.tsx # Route protection wrapper
├── pages/               # Page components
│   ├── LoginPage.tsx    # Login form
│   ├── RegisterPage.tsx # Registration form
│   └── DashboardPage.tsx # Main dashboard with tasks
├── services/            # API communication layer
│   ├── api.ts           # Axios configuration
│   ├── auth.service.ts  # Authentication API calls
│   └── task.service.ts  # Task CRUD API calls
├── context/             # React Context providers
│   └── AuthContext.tsx  # Authentication state management
├── types/               # TypeScript type definitions
│   └── api.types.ts     # API-related types
├── utils/               # Utility functions and constants
│   └── constants.ts     # App constants
├── App.tsx              # Main app with routing
└── main.tsx             # Entry point
```

## Design Decisions

### Architecture
- **Component-based architecture** for reusability and maintainability
- **Service layer** to separate API logic from UI
- **Context API** for global auth state only
- **Local component state** for UI state
- **TypeScript** for type safety

### State Management
- React Context for authentication state
- Local useState for component state
- No Redux/Zustand - not needed for this app size

### API Integration
- Axios with interceptors for token management
- Automatic token injection in headers
- Centralized error handling
- 401 handling with automatic logout

### Form Handling
- React Hook Form for efficient form management
- Built-in validation
- Type-safe forms with TypeScript

## Available Scripts

```bash
npm run dev      # Start development server
npm run build    # Build for production
npm run preview  # Preview production build
npm run lint     # Run ESLint
```

## Features

### Authentication
- User registration with validation
- Login with JWT authentication
- Protected routes
- Persistent auth (localStorage)
- Automatic logout on token expiration

### Task Management
- Create tasks with title, description, and due date
- View all tasks in responsive grid
- Edit task details and status
- Delete tasks with confirmation
- Filter by status (Pending, In Progress, Completed)
- Visual status indicators
- Overdue task highlighting

### User Experience
- Responsive design (mobile, tablet, desktop)
- Loading states
- Error handling with user-friendly messages
- Success notifications
- Form validation

## Documentation

- [Architecture](./docs/ARCHITECTURE.md)
- [API Integration](./docs/API_INTEGRATION.md)
