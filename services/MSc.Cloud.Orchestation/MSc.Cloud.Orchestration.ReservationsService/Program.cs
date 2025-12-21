using MSc.Cloud.Orchestration.Common;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// add commmon services.
builder.Services.AddCommon(builder.Configuration);
builder.Services.AddSendEmailService(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(c =>
{
    // add xml comments to swagger.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// add health checks.
builder.Services.AddPostgresHealthChecks(builder.Configuration);
builder.Services.AddSupabaseHealthChecks(builder.Configuration);

var app = builder.Build();

// use api documentation.
app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.MapGet("/", () => "Reservations Service is alive!").ExcludeFromDescription();
app.MapHealthChecks("/health", CommonHealthCheckOptions.Default);

app.Run();