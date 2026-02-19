
using Microsoft.EntityFrameworkCore;
using AcmvInventory.Data;
using AcmvInventory.Services;
using AcmvInventory.Middleware;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Render sets PORT dynamically. For local runs, default to 5100 to avoid common
// macOS conflicts on 5000 (AirPlay/ControlCenter).
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrWhiteSpace(port))
{
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
}
else if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("ASPNETCORE_URLS")))
{
    builder.WebHost.UseUrls("http://localhost:5100");
}

builder.Services.AddControllers()
    .AddJsonOptions(options => {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var lsConnectionString = builder.Configuration.GetConnectionString("LS")
    ?? throw new InvalidOperationException("Missing connection string 'LS'.");

builder.Services.AddDbContext<AcmvDbContext>(options =>
    options.UseSqlServer(lsConnectionString));

builder.Services.AddScoped<TransactionService>();
builder.Services.AddScoped<PurchasingService>();

var corsOrigins = (Environment.GetEnvironmentVariable("CORS_ORIGINS") ?? "")
    .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        if (corsOrigins.Length > 0)
        {
            policy.WithOrigins(corsOrigins)
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        }
        else
        {
            // Fallback for first deploy / local testing.
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        }
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AcmvDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("InventorySeeder");
    await db.Database.MigrateAsync();
    await InventorySeeder.SeedAsync(db, logger, app.Environment.ContentRootPath);
}

// Custom Global Exception Handler
app.UseMiddleware<ExceptionMiddleware>();

var enableSwagger = app.Environment.IsDevelopment() ||
                    string.Equals(Environment.GetEnvironmentVariable("ENABLE_SWAGGER"), "true", StringComparison.OrdinalIgnoreCase);
if (enableSwagger)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");
app.UseAuthorization();
app.MapControllers();
app.MapGet("/", () => Results.Ok(new { status = "ok", service = "acmv-backend" }));

app.Run();
