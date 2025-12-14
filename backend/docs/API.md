# API Documentation

Base URL: `http://localhost:5001/api`

## Authentication Endpoints

### Register User
```
POST /api/auth/register
Content-Type: application/json

{
  "name": "John Doe",
  "email": "john@example.com",
  "password": "SecurePass123"
}

Response: 201 Created
{
  "id": "guid",
  "name": "John Doe",
  "email": "john@example.com"
}
```

### Login
```
POST /api/auth/login
Content-Type: application/json

{
  "email": "john@example.com",
  "password": "SecurePass123"
}

Response: 200 OK
{
  "token": "jwt-token-here",
  "expiresAt": "2024-12-31T23:59:59Z"
}
```

## Task Endpoints (Protected)

All task endpoints require JWT token in Authorization header:
`Authorization: Bearer {token}`

### Get All Tasks
```
GET /api/tasks

Response: 200 OK
[
  {
    "id": "guid",
    "title": "Task title",
    "description": "Task description",
    "status": "Pending",
    "dueDate": "2024-12-31T23:59:59Z",
    "createdAt": "2024-01-01T00:00:00Z",
    "updatedAt": null
  }
]
```

### Get Task by ID
```
GET /api/tasks/{id}

Response: 200 OK or 404 Not Found
```

### Create Task
```
POST /api/tasks
Content-Type: application/json

{
  "title": "New task",
  "description": "Task details",
  "status": "Pending",
  "dueDate": "2024-12-31T23:59:59Z"
}

Response: 201 Created
```

### Update Task
```
PUT /api/tasks/{id}
Content-Type: application/json

{
  "title": "Updated task",
  "description": "Updated details",
  "status": "InProgress",
  "dueDate": "2024-12-31T23:59:59Z"
}

Response: 200 OK or 404 Not Found
```

### Delete Task
```
DELETE /api/tasks/{id}

Response: 204 No Content or 404 Not Found
```

## Status Codes

- 200 OK: Successful GET/PUT request
- 201 Created: Successful POST request
- 204 No Content: Successful DELETE request
- 400 Bad Request: Invalid input data
- 401 Unauthorized: Missing or invalid token
- 404 Not Found: Resource not found
- 500 Internal Server Error: Server error

## Error Response Format

```json
{
  "error": "Error message",
  "details": "Additional details if available"
}
```

