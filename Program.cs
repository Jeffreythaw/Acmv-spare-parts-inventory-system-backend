
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
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

var rawConnectionString = builder.Configuration.GetConnectionString("LS")
    ?? throw new InvalidOperationException("Missing connection string 'LS'.");
var dbConnectTimeoutSeconds = int.TryParse(Environment.GetEnvironmentVariable("DB_CONNECT_TIMEOUT_SECONDS"), out var connectTimeout)
    ? Math.Max(connectTimeout, 1)
    : 10;
var dbCommandTimeoutSeconds = int.TryParse(Environment.GetEnvironmentVariable("DB_COMMAND_TIMEOUT_SECONDS"), out var commandTimeout)
    ? Math.Max(commandTimeout, 1)
    : 15;
var sqlConnectionBuilder = new SqlConnectionStringBuilder(rawConnectionString)
{
    ConnectTimeout = dbConnectTimeoutSeconds
};
var lsConnectionString = sqlConnectionBuilder.ConnectionString;

builder.Services.AddDbContext<AcmvDbContext>(options =>
    options.UseSqlServer(lsConnectionString, sqlOptions =>
    {
        sqlOptions.CommandTimeout(dbCommandTimeoutSeconds);
        sqlOptions.EnableRetryOnFailure(3, TimeSpan.FromSeconds(5), null);
    }));

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

var runDbMigrations = !string.Equals(Environment.GetEnvironmentVariable("RUN_DB_MIGRATIONS"), "false", StringComparison.OrdinalIgnoreCase);
var migrationTimeoutSeconds = int.TryParse(Environment.GetEnvironmentVariable("DB_MIGRATION_TIMEOUT_SECONDS"), out var migrationTimeout)
    ? Math.Max(migrationTimeout, 1)
    : 30;
if (runDbMigrations)
{
    _ = Task.Run(async () =>
    {
        using var scope = app.Services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("DbStartup");
        var db = scope.ServiceProvider.GetRequiredService<AcmvDbContext>();
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(migrationTimeoutSeconds));
        try
        {
            logger.LogInformation("Starting DB migration/seed with timeout {Timeout}s", migrationTimeoutSeconds);
            await db.Database.MigrateAsync(cts.Token);
            await InventorySeeder.SeedAsync(db, logger, app.Environment.ContentRootPath, cts.Token);
            logger.LogInformation("DB migration/seed completed.");
        }
        catch (OperationCanceledException)
        {
            logger.LogWarning("DB migration/seed timed out after {Timeout}s. API remains available.", migrationTimeoutSeconds);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "DB migration/seed failed. API remains available.");
        }
    }, app.Lifetime.ApplicationStopping);
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
app.MapGet("/health/live", () => Results.Ok(new { status = "live" }));
app.MapGet("/health/ready", async (AcmvDbContext db, CancellationToken ct) =>
{
    try
    {
        using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        timeoutCts.CancelAfter(TimeSpan.FromSeconds(3));
        var canConnect = await db.Database.CanConnectAsync(timeoutCts.Token);
        return canConnect
            ? Results.Ok(new { status = "ready" })
            : Results.Problem("Database unavailable", statusCode: StatusCodes.Status503ServiceUnavailable);
    }
    catch
    {
        return Results.Problem("Database unavailable", statusCode: StatusCodes.Status503ServiceUnavailable);
    }
});

app.Run();
