# we use alpine again for a smaller image size to begin with for the dotnet building part.
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine as build
WORKDIR /src

# copy everything to the working dir.
COPY . .

# set as working dir the Events Service project folder.
WORKDIR "/src/MSc.Cloud.Orchestration.EventsService"

# run dotnet restore, build and publish commands.
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# we use the base image for runtime only. We use the alpine variant for a smaller image size only for the runtime.

# after successful dotnet build stage, we are going to use only the lightweight alpine runtime image.
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine
WORKDIR /app
EXPOSE 8080

# copy the published output from the building stage to the runtime image.
COPY --from=build /app/publish .

# run the application.
ENTRYPOINT ["dotnet", "MSc.Cloud.Orchestration.EventsService.dll"]