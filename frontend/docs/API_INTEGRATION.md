# API Integration Guide

## Backend API Reference

Base URL: `http://localhost:5000/api`

## Authentication Endpoints

### Register
```
POST /auth/register
Content-Type: application/json

Request:
{
  "name": "John Doe",
  "email": "john@example.com",
  "password": "TestPassword123"
}

Response: 200 OK
{
  "id": "guid",
  "name": "John Doe",
  "email": "john@example.com"
}

Errors:
- 400: Validation failed (email exists, weak password)
```

### Login
```
POST /auth/login
Content-Type: application/json

Request:
{
  "email": "john@example.com",
  "password": "TestPassword123"
}

Response: 200 OK
{
  "token": "eyJhbGc...",
  "user": {
    "id": "guid",
    "name": "John Doe",
    "email": "john@example.com"
  }
}

Errors:
- 400: Invalid credentials
```

## Task Endpoints (Protected)

All task endpoints require JWT token in header:
```
Authorization: Bearer <token>
```

### Get All Tasks
```
GET /tasks

Response: 200 OK
[
  {
    "id": "guid",
    "title": "Complete project",
    "description": "Finish frontend",
    "status": "InProgress",
    "dueDate": "2024-12-20T00:00:00Z",
    "createdAt": "2024-12-13T10:00:00Z",
    "updatedAt": "2024-12-13T15:00:00Z"
  }
]
```

### Get Task by ID
```
GET /tasks/{id}

Response: 200 OK
{
  "id": "guid",
  "title": "Task title",
  "description": "Description",
  "status": "Pending",
  "dueDate": "2024-12-20T00:00:00Z",
  "createdAt": "2024-12-13T10:00:00Z",
  "updatedAt": null
}

Errors:
- 404: Task not found
- 401: Unauthorized (not your task)
```

### Create Task
```
POST /tasks
Content-Type: application/json

Request:
{
  "title": "New task",
  "description": "Optional description",
  "dueDate": "2024-12-20T00:00:00Z"
}

Response: 201 Created
{
  "id": "guid",
  "title": "New task",
  "description": "Optional description",
  "status": "Pending",
  "dueDate": "2024-12-20T00:00:00Z",
  "createdAt": "2024-12-13T10:00:00Z",
  "updatedAt": null
}

Errors:
- 400: Validation failed (empty title, past due date)
```

### Update Task
```
PUT /tasks/{id}
Content-Type: application/json

Request:
{
  "title": "Updated title",
  "description": "Updated description",
  "dueDate": "2024-12-25T00:00:00Z",
  "status": "InProgress"
}

Response: 200 OK
{
  "id": "guid",
  "title": "Updated title",
  "description": "Updated description",
  "status": "InProgress",
  "dueDate": "2024-12-25T00:00:00Z",
  "createdAt": "2024-12-13T10:00:00Z",
  "updatedAt": "2024-12-13T16:00:00Z"
}

Errors:
- 404: Task not found
- 401: Unauthorized (not your task)
- 400: Validation failed
```

### Delete Task
```
DELETE /tasks/{id}

Response: 204 No Content

Errors:
- 404: Task not found
- 401: Unauthorized (not your task)
```

## Task Status Values

Valid status values:
- `Pending` - New task, not started
- `InProgress` - Currently working on it
- `Completed` - Finished

## Date Format

All dates in ISO 8601 format:
```
"2024-12-20T00:00:00Z"
```

Frontend should:
- Display in local format
- Send in ISO format

## Error Handling

Common error responses:
```json
{
  "error": "Error message",
  "statusCode": 400
}
```

Handle these status codes:
- 400: Bad request (validation)
- 401: Unauthorized (login required)
- 404: Not found
- 500: Server error

## Frontend Implementation

### Setup Axios
```typescript
import axios from 'axios';

const api = axios.create({
  baseURL: 'http://localhost:5000/api'
});

// Add token to all requests
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// Handle 401 errors
api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      localStorage.removeItem('token');
      window.location.href = '/';
    }
    return Promise.reject(error);
  }
);
```

### Auth Service
```typescript
export const authService = {
  login: async (email: string, password: string) => {
    const response = await api.post('/auth/login', { email, password });
    return response.data;
  },
  
  register: async (name: string, email: string, password: string) => {
    const response = await api.post('/auth/register', { name, email, password });
    return response.data;
  }
};
```

### Task Service
```typescript
export const taskService = {
  getAll: async () => {
    const response = await api.get('/tasks');
    return response.data;
  },
  
  create: async (task: CreateTaskDto) => {
    const response = await api.post('/tasks', task);
    return response.data;
  },
  
  update: async (id: string, task: UpdateTaskDto) => {
    const response = await api.put(`/tasks/${id}`, task);
    return response.data;
  },
  
  delete: async (id: string) => {
    await api.delete(`/tasks/${id}`);
  }
};
```

## Test Credentials

Use these for testing:
```
Email: john@example.com
Password: TestPassword123
```

## CORS

Backend already configured for CORS.
Frontend can make requests from `http://localhost:5173` (Vite default).

