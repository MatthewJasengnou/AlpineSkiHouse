# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

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