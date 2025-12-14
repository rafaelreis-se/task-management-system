# Thought Process - Task Management System

## How I Approached This Exercise

When I first read the requirements, I knew I had limited time to deliver a complete solution. So my first decision was to leverage patterns and architectures I'm already comfortable with from my daily work. This helped me move faster and focus on what really matters: delivering a working, well-structured application.

## Initial Decisions

### Using Familiar Architecture

I chose to implement Clean Architecture because it's an approach I use regularly in my projects. Having this familiarity allowed me to set up the project structure quickly and confidently, knowing exactly where each piece of code should go. The four-layer separation (Domain, Application, Infrastructure, API) came naturally, and I could focus my energy on the specific requirements rather than learning something new.

### Planning Before Coding

Before writing any code, I created implementation plans for both backend and frontend. I also wrote `.cursorrules` files to provide context when using AI assistance. This upfront planning saved me time later because I had a clear roadmap to follow.

## Development Process

### Test-Driven Development

I followed TDD throughout the project - writing tests first, then implementing the code to make them pass. This approach helped me:
- Think about the requirements before coding
- Catch bugs early
- Have confidence when refactoring

### Manual Implementations

Since Entity Framework and Dapper were not allowed, I implemented the data access layer using raw SQL with Npgsql. For authentication, I built JWT token generation and BCrypt password hashing manually. These constraints actually gave me the opportunity to demonstrate that I understand what happens "under the hood" of these common frameworks.

## Why Cypress for E2E Testing

One decision I'm particularly happy with was adding Cypress for end-to-end testing. I realized early on that having automated tests for the frontend-backend integration would save me a lot of time. Instead of manually clicking through the UI every time I made a change, Cypress allowed me to quickly validate that everything was working together correctly.

This proved especially valuable for catching integration bugs early - things like incorrect API responses, authentication issues, or data format mismatches between frontend and backend. Running `just e2e` gave me confidence that the full user flow was working before moving on to the next feature.

## Using GenAI Tools

I used Cursor AI as a coding assistant throughout the project. However, I was careful to:
- Always review and understand the generated code
- Reject suggestions that seemed over-engineered
- Validate security-related code manually
- Keep the codebase simple and maintainable

The AI helped me move faster on boilerplate code, but the architecture decisions and critical implementations were my own.

## Challenges I Faced

### Entity Hydration Without ORM

Without an ORM, I needed to figure out how to create domain entities from database results while respecting their validation rules. I solved this using reflection to bypass the constructor validation during hydration.

### Test Database Isolation

I wanted integration tests to use a real database without affecting development data. I created a separate Docker Compose file that runs PostgreSQL on a different port (5433), keeping the test environment completely isolated.

## What I Would Improve

Given more time, I would add:
- Refresh token mechanism
- Rate limiting
- More detailed logging
- API versioning

## Final Thoughts

I'm happy with how this project turned out. The code is clean, well-tested, and demonstrates the key requirements. By using familiar patterns and making smart choices about tooling (like Cypress), I was able to deliver a complete full-stack application within the time constraints.

---

**Repository**: https://github.com/rafaelreis-se/task-management-system

**Author**: Rafael Reis
