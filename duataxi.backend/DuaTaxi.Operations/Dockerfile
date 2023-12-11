FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 5004
EXPOSE 44304

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["DuaTaxi.Operations/DuaTaxi.Operations.csproj", "DuaTaxi.Operations/"]
RUN dotnet restore "DuaTaxi.Operations/DuaTaxi.Operations.csproj"
COPY . .
WORKDIR "/src/DuaTaxi.Operations"
RUN dotnet build "DuaTaxi.Operations.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DuaTaxi.Operations.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DuaTaxi.Operations.dll"]