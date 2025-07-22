# C# REST API Server with JWT Authentication

A modern REST API server built with ASP.NET Core 8.0, featuring JWT-based authentication, SQLite database, and Swagger UI for API documentation.

## Technology Stack

- **Framework**: ASP.NET Core 8.0 Web API
- **Authentication**: JWT Bearer tokens with ASP.NET Core Identity
- **Database**: SQLite with Entity Framework Core
- **API Documentation**: Swagger/OpenAPI with Swashbuckle
- **Language**: C# 12 with .NET 8

## Project Initialization

This project was created using the following commands:

```bash
# Create a new Web API project
dotnet new webapi -n CSharpServer --no-https --use-controllers -f net8.0

# Add required NuGet packages
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.11
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.11
```

## Architecture Overview

### Authentication Module

The authentication system uses JWT (JSON Web Tokens) for stateless authentication:

1. **User Registration**: Creates a new user with hashed password using ASP.NET Core Identity
2. **User Login**: Validates credentials and returns a JWT token
3. **Token Validation**: Protected endpoints validate the JWT token on each request
4. **User Profile**: Authenticated users can retrieve their profile information

### Database Layer

The application uses **Entity Framework Core** with **SQLite** as the database provider:

- **Database Context**: `AppDbContext` extends `IdentityDbContext<User>` to leverage built-in Identity tables
- **Connection**: SQLite file-based database (`app.db`) for simplicity and portability
- **Auto-creation**: Database is automatically created on first run

### Database Schema

The authentication system creates the following main tables:

- **AspNetUsers**: Stores user information (extended with custom fields)
  - Id (GUID)
  - UserName
  - Email
  - PasswordHash
  - CreatedAt (custom field)
  - LastLoginAt (custom field)
- **AspNetRoles**: Role definitions (created by Identity)
- **AspNetUserRoles**: User-role mappings
- **AspNetUserClaims**: User claims
- **AspNetUserLogins**: External login providers
- **AspNetUserTokens**: User tokens

### JWT Token Lifecycle

1. **Token Generation**: 
   - User successfully logs in
   - Server creates JWT with user claims (ID, username, email)
   - Token is signed with a secret key
   - Default expiration: 24 hours

2. **Token Usage**:
   - Client includes token in Authorization header: `Bearer <token>`
   - Server validates token signature and expiration
   - User claims are extracted from valid tokens

3. **Token Expiration**:
   - Tokens expire after 24 hours (configurable)
   - Expired tokens return 401 Unauthorized
   - Users must login again to get a new token

## API Endpoints

### Public Endpoints
- `GET /api/hello` - Hello World endpoint
- `POST /api/auth/register` - Register a new user
- `POST /api/auth/login` - Login with credentials

### Protected Endpoints (Require JWT)
- `GET /api/auth/me` - Get current user information
- `POST /api/auth/logout` - Logout (client-side token removal)

## Getting Started

### Environment Setup

#### 1. Install .NET 8.0 SDK

