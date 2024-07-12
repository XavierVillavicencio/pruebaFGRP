FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["pruebaFGRP.csproj", "."]
RUN dotnet restore "./pruebaFGRP.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./pruebaFGRP.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Crear y aplicar las migraciones
RUN dotnet ef migrations add InitialCreate

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./pruebaFGRP.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Instalar la herramienta dotnet-ef
RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"

# Script de inicio
COPY entrypoint.sh .
RUN chmod +x entrypoint.sh
ENTRYPOINT ["/app/entrypoint.sh"]