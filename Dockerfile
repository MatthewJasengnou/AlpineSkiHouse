# Use the official .NET SDK as the build environment
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy everything and restore dependencies
COPY . ./
RUN dotnet restore

# Build the application
RUN dotnet publish -c Release -o out

# Use the official ASP.NET Core runtime as the final container image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Expose the application on port 80
EXPOSE 80

# Run the application
ENTRYPOINT ["dotnet", "AlpineSkiHouse.dll"]