**Windows:**
- Download from [Microsoft .NET Download](https://dotnet.microsoft.com/download/dotnet/8.0)
- Run the installer and follow the setup wizard
- Verify installation: `dotnet --version`

**macOS:**
```bash
# Using Homebrew
brew install --cask dotnet

# Or download from Microsoft .NET Download page
```

**Linux (Ubuntu/Debian):**
```bash
# Add Microsoft package repository
wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb

# Install .NET SDK
sudo apt-get update
sudo apt-get install -y dotnet-sdk-8.0
```

**Verify Installation:**
```bash
dotnet --version
# Should show: 8.0.x
```

#### 2. Choose Your Development Environment

**Visual Studio Code (Recommended for beginners):**
- Download from [code.visualstudio.com](https://code.visualstudio.com/)
- Install the **C# Dev Kit** extension (includes C# and .NET tools)
- Install **REST Client** extension for API testing (optional)

**Visual Studio Community (Windows/Mac):**
- Download from [visualstudio.microsoft.com](https://visualstudio.microsoft.com/)
- Select "ASP.NET and web development" workload during installation

**JetBrains Rider:**
- Professional IDE with excellent .NET support
- 30-day free trial, then paid license required

**Command Line + Any Editor:**
- Works with any text editor (Sublime Text, Atom, Vim, etc.)
- All commands can be run from terminal

#### 3. Clone and Setup Project

```bash
# Clone the repository
git clone <your-repo-url>
cd c-sharp-server

# Restore NuGet packages
dotnet restore

# Verify everything is set up correctly
dotnet build
```

#### 4. Optional Tools

**Database Browser (for SQLite):**
- [DB Browser for SQLite](https://sqlitebrowser.org/) - View and edit SQLite databases
- [SQLiteStudio](https://sqlitestudio.pl/) - Alternative SQLite GUI

**API Testing Tools:**
- **Swagger UI** (built-in) - Available at http://localhost:5000/swagger when running
- **Postman** - Popular API testing tool
- **Insomnia** - Alternative to Postman
- **curl** - Command-line HTTP client

### Running the Application

```bash
# Navigate to project directory
cd c-sharp-server

# Restore dependencies (if not done already)
dotnet restore

# Run the application
dotnet run
```

**Expected Output:**
```
Building...
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shutdown.
```

### Accessing the Application

- **API Base URL**: http://localhost:5000
- **Swagger UI**: http://localhost:5000/swagger
- **Sample Endpoint**: http://localhost:5000/api/hello

### First Run Setup

1. **Database Creation**: The SQLite database (`app.db`) will be created automatically on first run
2. **No Migrations Needed**: Entity Framework will create all tables automatically
3. **Ready to Use**: All authentication endpoints are immediately available

### Troubleshooting

**Port Already in Use:**
```bash
# Kill process using port 5000
sudo kill -9 $(lsof -t -i:5000)

# Or change port in Properties/launchSettings.json
```

**Package Restore Issues:**
```bash
# Clear NuGet cache
dotnet nuget locals all --clear

# Restore packages
dotnet restore --force
```

**Build Errors:**
```bash
# Clean and rebuild
dotnet clean
dotnet build
```

**SSL Certificate Issues (if HTTPS enabled):**
```bash
# Trust development certificate
dotnet dev-certs https --trust
```

## Testing with Swagger UI

### 1. Access Swagger
Navigate to http://localhost:5000/swagger

### 2. Create an Account

1. Find the **POST /api/auth/register** endpoint
2. Click **"Try it out"**
3. Enter the following sample data:
   ```json
   {
     "email": "testuser@example.com",
     "username": "testuser",
     "password": "Test123!"
   }
   ```
4. Click **"Execute"**
5. You should receive a response with:
   - JWT token
   - Username
   - Email
   - Token expiration time

### 3. Authenticate in Swagger

1. Copy the `token` value from the registration response
2. Click the **"Authorize"** button 🔐 at the top of Swagger UI
3. In the popup dialog:
   - Enter: `Bearer YOUR_TOKEN_HERE` (replace YOUR_TOKEN_HERE with your actual token)
   - Click **"Authorize"**
   - Click **"Close"**

### 4. Access Protected Endpoints

1. Find the **GET /api/auth/me** endpoint
2. Notice the padlock icon 🔒 indicating it's protected
3. Click **"Try it out"**
4. Click **"Execute"**
5. You should see your user information:
   ```json
   {
     "id": "user-guid",
     "username": "testuser",
     "email": "testuser@example.com",
     "createdAt": "2024-01-20T10:30:00Z",
     "lastLoginAt": "2024-01-20T10:30:00Z"
   }
   ```

### 5. Test Login Flow

If you want to test the login endpoint:

1. Find **POST /api/auth/login**
2. Use either username or email:
   ```json
   {
     "usernameOrEmail": "testuser",
     "password": "Test123!"
   }
   ```
3. You'll receive a new JWT token

## Configuration

JWT settings can be modified in `appsettings.json`:

```json
{
  "JwtSettings": {
    "SecretKey": "YOUR_SECRET_KEY_HERE",
    "Issuer": "CSharpServer",
    "Audience": "CSharpServerUsers",
    "ExpirationHours": 24
  }
}
```

**Important**: Change the `SecretKey` for production use!

## Security Considerations

- Passwords are hashed using ASP.NET Core Identity (BCrypt)
- JWT tokens are signed with HMAC-SHA256
- HTTPS should be enabled in production
- Secret keys should be stored in environment variables or Azure Key Vault
- Consider implementing refresh tokens for better security

## Project Structure

```
/Controllers
  - HelloController.cs      # Simple REST endpoint
  - AuthController.cs       # Authentication endpoints
/Models
  - User.cs                # User entity
  - LoginDto.cs           # Login request model
  - RegisterDto.cs        # Registration request model
  - AuthResponseDto.cs    # Authentication response model
/Services
  - AuthService.cs        # Authentication business logic
  - TokenService.cs       # JWT token generation
/Data
  - AppDbContext.cs       # Entity Framework database context
Program.cs                # Application configuration
appsettings.json         # Application settings
```

## Development Tips

- The SQLite database file (`app.db`) is created automatically
- Use Swagger UI for easy API testing during development
- Check the console output for Entity Framework SQL queries
- JWT tokens can be decoded at https://jwt.io for debugging

## License

This project is open source and available under the MIT License.