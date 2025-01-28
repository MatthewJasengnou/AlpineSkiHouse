# Use the official .NET SDK as the build environment
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy only project files first (to cache restore layer)
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the application source code
COPY . ./

# Build the application
RUN dotnet publish -c Release -o out

# Use the official ASP.NET Core runtime as the final container image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy the published output from the build stage
COPY --from=build-env /app/out .

# Expose the application port (Render will map this automatically)
EXPOSE 5000

# Ensure app listens on the correct port using Render's PORT environment variable
CMD ["dotnet", "AlpineSkiHouse.dll"]
