# Understanding EF Core Migrations with Multiple Providers

This guide explains the changes made to support both SQL Server and PostgreSQL in the same project, and how to handle this in production.

## 1. What We Changed and Why

### The Problem
When you have one application supporting multiple database providers (SQL Server and PostgreSQL), EF Core can get confused about which migrations belong to which database.
- SQL Server uses data types like `nvarchar` and `datetime2`.
- PostgreSQL uses `text` and `timestamp with time zone`.

If you try to create a PostgreSQL migration but EF Core looks at SQL Server migrations, it might try to "fix" the data types, leading to errors or incorrect schema changes.

### The Solution
We separated the migrations into different folders and configured `Program.cs` to be explicit.

#### 1. Separate Folders
- **SQL Server**: `TaskManagerApi.Infrastructure/SqlServerMigrations`
- **PostgreSQL**: `TaskManagerApi.Infrastructure/Data/PgMigrations`

#### 2. Explicit Configuration in `Program.cs`
We added this configuration:

```csharp
options.UseNpgsql(connectionString, x => x.MigrationsAssembly("TaskManagerApi.Infrastructure")
    .MigrationsHistoryTable("__EFMigrationsHistory", "public"));
```

- **`MigrationsAssembly`**: Tells EF Core explicitly where the migration code lives.
- **`MigrationsHistoryTable`**: This is the table EF Core uses to track which migrations have been applied. Specifying it explicitly ensures PostgreSQL uses the correct schema (`public`).

## 2. How to Handle Production

In a production environment, you generally **do not create** migrations. You only **apply** them.

### Step 1: Configuration
In your production environment (e.g., Azure App Service, AWS, Docker), you will use **Environment Variables** to override the settings, so you don't need to change the code.

**Environment Variables for PostgreSQL Prod:**
- `DatabaseProvider` = `PostgreSQL`
- `ConnectionStrings__PgDefaultConnection` = `Host=prod-db-host;Port=5432;Database=prod_db;Username=prod_user;Password=prod_pass;`

### Step 2: Applying Migrations
There are three common ways to apply migrations in production:

#### Option A: SQL Script (Recommended for strict control)
Generate a SQL script and give it to your DBA or run it manually.
```bash
# Generate script from empty to latest
dotnet ef migrations script --output migrate_pg.sql --context AppDbContext --project TaskManagerApi.Infrastructure --startup-project TaskManagerApi
```

#### Option B: Migration Bundles (Best for DevOps pipelines)
Create a standalone executable that applies migrations.
```bash
dotnet ef migrations bundle --output efbundle --context AppDbContext --project TaskManagerApi.Infrastructure --startup-project TaskManagerApi
# Then on the server, just run:
./efbundle
```

#### Option C: Programmatic Apply (Easiest for small apps)
Add code to `Program.cs` to automatically apply migrations when the app starts.

**Warning**: This can cause issues if you have multiple instances of your app starting at the same time.

```csharp
// In Program.cs, before app.Run()
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}
```

## 3. Summary of Commands

### Development (Creating Migrations)
You only do this on your local machine.

**For SQL Server:**
```bash
dotnet ef migrations add <Name> --context AppDbContext --output-dir SqlServerMigrations
```

**For PostgreSQL:**
```bash
dotnet ef migrations add <Name> --context AppDbContext --output-dir Data/PgMigrations
```

### Production (Applying Migrations)
You never run `migrations add` in production. You just ensure the `DatabaseProvider` is set correctly, and then apply the existing migrations to the database.
