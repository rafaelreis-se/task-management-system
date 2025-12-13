# 🚀 Quick Start Guide

Get the Task Management System running in 5 minutes!

## Step 1: Backend Setup

```bash
# Navigate to backend
cd backend

# Initialize database and start API
./setup.sh

# In a new terminal, run the API
dotnet run --project src/TaskManagement.API
```

✅ Backend should be running at: **http://localhost:5000**

## Step 2: Frontend Setup

```bash
# In a new terminal, navigate to frontend
cd frontend

# Install dependencies
npm install

# Start development server
npm run dev
```

✅ Frontend should be running at: **http://localhost:5173**

## Step 3: Open and Test

1. Open browser: **http://localhost:5173**
2. Login with demo credentials:
   - Email: `john@example.com`
   - Password: `TestPassword123`
3. Create, edit, and delete tasks!

## 📋 Checklist

Before starting:
- [ ] .NET 8 SDK installed
- [ ] Node.js 18+ installed
- [ ] PostgreSQL running
- [ ] Ports 5000 and 5173 available

## 🐛 Troubleshooting

### Backend won't start?
```bash
# Check if PostgreSQL is running
psql --version

# Verify connection in appsettings.json
cat backend/src/TaskManagement.API/appsettings.json
```

### Frontend won't start?
```bash
# Clear and reinstall dependencies
rm -rf node_modules package-lock.json
npm install
```

### Database issues?
```bash
# Reset database
cd backend
./setup.sh
```

## 🎉 You're Ready!

Now you can:
- ✅ Create tasks
- ✅ Update task status
- ✅ Delete tasks
- ✅ Filter by status
- ✅ Test on mobile (responsive!)

For the interview presentation, see [PRESENTATION_GUIDE.md](./PRESENTATION_GUIDE.md)

---

**Having issues?** Check the detailed docs:
- [Backend Setup](./backend/SETUP.md)
- [Frontend Setup](./frontend/README.md)
- [Full README](./README.md)

