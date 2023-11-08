# Use the ASP.NET base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
ENV ASPNETCORE_ENVIRONMENT = Development

# Copy the solution file and individual project files 
COPY *.sln .
COPY WalletService.API/ WalletService.API/
COPY WalletService.Domain/ WalletService.Domain/
COPY WalletService.Persistence/ WalletService.Persistence/
COPY WalletService.Application/ WalletService.Application/
COPY WalletService.Infrastructure/ WalletService.Infrastructure/
COPY WalletService.DrawSignal.API/ WalletService.DrawSignal.API/
COPY WalletService.Common/ WalletService.Common/

# Restore NuGet packages for the entire solution
RUN dotnet restore

# Copy the rest of the source files
COPY . .

# Build the main application
WORKDIR /src/WalletService.API
RUN dotnet build WalletService.API.csproj -c Debug -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish WalletService.API.csproj -c Debug -o /app/publish /p:UseAppHost=false

# Final stage to setup the runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WalletService.API.dll"]