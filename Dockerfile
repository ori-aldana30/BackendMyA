# Imagen base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MyAMIS.csproj", "./"]
RUN dotnet restore "./MyAMIS.csproj"
COPY . .
RUN dotnet publish "MyAMIS.csproj" -c Release -o /app/publish

# Final
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MyAMIS.dll"]