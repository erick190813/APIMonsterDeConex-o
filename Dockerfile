FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["PokeApiBackend.csproj", "./"]
RUN dotnet restore "PokeApiBackend.csproj"

COPY . .
RUN dotnet build "PokeApiBackend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PokeApiBackend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PokeApiBackend.dll"]