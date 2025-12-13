# Task Management System - Monorepo Commands
# Run: just <command>

# Show all available commands
default:
    @just --list

# ═══════════════════════════════════════════════════════════
# 🚀 Quick Start
# ═══════════════════════════════════════════════════════════

# Complete setup (first time only)
setup:
    @echo "🔧 Setting up project..."
    @just backend-setup
    @just frontend-install
    @echo "✅ Setup complete! Run 'just dev' to start"

# Start everything in development mode
dev:
    @echo "🚀 Starting development servers..."
    @just backend-db-up
    @sleep 2
    @echo "\n📦 Backend starting on http://localhost:5000"
    @echo "🎨 Frontend starting on http://localhost:5173\n"
    @just -j2 backend-dev frontend-dev

# Stop all services
stop:
    @echo "🛑 Stopping all services..."
    @just backend-db-down
    @pkill -f "dotnet run" || true
    @pkill -f "vite" || true
    @echo "✅ All services stopped"

# ═══════════════════════════════════════════════════════════
# 🏗️  Build & Install
# ═══════════════════════════════════════════════════════════

# Install/update all dependencies
install:
    @echo "📦 Installing dependencies..."
    @cd backend && dotnet restore
    @cd frontend && npm install
    @echo "✅ Dependencies installed"

# Build everything
build:
    @echo "🔨 Building all projects..."
    @just backend-build
    @just frontend-build
    @echo "✅ Build complete"

# Build backend only
backend-build:
    @echo "🔨 Building backend..."
    @cd backend && dotnet build

# Build frontend only
frontend-build:
    @echo "🔨 Building frontend..."
    @cd frontend && npm run build

# Clean all build artifacts
clean:
    @echo "🧹 Cleaning..."
    @cd backend && dotnet clean
    @cd frontend && rm -rf node_modules dist
    @echo "✅ Clean complete"

# ═══════════════════════════════════════════════════════════
# 🧪 Testing
# ═══════════════════════════════════════════════════════════

# Run all tests (automatically manages test database)
test:
    #!/usr/bin/env bash
    set -e
    cd backend
    echo "🐘 Starting test database..."
    docker-compose -f docker-compose.test.yml up -d
    echo "⏳ Waiting for database to be ready..."
    MAX_TRIES=30
    COUNTER=0
    until docker exec taskmanagement-test-db pg_isready -U postgres > /dev/null 2>&1; do
        COUNTER=$((COUNTER+1))
        if [ $COUNTER -gt $MAX_TRIES ]; then
            echo "❌ Database failed to start!"
            docker-compose -f docker-compose.test.yml down
            exit 1
        fi
        sleep 1
        echo "  Waiting... ($COUNTER/$MAX_TRIES)"
    done
    echo "✅ Database is ready!"
    echo "🧪 Running tests..."
    if dotnet test; then
        echo "✅ All tests passed!"
        docker-compose -f docker-compose.test.yml down
    else
        echo "❌ Tests failed!"
        docker-compose -f docker-compose.test.yml down
        exit 1
    fi

# Run tests with coverage (same as test for now)
test-cov: test

# Watch tests (auto-run on changes)
test-watch:
    @echo "👀 Watching tests..."
    @cd backend && dotnet watch test

# ═══════════════════════════════════════════════════════════
# 🗄️  Database
# ═══════════════════════════════════════════════════════════

# Start database
db-up:
    @just backend-db-up

# Stop database
db-down:
    @just backend-db-down

# Reset database (drop + recreate + seed)
db-reset:
    @echo "🔄 Resetting database..."
    @just backend-db-down
    @just backend-db-up
    @sleep 3
    @just backend-db-setup
    @echo "✅ Database reset complete"

# Connect to database CLI
db-shell:
    @docker exec -it gym-management-db psql -U postgres -d taskmanagement

# ═══════════════════════════════════════════════════════════
# 📦 Backend Commands
# ═══════════════════════════════════════════════════════════

# Setup backend (database + seed)
backend-setup:
    @cd backend && just setup

# Start backend in dev mode
backend-dev:
    @cd backend && just run-watch

# Run backend tests
backend-test:
    @cd backend && dotnet test

# Start database only
backend-db-up:
    @cd backend && docker-compose up -d postgres

# Stop database
backend-db-down:
    @cd backend && docker-compose down

# Setup database (migrate + seed)
backend-db-setup:
    @cd backend && just db-setup

# ═══════════════════════════════════════════════════════════
# 🎨 Frontend Commands
# ═══════════════════════════════════════════════════════════

# Install frontend dependencies
frontend-install:
    @echo "📦 Installing frontend dependencies..."
    @cd frontend && npm install

# Start frontend in dev mode
frontend-dev:
    @cd frontend && npm run dev

# Preview production build
frontend-preview:
    @cd frontend && npm run preview

# Lint frontend code
frontend-lint:
    @cd frontend && npm run lint

# ═══════════════════════════════════════════════════════════
# 🎬 Demo & Presentation
# ═══════════════════════════════════════════════════════════

# Prepare for demo/presentation
demo:
    @echo "🎬 Preparing demo environment..."
    @just db-reset
    @echo "\n✨ Demo ready!"
    @echo "\n📝 Demo Credentials:"
    @echo "   Email: john@example.com"
    @echo "   Password: TestPassword123"
    @echo "\n🚀 Next steps:"
    @echo "   1. Run: just dev"
    @echo "   2. Backend: http://localhost:5000"
    @echo "   3. Frontend: http://localhost:5173"
    @echo ""

# ═══════════════════════════════════════════════════════════
# 📊 Info & Help
# ═══════════════════════════════════════════════════════════

# Show project information
info:
    @echo "╔══════════════════════════════════════════════════════════╗"
    @echo "║      Task Management System - Full Stack App             ║"
    @echo "╠══════════════════════════════════════════════════════════╣"
    @echo "║ Backend:          .NET 8 + PostgreSQL                    ║"
    @echo "║ Frontend:         React 18 + TypeScript + MUI            ║"
    @echo "║ Architecture:     Clean Architecture + TDD               ║"
    @echo "║ Database:         PostgreSQL (Docker)                    ║"
    @echo "╚══════════════════════════════════════════════════════════╝"
    @echo ""
    @echo "🚀 Quick Commands:"
    @echo "   just setup        First time setup"
    @echo "   just dev          Start dev servers"
    @echo "   just test         Run tests"
    @echo "   just demo         Prepare demo"
    @echo "   just stop         Stop everything"
    @echo ""
    @echo "📖 More info: just --list"
    @echo ""

# Show this help
help: info

