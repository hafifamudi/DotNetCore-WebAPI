FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["/WebService.API/WebService.API.csproj", "WebService.API/"]
RUN dotnet restore "WebService.API/WebService.API.csproj"

COPY . .
WORKDIR "/src/WebService.API"
RUN dotnet build "WebService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebService.API.dll"]
