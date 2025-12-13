using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MSc.Cloud.Orchestration.Common;
using Npgsql;
using System.Data;
using System.Reflection;
using System.Text.Json;

// get configuration from environment variables.
var dbConnStr = Environment.GetEnvironmentVariable(NamesValues.EnvironmentVariables.PostgresConnectionString);
ArgumentNullException.ThrowIfNull(dbConnStr, $"{NamesValues.EnvironmentVariables.PostgresConnectionString} environment variable is not set.");

Console.WriteLine($"Using PostgreSQL connection string: {dbConnStr}");

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// register PostgreSQL connection
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

// use api documentation.
app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";

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
