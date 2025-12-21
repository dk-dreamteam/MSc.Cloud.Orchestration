using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MSc.Cloud.Orchestration.Common.Repositories;
using MSc.Cloud.Orchestration.Common.Repositories.Interfaces;
using MSc.Cloud.Orchestration.Common.Services;
using Npgsql;
using System.Data;
using System.Net.Http;
using static MSc.Cloud.Orchestration.Common.NamesValues.HttpClientNames;

namespace MSc.Cloud.Orchestration.Common;

public static class RegisterServices
{
    private readonly static string EnvVariableIsNotSetMessage = "{0} environment variable is not set.";

    public static IServiceCollection AddCommon(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        // get configuration from environment variables.
        var dbConnStr = Environment.GetEnvironmentVariable(NamesValues.EnvironmentVariables.PostgresConnectionString);
        ArgumentNullException.ThrowIfNull(dbConnStr, string.Format(EnvVariableIsNotSetMessage, NamesValues.EnvironmentVariables.PostgresConnectionString));

        serviceCollection.AddScoped<IDbConnection>(_ => new NpgsqlConnection(dbConnStr));

        serviceCollection.AddScoped<IReservationRepository, ReservationRepository>();
        serviceCollection.AddScoped<IEventRepository, EventRepository>();

        return serviceCollection;
    }

    public static IServiceCollection AddPostgresHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        // get configuration from environment variables.
        var dbConnStr = Environment.GetEnvironmentVariable(NamesValues.EnvironmentVariables.PostgresConnectionString);
        ArgumentNullException.ThrowIfNull(dbConnStr, string.Format(EnvVariableIsNotSetMessage, NamesValues.EnvironmentVariables.PostgresConnectionString));

        services
            .AddHealthChecks()
            .AddNpgSql(
                connectionString: dbConnStr,
                name: "PostgreSQL",
                healthQuery: "SELECT 1;",
                failureStatus: HealthStatus.Unhealthy);

        return services;
    }

    public static IServiceCollection AddSupabaseHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        // get configuration from environment variables.
        var supabaseSendEmailFuncUrl = Environment.GetEnvironmentVariable(NamesValues.EnvironmentVariables.SupabaseSendEmailFunctionUrl);
        ArgumentNullException.ThrowIfNull(supabaseSendEmailFuncUrl, string.Format(EnvVariableIsNotSetMessage, NamesValues.EnvironmentVariables.SupabaseSendEmailFunctionUrl));

        var supabaseSendEmailFuncToken = Environment.GetEnvironmentVariable(NamesValues.EnvironmentVariables.SupabaseSendEmailFunctionToken);
        ArgumentNullException.ThrowIfNull(supabaseSendEmailFuncToken, string.Format(EnvVariableIsNotSetMessage, NamesValues.EnvironmentVariables.SupabaseSendEmailFunctionToken));

        services.AddHealthChecks()
            .AddAsyncCheck(
                name: "Supabase Serverless Send Email Function",
                async () =>
                {
                    using var client = new HttpClient();
                    var req = new HttpRequestMessage(
                        HttpMethod.Head, supabaseSendEmailFuncUrl);

                    req.Headers.Authorization = new("Bearer", supabaseSendEmailFuncToken);
                    var res = await client.SendAsync(req);

                    if (!res.IsSuccessStatusCode)
                    {
                        return new HealthCheckResult(HealthStatus.Unhealthy, description: "Supabase Function did not respond to HTTP HEAD request.");
                    }

                    return new HealthCheckResult(HealthStatus.Healthy);
                });

        return services;
    }

    public static IServiceCollection AddSendEmailService(this IServiceCollection services, IConfiguration configuration)
    {
        // get configuration from environment variables.
        var supabaseSendEmailFuncUrl = Environment.GetEnvironmentVariable(NamesValues.EnvironmentVariables.SupabaseSendEmailFunctionUrl);
        ArgumentNullException.ThrowIfNull(supabaseSendEmailFuncUrl, string.Format(EnvVariableIsNotSetMessage, NamesValues.EnvironmentVariables.SupabaseSendEmailFunctionUrl));

        var supabaseSendEmailFuncToken = Environment.GetEnvironmentVariable(NamesValues.EnvironmentVariables.SupabaseSendEmailFunctionToken);
        ArgumentNullException.ThrowIfNull(supabaseSendEmailFuncToken, string.Format(EnvVariableIsNotSetMessage, NamesValues.EnvironmentVariables.SupabaseSendEmailFunctionToken));

        var countdownMailUrl = Environment.GetEnvironmentVariable(NamesValues.EnvironmentVariables.CountdownMailUrl);
        ArgumentNullException.ThrowIfNull(countdownMailUrl, string.Format(EnvVariableIsNotSetMessage, NamesValues.EnvironmentVariables.CountdownMailUrl));

        var countdownMailToken = Environment.GetEnvironmentVariable(NamesValues.EnvironmentVariables.CountdownMailToken);
        ArgumentNullException.ThrowIfNull(countdownMailToken, string.Format(EnvVariableIsNotSetMessage, NamesValues.EnvironmentVariables.CountdownMailToken));

        // configure http clients.
        services.AddHttpClient(SupabaseClient, client => client.DefaultRequestHeaders.Authorization = new("Bearer", supabaseSendEmailFuncToken));
        services.AddHttpClient(GifClient, client => client.DefaultRequestHeaders.Authorization = new(countdownMailToken));

        // configure send email service.
        services.AddScoped<ISendEmailService, SendEmailService>((sp) => new SendEmailService(
            sp.GetRequiredService<IHttpClientFactory>(),
            countdownMailUrl,
            supabaseSendEmailFuncUrl));

        return services;
    }
}
