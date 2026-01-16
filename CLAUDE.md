# Project: premesti.se

## Tech Stack
- ASP.NET Core (C#)
- Entity Framework Core
- Blazor (if frontend needed)

## Development Guidelines

When working on this project, follow the patterns and best practices defined in the `.claude/` directory:

### Agents (Specialized Expertise)
Reference these for domain-specific guidance:
- `.claude/agents/architecture-specialist.md` - Clean Architecture, DDD, CQRS, solution structure
- `.claude/agents/aspnet-api-specialist.md` - REST APIs, minimal APIs, controllers
- `.claude/agents/aspnet-security-specialist.md` - JWT, authentication, authorization
- `.claude/agents/blazor-specialist.md` - Blazor components and state management
- `.claude/agents/entity-framework-specialist.md` - Database design, EF Core, migrations

### Skills (Code Patterns)
Reference these for implementation patterns:
- `.claude/skills/api-testing-patterns.md` - Integration and unit testing
- `.claude/skills/aspnet-core-best-practices.md` - Project structure, DI, error handling

## Key Principles

1. **API Development**: Use minimal APIs for simple endpoints, controllers for complex scenarios
2. **Security**: JWT for stateless APIs, policy-based authorization over role-based
3. **Database**: Use Fluent API over data annotations, AsNoTracking for read-only queries
4. **Testing**: Always include integration tests with WebApplicationFactory

## Project Structure (Target)
```
src/
├── Api/
│   ├── Endpoints/
│   ├── Models/
│   │   ├── Requests/
│   │   └── Responses/
│   └── Program.cs
├── Core/
│   ├── Entities/
│   ├── Interfaces/
│   └── Services/
├── Infrastructure/
│   ├── Data/
│   │   ├── ApplicationDbContext.cs
│   │   └── Configurations/
│   └── Migrations/
└── Tests/
    ├── Unit/
    └── Integration/
```
