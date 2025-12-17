using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MSc.Cloud.Orchestration.Common.Repositories;
using MSc.Cloud.Orchestration.Common.Repositories.Interfaces;
using Npgsql;
using System.Data;

namespace MSc.Cloud.Orchestration.Common;

public static class RegisterServices
{
    public static IServiceCollection AddCommon(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        // get configuration from environment variables.
        var dbConnStr = Environment.GetEnvironmentVariable(NamesValues.EnvironmentVariables.PostgresConnectionString);
        ArgumentNullException.ThrowIfNull(dbConnStr, $"{NamesValues.EnvironmentVariables.PostgresConnectionString} environment variable is not set.");

        serviceCollection.AddScoped<IDbConnection>(_ => new NpgsqlConnection(dbConnStr));

        serviceCollection.AddScoped<IReservationRepository, ReservationRepository>();
        serviceCollection.AddScoped<IEventRepository, EventRepository>();

        return serviceCollection;
    }
}
