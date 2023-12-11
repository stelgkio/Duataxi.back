#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
ENV ASPNETCORE_URLS http://*:5007
EXPOSE 5007


FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["DuaTaxi.ErrorHandlerSignalr/DuaTaxi.ErrorHandlerSignalr.csproj", "DuaTaxi.ErrorHandlerSignalr/"]
COPY ["DuaTaxi.Common/DuaTaxi.Common.csproj", "DuaTaxi.Common/"]
RUN dotnet restore "DuaTaxi.ErrorHandlerSignalr/DuaTaxi.ErrorHandlerSignalr.csproj"
COPY . .
WORKDIR "/src/DuaTaxi.ErrorHandlerSignalr"
RUN dotnet build "DuaTaxi.ErrorHandlerSignalr.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DuaTaxi.ErrorHandlerSignalr.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DuaTaxi.ErrorHandlerSignalr.dll"]