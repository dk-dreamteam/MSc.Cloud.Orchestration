using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace MSc.Cloud.Orchestration.Common;

public class CommonHealthCheckOptions
{
    public static HealthCheckOptions Default => new()
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
    };
}
