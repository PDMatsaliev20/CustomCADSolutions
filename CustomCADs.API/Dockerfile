# Base image for running the .NET app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
ENV ASPNETCORE_ENVIRONMENT=Production

# Stage 1: Build the React frontend
FROM node:18 AS react-build
WORKDIR /app
# Copy package files and install dependencies
COPY customcads/package*.json ./
RUN npm install
# Copy source code and build the React app
COPY customcads/ ./
RUN npm run build

# Stage 2: Build the .NET application with Node.js installed
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

# Install Node.js in the .NET build image
RUN apt-get update && apt-get install -y \
    curl \
    && curl -fsSL https://deb.nodesource.com/setup_18.x | bash - \
    && apt-get install -y nodejs

WORKDIR /src
# Copy project files and restore dependencies
COPY ["customcads/nuget.config", "customcads/"]
COPY ["CustomCADs.API/CustomCADs.API.csproj", "CustomCADs.API/"]
COPY ["CustomCADs.Infrastructure/CustomCADs.Infrastructure.csproj", "CustomCADs.Infrastructure/"]
COPY ["CustomCADs.Domain/CustomCADs.Domain.csproj", "CustomCADs.Domain/"]
COPY ["customcads/customcads.esproj", "customcads/"]
COPY ["CustomCADs.Application/CustomCADs.Application.csproj", "CustomCADs.Application/"]

# Restore .NET dependencies
RUN dotnet restore "./CustomCADs.API/CustomCADs.API.csproj"

# Copy the remaining source code
COPY . .
WORKDIR "/src/CustomCADs.API"

# Build the .NET application
RUN dotnet build "./CustomCADs.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Stage 3: Publish the .NET application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CustomCADs.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 4: Final image with the built .NET app
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

USER root
RUN mkdir -p /app/wwwroot/files && chmod -R 777 /app/wwwroot/files
USER app

ENTRYPOINT ["dotnet", "CustomCADs.API.dll"]
