FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NotesApp2/NotesApp2.Client/NotesApp2.Client.csproj", "NotesApp2/NotesApp2.Client/"]
RUN dotnet restore "NotesApp2/NotesApp2.Client/NotesApp2.Client.csproj"
COPY . .
WORKDIR "/src/NotesApp2/NotesApp2.Client"
RUN dotnet build "NotesApp2.Client.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "NotesApp2.Client.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NotesApp2.Client.dll"]
