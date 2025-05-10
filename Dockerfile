FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base
WORKDIR /app
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ServerMonitor.sln", "."]
COPY ["ServerMonitor.Bot/ServerMonitor.Bot.csproj", "ServerMonitor.Bot/"]
COPY ["ServerMonitor.Database/ServerMonitor.Database.csproj", "ServerMonitor.Database/"]
COPY ["ServerMonitor.Startup/ServerMonitor.Startup.csproj", "ServerMonitor.Startup/"]
COPY ["ServerMonitor.Website/ServerMonitor.Website.csproj", "ServerMonitor.Website/"]
COPY ["ServerMonitor.Models/ServerMonitor.Models.csproj", "ServerMonitor.Models/"]
RUN dotnet restore
COPY . .
WORKDIR "/src/ServerMonitor.Startup"
RUN dotnet build "ServerMonitor.Startup.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ServerMonitor.Startup.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ServerMonitor.Startup.dll"]
