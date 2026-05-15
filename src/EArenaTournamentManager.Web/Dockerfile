FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/EArenaTournamentManager.Web/EArenaTournamentManager.Web.csproj", "EArenaTournamentManager.Web/"]
RUN dotnet restore "EArenaTournamentManager.Web/EArenaTournamentManager.Web.csproj"
COPY src/EArenaTournamentManager.Web/. ./EArenaTournamentManager.Web
WORKDIR "/src/EArenaTournamentManager.Web"
RUN dotnet publish "EArenaTournamentManager.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "EArenaTournamentManager.Web.dll"]