#!/bin/bash

echo "=========================================="
echo "Task Management System - Setup Script"
echo "=========================================="
echo ""

# Check if Docker is running
if ! docker info > /dev/null 2>&1; then
    echo "ERROR: Docker is not running. Please start Docker first."
    exit 1
fi

echo "Docker is running"

# Start services
echo ""
echo "Starting services..."
docker-compose up -d

echo ""
echo "Waiting for PostgreSQL to be ready..."
sleep 5

# Check if database is ready
until docker exec taskmanagement-db pg_isready -U postgres > /dev/null 2>&1; do
    echo "Waiting for database..."
    sleep 2
done

echo "PostgreSQL is ready"

# Run migrations
echo ""
echo "Creating database tables..."
docker exec -i taskmanagement-db psql -U postgres -d taskmanagement < src/TaskManagement.Infrastructure/Data/Scripts/001_create_tables.sql

echo ""
echo "Seeding test data..."
docker exec -i taskmanagement-db psql -U postgres -d taskmanagement < src/TaskManagement.Infrastructure/Data/Scripts/002_seed_data.sql

echo ""
echo "=========================================="
echo "Setup Complete!"
echo "=========================================="
echo ""
echo "Services:"
echo "  - API: http://localhost:5000"
echo "  - Swagger: http://localhost:5000/swagger"
echo "  - PostgreSQL: localhost:5432"
echo ""
echo "Test Credentials:"
echo "  - Email: john@example.com"
echo "  - Password: TestPassword123"
echo ""
echo "Useful Commands:"
echo "  - View logs: docker-compose logs -f"
echo "  - Stop services: docker-compose down"
echo "  - Run tests: dotnet test"
echo ""
echo "Next: Open http://localhost:5000/swagger in your browser"
echo "=========================================="

