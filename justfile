# Task Management System - Monorepo
# Run: just <command>

default:
    @just --list

# ═══════════════════════════════════════════════════════════
# Main Commands
# ═══════════════════════════════════════════════════════════

# First time setup (install all dependencies)
setup:
    @echo "Setting up project..."
    @cd backend && dotnet restore
    @cd frontend && npm install
    @cd e2e && npm install
    @echo ""
    @echo "Setup complete! Run 'just dev' to start"

# Start backend + frontend for demo
dev:
    #!/usr/bin/env bash
    set -e
    echo "Starting development servers..."
    
    cd backend
    docker-compose up -d postgres
    
    echo "Waiting for database..."
    until docker exec taskmanagement-db pg_isready -U postgres > /dev/null 2>&1; do
        sleep 1
    done
    
    # Initialize database if needed
    echo "Initializing database..."
    cat src/TaskManagement.Infrastructure/Data/Scripts/001_create_tables.sql | docker exec -i taskmanagement-db psql -U postgres -d taskmanagement 2>/dev/null || true
    cat src/TaskManagement.Infrastructure/Data/Scripts/002_seed_data.sql | docker exec -i taskmanagement-db psql -U postgres -d taskmanagement 2>/dev/null || true
    
    echo ""
    echo "Backend:  http://localhost:5001"
    echo "Frontend: http://localhost:5173"
    echo ""
    echo "Demo credentials:"
    echo "  Email:    john@example.com"
    echo "  Password: TestPassword123"
    echo ""
    
    ASPNETCORE_ENVIRONMENT=Development ASPNETCORE_URLS=http://localhost:5001 dotnet run --project src/TaskManagement.API &
    cd ../frontend && npm run dev

# Stop all services
stop:
    @echo "Stopping all services..."
    @cd backend && docker-compose down
    @pkill -f "dotnet run" || true
    @pkill -f "vite" || true
    @echo "All services stopped"

# ═══════════════════════════════════════════════════════════
# Testing
# ═══════════════════════════════════════════════════════════

# Run all tests - unit and integration (.NET)
test:
    #!/usr/bin/env bash
    set -e
    cd backend
    echo "Starting test database..."
    docker-compose -f docker-compose.test.yml up -d
    
    echo "Waiting for database..."
    until docker exec taskmanagement-test-db pg_isready -U postgres > /dev/null 2>&1; do
        sleep 1
    done
    
    echo "Running unit and integration tests..."
    if dotnet test; then
        echo "All tests passed!"
        docker-compose -f docker-compose.test.yml down
    else
        echo "Tests failed!"
        docker-compose -f docker-compose.test.yml down
        exit 1
    fi

# Run E2E tests (Cypress)
e2e:
    #!/usr/bin/env bash
    set -e
    
    # Fix Cypress issue with Cursor IDE's ELECTRON_RUN_AS_NODE
    unset ELECTRON_RUN_AS_NODE
    
    ROOT_DIR="$(pwd)"
    
    cleanup() {
        echo "Cleaning up..."
        pkill -f "dotnet run" 2>/dev/null || true
        pkill -f "vite" 2>/dev/null || true
        cd "$ROOT_DIR/backend" && docker-compose -f docker-compose.test.yml down -v 2>/dev/null || true
    }
    trap cleanup EXIT
    
    # Start database
    echo "Starting test database..."
    cd "$ROOT_DIR/backend"
    docker-compose -f docker-compose.test.yml down -v 2>/dev/null || true
    docker-compose -f docker-compose.test.yml up -d
    
    until docker exec taskmanagement-test-db pg_isready -U postgres > /dev/null 2>&1; do
        sleep 1
    done
    sleep 3
    
    # Start backend
    echo "Starting backend..."
    export ConnectionStrings__DefaultConnection="Host=localhost;Port=5433;Database=taskmanagement_test;Username=postgres;Password=postgres"
    export ASPNETCORE_ENVIRONMENT=Development
    export ASPNETCORE_URLS=http://localhost:5001
    dotnet run --project src/TaskManagement.API &
    
    until curl -s http://localhost:5001/api/auth/login > /dev/null 2>&1; do
        sleep 1
    done
    
    # Start frontend
    echo "Starting frontend..."
    cd "$ROOT_DIR/frontend"
    npm run dev &
    sleep 5
    
    # Run tests
    echo ""
    echo "Running E2E tests..."
    cd "$ROOT_DIR/e2e"
    npm run cy:run

# ═══════════════════════════════════════════════════════════
# Build
# ═══════════════════════════════════════════════════════════

# Build all projects
build:
    @echo "Building backend..."
    @cd backend && dotnet build
    @echo "Building frontend..."
    @cd frontend && npm run build
    @echo "Build complete!"
