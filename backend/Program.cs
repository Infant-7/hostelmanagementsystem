using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using HostelManagement.API.Data;
using HostelManagement.API.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Entity Framework with MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    try
    {
        // Use ServerVersion.AutoDetect for automatic version detection
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }
    catch (Exception ex)
    {
        throw new InvalidOperationException($"Failed to configure MySQL: {ex.Message}. Please check your connection string and ensure MySQL server is running.", ex);
    }
});

// Configure Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;

    // User settings
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero,
        // Map role claim for authorization
        RoleClaimType = ClaimTypes.Role
    };
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// HTTPS redirection is disabled for development (HTTP only)
// Uncomment the line below if you want to enable HTTPS
// app.UseHttpsRedirection();

app.UseCors("AllowReactApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Add a simple root route
app.MapGet("/", () => Results.Redirect("/swagger"));

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        logger.LogInformation("Checking database connection...");
        
        // Try to create database first if it doesn't exist
        try
        {
            // Create database if it doesn't exist
            context.Database.ExecuteSqlRaw("CREATE DATABASE IF NOT EXISTS HostelManagementDB;");
            logger.LogInformation("Database created or already exists.");
        }
        catch (Exception dbEx)
        {
            logger.LogWarning("Could not create database automatically: {Message}", dbEx.Message);
            logger.LogWarning("Please create the database manually or run the SQL script: backend/database.sql");
        }
        
        // Now ensure all tables are created
        if (context.Database.CanConnect())
        {
            logger.LogInformation("Database connection successful. Creating tables...");
            context.Database.EnsureCreated();
            logger.LogInformation("Database and tables ensured/created successfully.");
        }
        else
        {
            logger.LogError("Cannot connect to database. Please:");
            logger.LogError("1. Ensure MySQL server is running");
            logger.LogError("2. Create the database manually: CREATE DATABASE HostelManagementDB;");
            logger.LogError("3. Or run the SQL script: backend/database.sql");
        }
    }
    catch (Exception ex) when (ex.GetType().Name.Contains("MySql") || ex.Message.Contains("MySQL") || ex.Message.Contains("MySql"))
    {
        logger.LogError(ex, "MySQL Error: {Message}", ex.Message);
        logger.LogError("Please check:");
        logger.LogError("1. MySQL server is running");
        logger.LogError("2. Connection string in appsettings.json is correct");
        logger.LogError("3. Database user has proper permissions");
        logger.LogError("4. Run the SQL script manually: backend/database.sql");
        logger.LogError("Connection string: {ConnectionString}", connectionString?.Replace("Password=", "Password=***"));
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred creating the DB: {Message}", ex.Message);
        logger.LogError("Stack trace: {StackTrace}", ex.StackTrace);
    }
}

app.Run();
