FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["noteApp/noteApp/noteApp.csproj", "noteApp/noteApp/"]
COPY ["noteApp/noteApp.Client/noteApp.Client.csproj", "noteApp/noteApp.Client/"]
RUN dotnet restore "noteApp/noteApp/noteApp.csproj"
COPY . .
WORKDIR "/src/noteApp/noteApp"
RUN dotnet build "noteApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "noteApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "noteApp.dll"]
