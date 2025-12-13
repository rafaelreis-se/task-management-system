# Quick Setup Guide

## Prerequisites
- Node.js 18+ installed
- Backend API running on port 5000

## Installation Steps

1. **Install dependencies**
```bash
npm install
```

2. **Start the development server**
```bash
npm run dev
```

3. **Open browser**
Navigate to: http://localhost:5173

## Default Login Credentials
```
Email: john@example.com
Password: TestPassword123
```

## Verify Backend Connection
Make sure the backend API is running:
```bash
curl http://localhost:5000/api/health
```

## Common Issues

### Port 5173 already in use
Change the port in `vite.config.ts`:
```typescript
server: {
  port: 3000, // or any available port
}
```

### Cannot connect to backend
1. Check backend is running
2. Verify CORS is configured on backend
3. Check API URL in `src/utils/constants.ts`

### Module not found errors
```bash
rm -rf node_modules package-lock.json
npm install
```

## Project Structure Overview
```
src/
├── components/     # Reusable components
├── pages/         # Route pages
├── services/      # API calls
├── context/       # Global state
├── types/         # TypeScript types
└── utils/         # Constants & helpers
```

## Next Steps
1. Test login with demo credentials
2. Create a new task
3. Try editing and deleting tasks
4. Test filtering by status
5. Register a new account
6. Test responsive design on mobile

## For Development
- Hot reload is enabled
- TypeScript errors show in console
- ESLint checks code quality
- Material-UI provides component documentation

Enjoy coding! 🚀

