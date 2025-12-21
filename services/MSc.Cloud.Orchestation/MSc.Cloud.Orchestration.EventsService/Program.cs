using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using MSc.Cloud.Orchestration.Common;
using System.Data;
using System.Reflection;
using System.Text.Json;

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

// add health checks.
builder.Services.AddPostgresHealthChecks(builder.Configuration);

var app = builder.Build();

// use api documentation.
app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();
app.MapGet("/", () => "Events Service is alive!").ExcludeFromDescription();
app.MapHealthChecks("/health", CommonHealthCheckOptions.Default);

app.Run();
