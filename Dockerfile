# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY CrimePortal/*.csproj CrimePortal/
RUN dotnet restore "CrimePortal/CrimePortal.csproj"

# Copy everything else and build
COPY CrimePortal/ CrimePortal/
WORKDIR /src/CrimePortal
RUN dotnet build "CrimePortal.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "CrimePortal.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Install curl for health checks (optional but useful for Render)
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Copy published app
COPY --from=publish /app/publish .

# Set environment variables
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

# Expose port
EXPOSE 80

# Run the application
ENTRYPOINT ["dotnet", "CrimePortal.dll"]
