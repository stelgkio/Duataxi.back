#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 5002
EXPOSE 44302

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["DuaTaxi.Service.Payments/DuaTaxi.Service.Payments.csproj", "DuaTaxi.Service.Payments/"]
COPY ["DuaTaxi.Common/DuaTaxi.Common.csproj", "DuaTaxi.Common/"]
RUN dotnet restore "DuaTaxi.Service.Payments/DuaTaxi.Service.Payments.csproj"
COPY . .
WORKDIR "/src/DuaTaxi.Service.Payments"
RUN dotnet build "DuaTaxi.Service.Payments.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DuaTaxi.Service.Payments.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DuaTaxi.Service.Payments.dll"]