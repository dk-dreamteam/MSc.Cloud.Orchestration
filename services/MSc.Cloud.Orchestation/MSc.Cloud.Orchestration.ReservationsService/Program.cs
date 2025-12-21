using MSc.Cloud.Orchestration.Common;
using System.Reflection;

// get configuration from environment variables.
var dbConnStr = Environment.GetEnvironmentVariable(NamesValues.EnvironmentVariables.PostgresConnectionString);
ArgumentNullException.ThrowIfNull(dbConnStr, $"{NamesValues.EnvironmentVariables.PostgresConnectionString} environment variable is not set.");

var supabaseSendEmailFuncUrl = Environment.GetEnvironmentVariable(NamesValues.EnvironmentVariables.SupabaseSendEmailFunctionUrl);
ArgumentNullException.ThrowIfNull(dbConnStr, $"{NamesValues.EnvironmentVariables.SupabaseSendEmailFunctionUrl} environment variable is not set.");

Console.WriteLine($"Using PostgreSQL connection string: {dbConnStr}");
Console.WriteLine($"Using Supabase Send Email Function URL: {supabaseSendEmailFuncUrl}");

var builder = WebApplication.CreateBuilder(args);

// add commmon services.
builder.Services.AddCommon(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(c =>
{
    // add xml comments to swagger.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// use api documentation.
app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();