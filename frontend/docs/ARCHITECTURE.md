# Frontend Architecture

## Overview
Simple React application with Material UI. No complex patterns, just clean and functional code.

## Tech Stack

- **React 18** - UI library
- **TypeScript** - Type safety
- **Material UI** - Component library
- **React Router** - Navigation
- **Axios** - HTTP client
- **React Hook Form** - Form handling
- **Vite** - Build tool

## Folder Structure

```
src/
├── components/          # Reusable UI components
│   ├── Layout.tsx      # App layout with header
│   ├── TaskList.tsx    # List of tasks
│   ├── TaskCard.tsx    # Single task display
│   ├── TaskDialog.tsx  # Create/edit task form
│   └── PrivateRoute.tsx # Protected route wrapper
├── pages/              # Page components
│   ├── LoginPage.tsx   # Login form
│   ├── RegisterPage.tsx # Registration form
│   └── DashboardPage.tsx # Main dashboard
├── services/           # API layer
│   ├── api.ts         # Axios setup
│   ├── auth.service.ts # Auth API calls
│   └── task.service.ts # Task API calls
├── context/            # React Context
│   └── AuthContext.tsx # Auth state
├── types/              # TypeScript types
│   └── api.types.ts   # API interfaces
├── utils/              # Utilities
│   └── constants.ts   # Constants
├── App.tsx            # Main app
└── main.tsx          # Entry point
```

## Component Architecture

### Pages
Entry points for routes. Handle data fetching and page-level state.

### Components
Reusable UI pieces. Receive props, render UI, emit events.

### Services
API communication. Axios calls with error handling.

### Context
Global state (auth only). Provider wraps app.

## Data Flow

1. User interacts with component
2. Component calls service method
3. Service makes API call
4. Response updates state
5. Component re-renders

Simple and direct - no complex state management needed.

## State Management

**Auth State:** React Context
- User info
- JWT token
- Login/logout methods

**Component State:** useState
- Form inputs
- Loading states
- Error messages
- UI toggles

**No Redux/Zustand** - not needed for this app size.

## API Integration

Base URL: `http://localhost:5000/api`

**Axios interceptor** adds JWT token to requests:
```typescript
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});
```

**Error handling:**
- 401 → redirect to login
- 400 → show validation errors
- 500 → show generic error

## Routing

```
/ → Login (public)
/register → Register (public)
/dashboard → Dashboard (protected)
```

Protected routes check for token and redirect if not authenticated.

## Security

- JWT stored in localStorage
- Token sent in Authorization header
- Cleared on logout
- Protected routes require authentication

## Styling

Material UI for everything:
- Consistent design system
- Responsive grid
- Theme customization
- sx prop for custom styles

Keep it simple - no custom CSS files.

## Form Handling

React Hook Form for validation:
- Built-in validation
- Error messages
- Easy integration with MUI
- Type-safe

## Error Handling

User-friendly messages:
- Network errors
- Validation errors
- Auth errors
- Generic errors

Show errors in Snackbar or inline in forms.

## Responsive Design

Mobile-first approach:
- MUI Grid system
- Breakpoints: xs, sm, md, lg
- Responsive spacing
- Mobile-friendly navigation

## Performance

Keep it simple:
- Small components
- Minimal re-renders
- Lazy load routes if needed
- No premature optimization

## Code Quality

Focus on:
- Readable code
- Proper TypeScript types
- Consistent formatting
- Clear component structure

Avoid:
- Complex abstractions
- Deep nesting
- God components
- Over-engineering

## Interview Focus

Demonstrate:
- Clean React code
- TypeScript usage
- API integration
- Responsive design
- Error handling
- User experience

Not needed:
- Complex state management
- Advanced patterns
- Performance optimization
- Testing (focus on working features)

## Development Approach

1. Setup project
2. Build API layer
3. Create auth flow
4. Build dashboard
5. Add CRUD operations
6. Polish UI
7. Test manually

Simple, incremental, working software.

