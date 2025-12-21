using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MSc.Cloud.Orchestration.Common;
using System.Data;
using System.Reflection;
using System.Text.Json;

// get configuration from environment variables.
var dbConnStr = Environment.GetEnvironmentVariable(NamesValues.EnvironmentVariables.PostgresConnectionString);
ArgumentNullException.ThrowIfNull(dbConnStr, $"{NamesValues.EnvironmentVariables.PostgresConnectionString} environment variable is not set.");

Console.WriteLine($"Using PostgreSQL connection string: {dbConnStr}");

var builder = WebApplication.CreateBuilder(args);

// add commmon services.
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(c =>
{
    // add xml comments to swagger.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// add health checks
builder.Services.AddHealthChecks()
    .AddAsyncCheck(
        name: "SupaBase Function",
        async () =>
        {
            using var client = new HttpClient();
            var req = new HttpRequestMessage(
                HttpMethod.Head, "https://mlrejxahluybrnrizzmh.supabase.co/functions/v1/send-sms-notificaiton");

            //@dimgrev: you can get this from env var as well as the function url.
            req.Headers.Authorization = new("Bearer", "bearer token. use the env file.");
            var res = await client.SendAsync(req);

            if (!res.IsSuccessStatusCode)
            {
                return new HealthCheckResult(HealthStatus.Unhealthy, description: "SupaBase function is not healthy.");
            }

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
