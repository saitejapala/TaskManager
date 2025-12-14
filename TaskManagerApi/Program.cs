using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Options;
using System.Data.Common;
using TaskManagerApi.Application.Interfaces;
using TaskManagerApi.Application.Services;
using TaskManagerApi.Domain.Interfaces;
using TaskManagerApi.Infrastructure.Data;
using TaskManagerApi.Infrastructure.Repositories;
using TaskManagerApi.Middleware;
var builder = WebApplication.CreateBuilder(args);

var Configuration = builder.Configuration;
var allowedOrigins = Configuration
    .GetSection("AllowFrontend")
    .Get<string[]>() ?? Array.Empty<string>();

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
             .WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string? DbPorovider = Configuration.GetValue<string>("DatabaseProvider");
string connectionString = string.Empty;

builder.Services.AddDbContext<TaskManagerApi.Infrastructure.Data.AppDbContext>(options =>
{
    if (DbPorovider == "PostgreSQL")
    {
        connectionString = Configuration.GetConnectionString("PgDefaultConnection") ?? string.Empty;
        options.UseNpgsql(connectionString, x => x.MigrationsAssembly("TaskManagerApi.Infrastructure"));
    }
    else // Default to SqlServer
    {
        connectionString = Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        options.UseSqlServer(connectionString, x => x.MigrationsAssembly("TaskManagerApi.Infrastructure"));
    }
});

//Dependency Injection for Repositories and Services
#region Services and Repository

builder.Services.AddScoped<IWorkItemRepository, WorkItemRepository>();
builder.Services.AddScoped<IWorkItemService, WorkItemService>(); 

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
