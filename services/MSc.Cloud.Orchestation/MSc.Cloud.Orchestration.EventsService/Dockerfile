# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MSc.Cloud.Orchestration.EventsService/MSc.Cloud.Orchestration.EventsService.csproj", "MSc.Cloud.Orchestration.EventsService/"]
COPY ["MSc.Cloud.Orchestration.Common/MSc.Cloud.Orchestration.Common.csproj", "MSc.Cloud.Orchestration.Common/"]
RUN dotnet restore "./MSc.Cloud.Orchestration.EventsService/MSc.Cloud.Orchestration.EventsService.csproj"
COPY . .
WORKDIR "/src/MSc.Cloud.Orchestration.EventsService"
RUN dotnet build "./MSc.Cloud.Orchestration.EventsService.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MSc.Cloud.Orchestration.EventsService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MSc.Cloud.Orchestration.EventsService.dll"]