#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 6001
EXPOSE 6002

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["gateway/MicroService.Template.ApiGateway/MicroService.Template.ApiGateway.csproj", "gateway/MicroService.Template.ApiGateway/"]
RUN dotnet restore "./gateway/MicroService.Template.ApiGateway/MicroService.Template.ApiGateway.csproj"
COPY . .
WORKDIR "/src/gateway/MicroService.Template.ApiGateway"
RUN dotnet build "./MicroService.Template.ApiGateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MicroService.Template.ApiGateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroService.Template.ApiGateway.dll"]