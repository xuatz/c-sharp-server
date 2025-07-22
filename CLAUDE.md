# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a C# REST API server built with ASP.NET Core Web API. It provides a simple Hello World endpoint and includes Swagger UI for API documentation.

## Common Commands

### Build
```bash
dotnet build
```

### Run
```bash
dotnet run
```

The server will start on http://localhost:5000
- API endpoint: http://localhost:5000/api/hello
- Swagger UI: http://localhost:5000/swagger

### Test
```bash
dotnet test
```

### Run specific test
```bash
dotnet test --filter "FullyQualifiedName~TestClassName.TestMethodName"
```

### Clean
```bash
dotnet clean
```

### Restore dependencies
```bash
dotnet restore
```

## Project Structure

Current structure:
- `/Controllers` - API controllers (similar to Express routes)
  - `HelloController.cs` - Hello World endpoint
  - `AuthController.cs` - Authentication endpoints
- `/Models` - Data models and DTOs
  - `User.cs` - User entity (extends IdentityUser)
  - `LoginDto.cs`, `RegisterDto.cs`, `AuthResponseDto.cs` - Request/Response models
- `/Services` - Business logic
  - `AuthService.cs` - Authentication logic
  - `TokenService.cs` - JWT token generation
- `/Data` - Database context
  - `AppDbContext.cs` - Entity Framework database context
- `Program.cs` - Application entry point (similar to Node.js index.js)
- `appsettings.json` - Configuration file (similar to .env)
- `CSharpServer.csproj` - Project file (similar to package.json)
- `app.db` - SQLite database file (created on first run)

## Development Guidelines

### Creating a new project
For a web API server:
```bash
dotnet new webapi -n ProjectName
```

For a console application server:
```bash
dotnet new console -n ProjectName
```

### Adding packages
```bash
dotnet add package PackageName
```

### Common server packages to consider:
- ASP.NET Core for web APIs
- Entity Framework Core for data access
- Serilog for logging
- xUnit, NUnit, or MSTest for testing
- FluentValidation for input validation
- AutoMapper for object mapping

## API Endpoints

### Hello Controller
- `GET /api/hello` - Returns `{"message": "Hello World!"}`
- `GET /api/hello/{name}` - Returns `{"message": "Hello {name}!"}`

### Auth Controller
- `POST /api/auth/register` - Register a new user
  - Body: `{"email": "user@example.com", "username": "username", "password": "password"}`
- `POST /api/auth/login` - Login user
  - Body: `{"usernameOrEmail": "username or email", "password": "password"}`
- `POST /api/auth/logout` - Logout (requires authentication)
- `GET /api/auth/me` - Get current user info (requires authentication)

## Quick Test Commands

Test the API using curl:
```bash
# Basic hello
curl http://localhost:5000/api/hello

# Hello with name
curl http://localhost:5000/api/hello/John

# Register a new user
curl -X POST http://localhost:5000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"email":"test@example.com","username":"testuser","password":"testpass123"}'

# Login
curl -X POST http://localhost:5000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"usernameOrEmail":"testuser","password":"testpass123"}'

# Get current user (replace TOKEN with actual JWT token)
curl http://localhost:5000/api/auth/me \
  -H "Authorization: Bearer TOKEN"
```

## Testing Approach

- Unit tests for business logic
- Integration tests for API endpoints
- Use test doubles (mocks, stubs) appropriately
- Follow AAA pattern (Arrange, Act, Assert)