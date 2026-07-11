FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MapleDreams/MapleDreams.csproj", "MapleDreams/"]
RUN dotnet restore "MapleDreams/MapleDreams.csproj"
COPY . .
WORKDIR "/src/MapleDreams"
RUN dotnet build "MapleDreams.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MapleDreams.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MapleDreams.dll"]