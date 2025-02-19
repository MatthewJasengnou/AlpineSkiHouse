# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore --verbosity detailed

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out --no-restore --verbosity detailed

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Environment configuration
ENV ASPNETCORE_URLS=http://+:5000
ENV EnableSwagger=true
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 5000
CMD ["dotnet", "AlpineSkiHouse.dll"]
