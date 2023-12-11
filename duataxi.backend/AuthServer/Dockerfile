#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#  cd ..     docker build -f .\AuthServer\Dockerfile -t  duataxi.Auth:v1 .
#            docker run -p 5000:5000 --network=dshop-network  duataxi.Auth:v1     
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT docker
ENV ASPNETCORE_URLS http://*:5000
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["AuthServer/DuaTaxi.AuthServer.csproj", "AuthServer/"]
COPY ["DuaTaxi.Common/DuaTaxi.Common.csproj", "DuaTaxi.Common/"]
COPY ["DuaTaxi.DBContext/DuaTaxi.DBContext.csproj", "DuaTaxi.DBContext/"]
COPY ["DuaTaxi.Entities.Core/DuaTaxi.Entities.Core.csproj", "DuaTaxi.Entities.Core/"]
COPY ["AuthServer.Infrastructure/DuaTaxi.AuthServer.Infrastructure.csproj", "AuthServer.Infrastructure/"]
RUN dotnet restore "AuthServer/DuaTaxi.AuthServer.csproj"
COPY . .
WORKDIR "/src/AuthServer"
RUN dotnet build "DuaTaxi.AuthServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DuaTaxi.AuthServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DuaTaxi.AuthServer.dll","--environment=docker"]