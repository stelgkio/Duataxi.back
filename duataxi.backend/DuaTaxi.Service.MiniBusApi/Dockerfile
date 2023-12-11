FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["DuaTaxi.Service.MiniBusApi/DuaTaxi.Service.MiniBusApi.csproj", "DuaTaxi.Service.MiniBusApi/"]
RUN dotnet restore "DuaTaxi.Service.MiniBusApi/DuaTaxi.Service.MiniBusApi.csproj"
COPY . .
WORKDIR "/src/DuaTaxi.Service.MiniBusApi"
RUN dotnet build "DuaTaxi.Service.MiniBusApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DuaTaxi.Service.MiniBusApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DuaTaxi.Service.MiniBusApi.dll"]