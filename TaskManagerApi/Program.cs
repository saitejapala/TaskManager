using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Application.Interfaces;
using TaskManagerApi.Application.Services;
using TaskManagerApi.Domain.Interfaces;
using TaskManagerApi.Infrastructure.Repositories;
using TaskManagerApi.Middleware;
using TaskManagerApi.Security.Services.Classes;
using TaskManagerApi.Security.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

string connectionString = string.Empty;

//Dependency Injection for Repositories and Services
#region Services and Repository
builder.Services.AddDbContext<TaskManagerApi.Infrastructure.Data.AppDbContext>(options =>
{
    connectionString = Configuration.GetConnectionString("PgDefaultConnection") ?? string.Empty;
    options.UseNpgsql(connectionString, x => x.MigrationsAssembly("TaskManagerApi.Infrastructure"));
});

builder.Services.AddScoped<IWorkItemRepository, WorkItemRepository>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();

builder.Services.AddScoped<IWorkItemService, WorkItemService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<ITokenService, TokenService>();

#region Authenticatication
if (string.IsNullOrWhiteSpace(Configuration["Jwt:Key"]))
    throw new InvalidOperationException("JWT Key is not configured in appsettings.json");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = Configuration["Jwt:Issuer"],
        ValidAudience = Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"] ?? string.Empty)),
        ClockSkew = TimeSpan.Zero
    }
);
#endregion
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();

app.Run();
