FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Danske/Danske.csproj", "Danske/"]
COPY ["Danske.Application/Danske.Application.csproj", "Danske.Application/"]
COPY ["Danske.Domain/Danske.Domain.csproj", "Danske.Domain/"]
COPY ["Danske.Infrastructure/Danske.Infrastructure.csproj", "Danske.Infrastructure/"]
RUN dotnet restore "./Danske/Danske.csproj"
COPY . .
WORKDIR "/src/Danske"
RUN dotnet build "./Danske.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Danske.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

USER root
RUN mkdir -p /app/Data && chown -R $APP_UID:$APP_UID /app/Data
USER $APP_UID
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Danske.dll"]