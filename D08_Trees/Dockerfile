FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["D08_Trees/D08_Trees.csproj", "D08_Trees/"]
RUN dotnet restore "D08_Trees/D08_Trees.csproj"
COPY . .
WORKDIR "/src/D08_Trees"
RUN dotnet build "D08_Trees.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "D08_Trees.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "D08_Trees.dll"]
