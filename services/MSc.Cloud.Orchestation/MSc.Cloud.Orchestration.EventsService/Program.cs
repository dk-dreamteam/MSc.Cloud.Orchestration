using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Npgsql;
using System.Data;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// register PostgreSQL connection
var dbConnStr = Environment.GetEnvironmentVariable("POSTGRES_CONN_STR");
ArgumentNullException.ThrowIfNull(dbConnStr, "POSTGRES_CONN_STR environment variable is not set.");

builder.Services.AddScoped<IDbConnection>(sp =>
    new NpgsqlConnection(dbConnStr));

// add health checks
builder.Services.AddHealthChecks()
    .AddCheck(
        name: "SupaBase Function",
        () =>
        {
            return new HealthCheckResult(HealthStatus.Healthy);
        })
    .AddNpgSql(
        connectionString: dbConnStr,
        name: "PostgreSQL",
        healthQuery: "SELECT 1;",
        failureStatus: HealthStatus.Unhealthy);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        var response = new
        {
            status = report.Status.ToString(),
            totalDuration = report.TotalDuration,
            results = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                duration = e.Value.Duration,
            })
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
});

app.Run();
