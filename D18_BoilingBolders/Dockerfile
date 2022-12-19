FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["D18_BoilingBolders/D18_BoilingBolders.csproj", "D18_BoilingBolders/"]
RUN dotnet restore "D18_BoilingBolders/D18_BoilingBolders.csproj"
COPY . .
WORKDIR "/src/D18_BoilingBolders"
RUN dotnet build "D18_BoilingBolders.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "D18_BoilingBolders.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "D18_BoilingBolders.dll"]
