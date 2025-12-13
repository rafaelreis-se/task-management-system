# Justfile Commands Guide

## What is Just?

`just` is a modern command runner (like Make, but better). It simplifies running project commands.

## Installation

```bash
# macOS
brew install just

# Linux
cargo install just

# Windows
scoop install just
```

## Why Use Just?

**For Interview:**
- Shows professionalism
- Makes it easy for evaluators to test your project
- Demonstrates automation skills
- Better developer experience

**Benefits:**
- Simple, readable syntax
- No Makefile tab issues
- Cross-platform
- Self-documenting

## Available Commands

### Essential Commands

```bash
# See all available commands
just

# Complete setup from scratch
just setup

# Run the API
just run

# Run all tests (72 tests with isolated test database on port 5433)
just test
```

### Development Commands

```bash
# Build the solution
just build

# Clean build artifacts
just clean

# Format code
just format

# Watch tests (auto-reload)
just test-watch

# Run API with watch (auto-reload)
just run-watch
```

### Database Commands

```bash
# Start PostgreSQL
just db-up

# Run migrations
just db-migrate

# Seed test data
just db-seed

# Complete database setup
just db-setup

# Connect to database CLI
just db-connect

# Stop database
just db-down
```

### Docker Commands

```bash
# Start all services
just docker-up

# Rebuild and start
just docker-rebuild

# View logs
just docker-logs

# Clean everything
just docker-clean
```

### Testing Commands

```bash
# Run all tests
just test

# Run only unit tests
just test-unit

# Watch tests (auto-run on changes)
just test-watch
```

### Utility Commands

```bash
# Show project info
just info

# Quick demo setup for presentation
just demo
```

## Quick Start for Interview

When presenting the project:

```bash
# 1. Complete setup
just setup

# 2. Start API
just run

# 3. In another terminal, run tests
just test
```

## Common Workflows

### First Time Setup
```bash
just setup
just run
```

### Daily Development
```bash
just db-up          # Start database
just run-watch      # Start API with auto-reload
just test-watch     # Run tests on file changes
```

### Before Committing
```bash
just format         # Format code
just test          # Run tests
just lint          # Check for errors
```

### Demo for Interview
```bash
just demo          # Complete demo setup
just run           # Start API
# Open http://localhost:5000/swagger
```

## Customization

You can easily add your own commands by editing the `justfile`:

```just
# Custom command example
my-command:
    @echo "Running my command..."
    @dotnet run --project MyProject
```

## Tips

1. **Always use just for demos** - Makes you look professional
2. **Add custom commands** as you need them
3. **Document complex commands** with comments
4. **Use @ prefix** to hide command output (cleaner)

## Comparison with Other Tools

| Feature | just | Make | npm scripts |
|---------|------|------|-------------|
| Tab issues | No | Yes | No |
| Cross-platform | Yes | Limited | Yes |
| Readable | Yes | Complex | Yes |
| .NET native | Yes | No | No |

## For the Interview

**Mention to evaluators:**
"I've included a `justfile` for easy command running. You can see all available commands with `just` or `just --list`. For a quick demo, just run `just demo` and then `just run`."

This shows:
- Attention to developer experience
- Professional tooling knowledge
- Making life easier for reviewers
- Modern development practices

