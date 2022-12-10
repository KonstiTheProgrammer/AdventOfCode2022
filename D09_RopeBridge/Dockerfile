FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["D09_RopeBridge/D09_RopeBridge.csproj", "D09_RopeBridge/"]
RUN dotnet restore "D09_RopeBridge/D09_RopeBridge.csproj"
COPY . .
WORKDIR "/src/D09_RopeBridge"
RUN dotnet build "D09_RopeBridge.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "D09_RopeBridge.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "D09_RopeBridge.dll"]
