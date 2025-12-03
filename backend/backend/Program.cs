using backend.Models;
using backend.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Register DbContext with SQL Server using the connection string from configuration
builder.Services.AddDbContext<HotelDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

// Seed database (runtime seeding avoids dynamic HasData problems)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<HotelDbContext>();
    context.Database.EnsureCreated();

    if (!context.RoomTypes.Any())
    {
        context.RoomTypes.AddRange(
            new RoomType { Name = "Single", Description = "Single bed", PricePerNight = 50 },
            new RoomType { Name = "Double", Description = "Double bed", PricePerNight = 80 },
            new RoomType { Name = "Suite", Description = "Suite", PricePerNight = 200 }
        );
        context.SaveChanges();
    }

    // Seed employees at runtime using PasswordHasher (produces dynamic values but not used in model comparison)
    if (!context.Employees.Any())
    {
        PasswordHasher.CreatePasswordHash("admin", out var hash1, out var salt1);
        PasswordHasher.CreatePasswordHash("admin", out var hash2, out var salt2);
        PasswordHasher.CreatePasswordHash("admin", out var hash3, out var salt3);

        context.Employees.AddRange(
            new Employee { Name = "Marek Papszun", Login = "maras", PasswordHash = hash1, PasswordSalt = salt1 },
            new Employee { Name = "Dawid Szwarga", Login = "szwagier", PasswordHash = hash2, PasswordSalt = salt2 },
            new Employee { Name = "Jacek Zielinski", Login = "jayz", PasswordHash = hash3, PasswordSalt = salt3 }
        );
        context.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
