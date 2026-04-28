# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["EyeroneApi/EyeroneApi.csproj", "EyeroneApi/"]
COPY ["Eyerone.Application/Eyerone.Application.csproj", "Eyerone.Application/"]
COPY ["Eyerone.Domain/Eyerone.Domain.csproj", "Eyerone.Domain/"]
COPY ["Eyerone.Infrastructure/Eyerone.Infrastructure.csproj", "Eyerone.Infrastructure/"]
RUN dotnet restore "./EyeroneApi/EyeroneApi.csproj"
COPY . .
WORKDIR "/src/EyeroneApi"
RUN dotnet build "./EyeroneApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EyeroneApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EyeroneApi.dll"]