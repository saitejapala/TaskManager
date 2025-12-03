# Task Manager API

A robust RESTful API for managing work items and tasks, built with **.NET Core** and following **Clean Architecture** principles. This project supports multiple database providers (**SQL Server** and **PostgreSQL**) with separate migration histories.

## 🚀 Features

- **CRUD Operations**: Create, Read, Update, and Delete work items.
- **Clean Architecture**: Separation of concerns into Domain, Application, Infrastructure, and API layers.
- **Multi-Database Support**: Easily switch between SQL Server and PostgreSQL via configuration.
- **Separate Migrations**: Dedicated migration folders for each database provider to ensure compatibility.
- **Swagger UI**: Interactive API documentation.
- **Health Checks**: Built-in health monitoring endpoint.
- **Dependency Injection**: Loose coupling using .NET Core's built-in DI container.

## 🛠️ Tech Stack

- **Framework**: .NET 8.0 / 9.0 (ASP.NET Core Web API)
- **ORM**: Entity Framework Core
- **Databases**:
  - Microsoft SQL Server
  - PostgreSQL (via Npgsql)
- **Documentation**: Swashbuckle (Swagger)

## 📂 Project Structure

The solution follows the Clean Architecture pattern:

- **TaskManagerApi.Domain**: Core entities, interfaces, and business logic. (No dependencies)
- **TaskManagerApi.Application**: Application logic, DTOs, service interfaces, and mapping. (Depends on Domain)
- **TaskManagerApi.Infrastructure**: Database context, repositories, migrations, and external service implementations. (Depends on Application & Domain)
- **TaskManagerApi**: The entry point (Web API), controllers, and DI configuration. (Depends on Application & Infrastructure)

## ⚙️ Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (Version 8.0 or later)
- [SQL Server](https://www.microsoft.com/sql-server/) (LocalDB or full instance) OR [PostgreSQL](https://www.postgresql.org/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/TaskManager.git
   cd TaskManager
   ```

2. **Configure Database Connection**
   Open `TaskManagerApi/appsettings.json` and update the connection strings for your environment:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=.;Database=taskmanager_db;User Id=sa;Password=YourPassword;TrustServerCertificate=True;",
     "PgDefaultConnection": "Host=localhost;Port=5432;Database=taskmanager_db;Username=postgres;Password=YourPassword;"
   }
   ```

3. **Select Database Provider**
   In `TaskManagerApi/appsettings.json`, set the `DatabaseProvider` to either `"SqlServer"` or `"PostgreSQL"`:

   ```json
   "DatabaseProvider": "PostgreSQL" // or "SqlServer"
   ```

### 🗄️ Database Migrations

This project maintains separate migration folders for each provider.

#### For SQL Server
1. Set `"DatabaseProvider": "SqlServer"` in `appsettings.json`.
2. Run the following commands:
   ```bash
   # Apply migrations
   dotnet ef database update --project TaskManagerApi.Infrastructure --startup-project TaskManagerApi --context AppDbContext
   ```

#### For PostgreSQL
1. Set `"DatabaseProvider": "PostgreSQL"` in `appsettings.json`.
2. Run the following commands:
   ```bash
   # Apply migrations
   dotnet ef database update --project TaskManagerApi.Infrastructure --startup-project TaskManagerApi --context AppDbContext
   ```

### Creating New Migrations

If you modify the entities, create migrations specifying the correct output directory or context configuration.

**SQL Server:**
```bash
dotnet ef migrations add <MigrationName> --project TaskManagerApi.Infrastructure --startup-project TaskManagerApi --context AppDbContext --output-dir SqlServerMigrations
```

**PostgreSQL:**
```bash
dotnet ef migrations add <MigrationName> --project TaskManagerApi.Infrastructure --startup-project TaskManagerApi --context AppDbContext --output-dir Data/PgMigrations
```

## ▶️ Running the Application

1. Navigate to the API project folder:
   ```bash
   cd TaskManagerApi
   ```
2. Run the application:
   ```bash
   dotnet run
   ```
3. Open your browser and navigate to the Swagger UI:
   ```
   https://localhost:7040/swagger
   ```
   *(Note: Port may vary, check the launch logs)*

4. Check the application health:
   ```
   https://localhost:7040/health
   ```
   Should return `Healthy`.

## 📄 License

This project is licensed under the MIT License.
