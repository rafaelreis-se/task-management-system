# Task Management System - Frontend

Modern React frontend for the Task Management System, built with TypeScript and Material-UI.

##  Tech Stack

- **React 18** - Modern UI library
- **TypeScript** - Type safety and better developer experience
- **Vite** - Fast build tool and development server
- **Material-UI (MUI)** - Professional component library
- **React Router** - Client-side routing
- **Axios** - HTTP client for API communication
- **React Hook Form** - Efficient form handling and validation
- **date-fns** - Date formatting utilities

##  Features

### Authentication
-  User registration with validation
-  User login with JWT authentication
-  Protected routes
-  Persistent authentication (localStorage)
-  Automatic logout on token expiration

### Task Management
-  Create tasks with title, description, and due date
-  View all tasks in a responsive grid
-  Edit task details and status
-  Delete tasks with confirmation
-  Filter tasks by status (Pending, In Progress, Completed)
-  Visual status indicators with color coding
-  Overdue task highlighting
-  Task count by status

### User Experience
-  Responsive design (mobile, tablet, desktop)
-  Loading states and indicators
-  Error handling with user-friendly messages
-  Success notifications
-  Form validation with helpful error messages
-  Intuitive navigation
-  Material Design principles

##  Setup Instructions

### Prerequisites

- Node.js 18+ and npm/yarn
- Backend API running on `http://localhost:5000`

### Installation

1. **Navigate to frontend directory**
```bash
cd frontend
```

2. **Install dependencies**
```bash
npm install
```

3. **Configure environment (optional)**

Create a `.env` file if you need to change the API URL:
```bash
VITE_API_URL=http://localhost:5000/api
```

4. **Start development server**
```bash
npm run dev
```

The application will be available at `http://localhost:5173`

### Build for Production

```bash
npm run build
```

The built files will be in the `dist/` directory.

### Preview Production Build

```bash
npm run preview
```

##  Project Structure

```
src/
├── components/          # Reusable UI components
│   ├── Layout.tsx      # App layout with header and footer
│   ├── TaskList.tsx    # Task list container
│   ├── TaskCard.tsx    # Individual task card
│   ├── TaskDialog.tsx  # Create/edit task form
│   ├── DeleteConfirmDialog.tsx  # Delete confirmation
│   └── PrivateRoute.tsx # Route protection wrapper
├── pages/              # Page components
│   ├── LoginPage.tsx   # Login form
│   ├── RegisterPage.tsx # Registration form
│   └── DashboardPage.tsx # Main dashboard with tasks
├── services/           # API communication layer
│   ├── api.ts         # Axios configuration
│   ├── auth.service.ts # Authentication API calls
│   └── task.service.ts # Task CRUD API calls
├── context/            # React Context providers
│   └── AuthContext.tsx # Authentication state management
├── types/              # TypeScript type definitions
│   └── api.types.ts   # API-related types
├── utils/              # Utility functions and constants
│   └── constants.ts   # App constants
├── App.tsx            # Main app with routing
└── main.tsx          # Entry point
```

##  Design Decisions

### Architecture
- **Component-based architecture** for reusability and maintainability
- **Service layer** to separate API logic from UI
- **Context API** for simple global state (authentication only)
- **Local component state** for UI state management
- **TypeScript** for type safety and better IDE support

### Styling
- Material-UI for consistent, professional design
- Responsive grid system
- Custom theming
- No custom CSS files (sx prop for customization)

### State Management
- React Context for authentication state
- Local useState for component state
- No complex state management library (Redux, Zustand) - not needed for this app size

### API Integration
- Axios with interceptors for token management
- Automatic token injection in headers
- Centralized error handling
- 401 handling with automatic logout

### Form Handling
- React Hook Form for efficient form management
- Built-in validation
- Type-safe forms with TypeScript
- User-friendly error messages

##  Demo Credentials

Use these credentials to test the application:

```
Email: john@example.com
Password: TestPassword123
```

Or create a new account using the registration page.

## 📱 Responsive Design

The application is fully responsive and works on:
- 📱 Mobile devices (xs: 320px+)
- 📱 Tablets (sm: 600px+)
-  Desktop (md: 900px+, lg: 1200px+)

## 🔒 Security

- JWT token stored in localStorage
- Tokens sent via Authorization header
- Protected routes require authentication
- Automatic logout on token expiration
- Password validation requirements
- XSS protection via React

##  Key Features for Interview

### Code Quality
- Clean, readable code
- Proper TypeScript usage
- Component composition
- Separation of concerns
- Consistent formatting

### Best Practices
- React Hooks usage
- Error boundaries (implicit via React)
- Loading states
- Form validation
- Responsive design
- Accessibility considerations

### User Experience
- Intuitive interface
- Visual feedback (loading, success, errors)
- Confirmation dialogs for destructive actions
- Keyboard navigation support
- Clear visual hierarchy

##  Available Scripts

- `npm run dev` - Start development server
- `npm run build` - Build for production
- `npm run preview` - Preview production build
- `npm run lint` - Run ESLint

##  Troubleshooting

### Backend Connection Issues
- Ensure backend is running on `http://localhost:5000`
- Check CORS configuration on backend
- Verify API base URL in `.env` or `constants.ts`

### Build Errors
- Delete `node_modules` and run `npm install` again
- Clear npm cache: `npm cache clean --force`
- Check Node.js version (should be 18+)

### Authentication Issues
- Clear localStorage in browser DevTools
- Check that backend is returning valid JWT tokens
- Verify token format in network tab

##  Additional Documentation

- [Architecture Documentation](./docs/ARCHITECTURE.md)
- [API Integration Guide](./docs/API_INTEGRATION.md)
- [Implementation Plan](./IMPLEMENTATION_PLAN.md)

##  Learning Resources

If presenting this project, highlight:
1. Component architecture and reusability
2. TypeScript integration and type safety
3. Material-UI usage and customization
4. API integration with error handling
5. Form validation and user experience
6. Responsive design implementation
7. Authentication flow and JWT handling

##  License

This project is created for interview purposes.

---

**Built with ❤ using React, TypeScript, and Material-UI**

