FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5178

ENV ASPNETCORE_URLS=http://+:5178

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["WalletService.API/WalletService.API.csproj", "WalletService.API/"]
RUN dotnet restore "WalletService.API/WalletService.API.csproj"
COPY . .
WORKDIR "/src/WalletService.API"
RUN dotnet build "WalletService.API.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "WalletService.API.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WalletService.API.dll"]
