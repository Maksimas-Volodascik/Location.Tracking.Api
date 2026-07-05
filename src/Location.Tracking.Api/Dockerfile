# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
ARG APP_UID=1000 
USER $APP_UID
WORKDIR /app
EXPOSE 8080


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Location.Tracking.Api/Location.Tracking.Api.csproj", "src/Location.Tracking.Api/"]
COPY ["src/Location.Tracking.Application/Location.Tracking.Application.csproj", "src/Location.Tracking.Application/"]
COPY ["src/Location.Tracking.Domain/Location.Tracking.Domain.csproj", "src/Location.Tracking.Domain/"]
COPY ["src/Location.Tracking.Infrastructure/Location.Tracking.Infrastructure.csproj", "src/Location.Tracking.Infrastructure/"]
RUN dotnet restore "./src/Location.Tracking.Api/Location.Tracking.Api.csproj"
COPY . .
WORKDIR "/src/src/Location.Tracking.Api"
RUN dotnet build "./Location.Tracking.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Location.Tracking.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "Location.Tracking.Api.dll"]