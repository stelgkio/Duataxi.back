FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 5012
EXPOSE 44312

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["DuaTaxi.SMTP/DuaTaxi.SMTP.csproj", "DuaTaxi.SMTP/"]
COPY ["DuaTaxi.Common/DuaTaxi.Common.csproj", "DuaTaxi.Common/"]
RUN dotnet restore "DuaTaxi.SMTP/DuaTaxi.SMTP.csproj"
COPY . .
WORKDIR "/src/DuaTaxi.SMTP"
RUN dotnet build "DuaTaxi.SMTP.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DuaTaxi.SMTP.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DuaTaxi.SMTP.dll"]