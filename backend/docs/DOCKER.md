# Docker Setup

## Quick Start

Start PostgreSQL and API:
```bash
docker-compose up -d
```

Stop services:
```bash
docker-compose down
```

Stop and remove volumes:
```bash
docker-compose down -v
```

## Services

### PostgreSQL
- Port: 5432
- Database: taskmanagement
- User: postgres
- Password: postgres

Access database directly:
```bash
docker exec -it taskmanagement-db psql -U postgres -d taskmanagement
```

### API
- Port: 5000
- Swagger: http://localhost:5000/swagger
- Health: http://localhost:5000/health

## Development

Run only database for local development:
```bash
docker-compose up -d postgres
```

Then run API from IDE or terminal:
```bash
cd src/TaskManagement.API
dotnet run
```

## Connection String

For local development with Docker PostgreSQL:
```
Host=localhost;Database=taskmanagement;Username=postgres;Password=postgres
```

For API running in Docker:
```
Host=postgres;Database=taskmanagement;Username=postgres;Password=postgres
```

## Logs

View API logs:
```bash
docker-compose logs -f api
```

View database logs:
```bash
docker-compose logs -f postgres
```

## Rebuild

After code changes:
```bash
docker-compose up -d --build api
```

