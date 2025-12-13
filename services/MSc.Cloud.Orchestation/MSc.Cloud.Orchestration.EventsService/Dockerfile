# we use alpine again for a smaller image size to begin with for the build part.
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine as build
WORKDIR /src

# copy the project's files and its depedencies and run dotnet restore to restore the dependencies.
COPY ["MSc.Cloud.Orchestration.EventsService/MSc.Cloud.Orchestration.EventsService.csproj", "MSc.Cloud.Orchestration.EventsService/"]
COPY ["MSc.Cloud.Orchestration.Common/MSc.Cloud.Orchestration.Common.csproj", "MSc.Cloud.Orchestration.Common/"]
RUN dotnet restore "MSc.Cloud.Orchestration.EventsService/MSc.Cloud.Orchestration.EventsService.csproj"

# copy to the working dir.
COPY . .

# build and publish app locally.
WORKDIR "/src/MSc.Cloud.Orchestration.EventsService"
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# we use the base image for runtime only. We use the alpine variant for a smaller image size only for the runtime.
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MSc.Cloud.Orchestration.EventsService.dll"]