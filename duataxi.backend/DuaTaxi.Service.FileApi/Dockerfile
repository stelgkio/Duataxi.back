FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app


FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["DuaTaxi.Service.FileApi/DuaTaxi.Service.FileApi.csproj", "DuaTaxi.Service.FileApi/"]
RUN dotnet restore "DuaTaxi.Service.FileApi/DuaTaxi.Service.FileApi.csproj"
COPY . .
WORKDIR "/src/DuaTaxi.Service.FileApi"
RUN dotnet build "DuaTaxi.Service.FileApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DuaTaxi.Service.FileApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DuaTaxi.Service.FileApi.dll"]