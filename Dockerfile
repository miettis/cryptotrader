#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CryptoTrader.sln", "."]
COPY ["CryptoTrader.Web/CryptoTrader.Web.csproj", "./CryptoTrader.Web/"]
COPY ["CryptoTrader.Data/CryptoTrader.Data.csproj", "./CryptoTrader.Data/"]
COPY ["CryptoTrader.Import/CryptoTrader.Import.csproj", "./CryptoTrader.Import/"]
#RUN dotnet restore "CryptoTrader.Web/CryptoTrader.Web.csproj"
RUN dotnet restore
COPY . .
WORKDIR "/src/."
RUN dotnet build "./CryptoTrader.Web/CryptoTrader.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CryptoTrader.Web/CryptoTrader.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CryptoTrader.Web.dll"]