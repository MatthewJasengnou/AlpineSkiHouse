# Use the official ASP.NET Core runtime image from Microsoft:
    FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
    WORKDIR /app
    EXPOSE 80
    EXPOSE 443
    
    # Use the SDK image to build the application:
    FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
    WORKDIR /src
    COPY ["AlpineSkiHouse.csproj", "./"]
    RUN dotnet restore "AlpineSkiHouse.csproj"
    COPY . .
    WORKDIR "/src/."
    RUN dotnet build "AlpineSkiHouse.csproj" -c Release -o /app/build
    
    FROM build AS publish
    RUN dotnet publish "AlpineSkiHouse.csproj" -c Release -o /app/publish
    
    # Copy the published application to the official ASP.NET Core runtime image:
    FROM base AS final
    WORKDIR /app
    COPY --from=publish /app/publish .
    ENTRYPOINT ["dotnet", "AlpineSkiHouse.dll"]
    