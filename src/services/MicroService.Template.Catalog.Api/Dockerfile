# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 6880
EXPOSE 6881


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/services/MicroService.Template.Catalog.Api/MicroService.Template.Catalog.Api.csproj", "src/services/MicroService.Template.Catalog.Api/"]
COPY ["src/Infrastructure/MicroService.Template.PostgreSQL/MicroService.Template.PostgreSQL.csproj", "src/Infrastructure/MicroService.Template.PostgreSQL/"]
COPY ["src/core/buildingBlocks/BuildingBlocks.csproj", "src/core/buildingBlocks/"]
COPY ["src/Infrastructure/MicroService.Template.Redis/MicroService.Template.Redis.csproj", "src/Infrastructure/MicroService.Template.Redis/"]
RUN dotnet restore "./src/services/MicroService.Template.Catalog.Api/MicroService.Template.Catalog.Api.csproj"
COPY . .
WORKDIR "/src/src/services/MicroService.Template.Catalog.Api"
RUN dotnet build "./MicroService.Template.Catalog.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MicroService.Template.Catalog.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroService.Template.Catalog.Api.dll"]