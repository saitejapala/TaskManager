using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Data.Common;
using TaskManagerApi.Application.Interfaces;
using TaskManagerApi.Application.Services;
using TaskManagerApi.Domain.Interfaces;
using TaskManagerApi.Infrastructure.Data;
using TaskManagerApi.Infrastructure.Repositories;
using TaskManagerApi.Middleware;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string? DbPorovider = builder.Configuration.GetValue<string>("DatabaseProvider");
string connectionString = string.Empty;

builder.Services.AddDbContext<TaskManagerApi.Infrastructure.Data.AppDbContext>(options =>
{
    if (DbPorovider == "PostgreSQL")
    {
        connectionString = builder.Configuration.GetConnectionString("PgDefaultConnection") ?? string.Empty;
    }
    else // Default to SqlServer
    {
        connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
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
