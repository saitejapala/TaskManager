# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TaskManagerApi/TaskManagerApi.csproj", "TaskManagerApi/"]
COPY ["TaskManager.Email/TaskManager.Email.csproj", "TaskManager.Email/"]
COPY ["TaskManagerApi.Application/TaskManagerApi.Application.csproj", "TaskManagerApi.Application/"]
COPY ["TaskManagerApi.Domain/TaskManagerApi.Domain.csproj", "TaskManagerApi.Domain/"]
COPY ["TaskManagerApi.Security/TaskManagerApi.Security.csproj", "TaskManagerApi.Security/"]
COPY ["TaskManagerApi.Infrastructure/TaskManagerApi.Infrastructure.csproj", "TaskManagerApi.Infrastructure/"]
RUN dotnet restore "./TaskManagerApi/TaskManagerApi.csproj"
COPY . .
WORKDIR "/src/TaskManagerApi"
RUN dotnet build "./TaskManagerApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TaskManagerApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskManagerApi.dll"]