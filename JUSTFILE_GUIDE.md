# Just Command Runner Guide

This project uses [Just](https://github.com/casey/just) as a task runner to simplify common commands.

## Installing Just

### macOS
```bash
brew install just
```

### Linux
```bash
curl --proto '=https' --tlsv1.2 -sSf https://just.systems/install.sh | bash -s -- --to /usr/local/bin
```

### Windows
```bash
scoop install just
# or
cargo install just
```

## Available Commands

Run `just` from the project root to see all available commands:

```bash
just
```

### Main Commands

| Command | Description |
|---------|-------------|
| `just setup` | First-time setup - installs all dependencies (.NET packages, npm modules) |
| `just dev` | Start all services (database + backend + frontend) with seed data |
| `just stop` | Stop all running services |

### Testing

| Command | Description |
|---------|-------------|
| `just test` | Run all .NET tests (unit + integration) with isolated test database |
| `just e2e` | Run Cypress end-to-end tests (starts all services automatically) |

### Build

| Command | Description |
|---------|-------------|
| `just build` | Build both backend and frontend projects |

## Typical Workflow

### First Time Setup

```bash
just setup    # Install all dependencies
just dev      # Start everything
```

Open http://localhost:5173 and login with demo credentials.

### Daily Development

```bash
just dev      # Start all services
# ... work on code ...
just stop     # Stop when done
```

### Running Tests

```bash
just test     # Run unit and integration tests
just e2e      # Run end-to-end tests
```

## What Each Command Does

### `just setup`
- Restores .NET packages (`dotnet restore`)
- Installs frontend npm packages (`npm install`)
- Installs e2e npm packages (`npm install`)

### `just dev`
- Starts PostgreSQL container via Docker
- Waits for database to be ready
- Runs SQL scripts to create tables and seed demo data
- Starts backend API on http://localhost:5001
- Starts frontend dev server on http://localhost:5173

### `just stop`
- Stops PostgreSQL container
- Kills backend and frontend processes

### `just test`
- Starts isolated test database on port 5433
- Runs all .NET tests (Domain, Application, Infrastructure, Integration)
- Stops test database when done

### `just e2e`
- Starts test database, backend, and frontend
- Runs Cypress tests
- Cleans up all services when done

### `just build`
- Builds backend (`dotnet build`)
- Builds frontend (`npm run build`)

## More Information

- Just Documentation: https://just.systems
- Project README: [README.md](./README.md)
