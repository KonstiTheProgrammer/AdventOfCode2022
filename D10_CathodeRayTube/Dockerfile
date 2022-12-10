FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["D10_CathodeRayTube/D10_CathodeRayTube.csproj", "D10_CathodeRayTube/"]
RUN dotnet restore "D10_CathodeRayTube/D10_CathodeRayTube.csproj"
COPY . .
WORKDIR "/src/D10_CathodeRayTube"
RUN dotnet build "D10_CathodeRayTube.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "D10_CathodeRayTube.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "D10_CathodeRayTube.dll"]
