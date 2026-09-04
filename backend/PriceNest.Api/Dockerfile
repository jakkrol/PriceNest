FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["PriceNest.Api.csproj", "./"]
RUN dotnet restore "PriceNest.Api.csproj"

COPY . .
RUN dotnet publish "PriceNest.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# --- ETAP 2: Lekki obraz uruchomieniowy ---
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "PriceNest.Api.dll"]