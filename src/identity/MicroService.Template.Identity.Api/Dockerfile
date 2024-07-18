# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 6181
EXPOSE 6182


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/identity/MicroService.Template.Identity.Api/MicroService.Template.Identity.Api.csproj", "src/identity/MicroService.Template.Identity.Api/"]
COPY ["src/identity/MicroService.Template.Identity/MicroService.Template.Identity.csproj", "src/identity/MicroService.Template.Identity/"]
COPY ["src/core/buildingBlocks/BuildingBlocks.csproj", "src/core/buildingBlocks/"]
COPY ["src/core/MicroService.Template.Shared/MicroService.Template.Shared.csproj", "src/core/MicroService.Template.Shared/"]
RUN dotnet restore "./src/identity/MicroService.Template.Identity.Api/MicroService.Template.Identity.Api.csproj"
COPY . .
WORKDIR "/src/src/identity/MicroService.Template.Identity.Api"
RUN dotnet build "./MicroService.Template.Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MicroService.Template.Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroService.Template.Identity.Api.dll"]