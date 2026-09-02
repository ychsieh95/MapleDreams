# syntax=docker/dockerfile:1

# Build from the repository root, the Dockerfile lives in Docker/:
#   docker build -t mapledreams -f Docker/Dockerfile .

# =============================================================================
# Build - restore, compile and publish the app
# =============================================================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Restore NuGet packages and local tools first, in their own layers, so neither
# is repeated while only sources change.
COPY MapleDreams.csproj ./
RUN dotnet restore MapleDreams.csproj

COPY .config/dotnet-tools.json .config/
RUN dotnet tool restore

COPY . .

# wwwroot/lib is not tracked in git, restore it from libman.json
RUN dotnet libman restore

RUN dotnet publish MapleDreams.csproj \
        --configuration "$BUILD_CONFIGURATION" \
        --no-restore \
        --output /app/publish \
        -p:UseAppHost=false

# =============================================================================
# Final - ASP.NET Core runtime only, running as a non-root user
# =============================================================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build --chown=$APP_UID:$APP_UID /app/publish ./

# Written at runtime: the SQLite database (created on start when missing) and
# the error logs, so both have to be writable by the non-root user.
RUN mkdir -p files/database wwwroot/logs \
    && chown -R "$APP_UID:$APP_UID" files wwwroot/logs

# Non-root user shipped with the base image, uid 1654
USER $APP_UID

# Kestrel listens here, the base image sets ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "MapleDreams.dll"]
