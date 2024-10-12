# Base image for running the .NET app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
ENV ASPNETCORE_ENVIRONMENT=Production

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src
COPY ["CustomCADs.API/CustomCADs.API.csproj", "CustomCADs.API/"]
COPY ["CustomCADs.Infrastructure/CustomCADs.Infrastructure.csproj", "CustomCADs.Infrastructure/"]
COPY ["CustomCADs.Domain/CustomCADs.Domain.csproj", "CustomCADs.Domain/"]
COPY ["CustomCADs.Application/CustomCADs.Application.csproj", "CustomCADs.Application/"]
COPY ["CustomCADs.Auth/CustomCADs.Auth.csproj", "CustomCADs.Auth/"]

RUN dotnet restore "./CustomCADs.API/CustomCADs.API.csproj"

COPY . .
WORKDIR "/src/CustomCADs.API"

RUN dotnet build "./CustomCADs.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CustomCADs.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

USER root
RUN mkdir -p /app/wwwroot/files && chmod -R 777 /app/wwwroot/files
USER app

ENTRYPOINT ["dotnet", "CustomCADs.API.dll"]
