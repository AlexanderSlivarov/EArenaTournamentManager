FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/EArenaTournamentManager.API/EArenaTournamentManager.API.csproj", "EArenaTournamentManager.API/"]
COPY ["src/EArenaTournamentManager.Application/EArenaTournamentManager.Application.csproj", "EArenaTournamentManager.Application/"]
COPY ["src/EArenaTournamentManager.Infrastructure/EArenaTournamentManager.Infrastructure.csproj", "EArenaTournamentManager.Infrastructure/"]
COPY ["src/EArenaTournamentManager.Domain/EArenaTournamentManager.Domain.csproj", "EArenaTournamentManager.Domain/"]
RUN dotnet restore "EArenaTournamentManager.API/EArenaTournamentManager.API.csproj"
COPY src/EArenaTournamentManager.API/. ./EArenaTournamentManager.API
COPY src/EArenaTournamentManager.Application/. ./EArenaTournamentManager.Application
COPY src/EArenaTournamentManager.Infrastructure/. ./EArenaTournamentManager.Infrastructure
COPY src/EArenaTournamentManager.Domain/. ./EArenaTournamentManager.Domain
WORKDIR "/src/EArenaTournamentManager.API"
RUN dotnet publish "EArenaTournamentManager.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "EArenaTournamentManager.API.dll"]