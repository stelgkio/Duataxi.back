#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
#  cd ..     docker build -f .\DuaTaxi.CoreApi\Dockerfile -t  duataxi.coreapi:v1 .
#            docker run -p 5000:5000 --network=dshop-network  duataxi.coreapi:v1      
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
ENV ASPNETCORE_URLS http://*:5555
EXPOSE 5555


FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["./DuaTaxi.CoreApi/DuaTaxi.CoreApi.csproj", "DuaTaxi.CoreApi/"]
COPY ["./DuaTaxi.Common/DuaTaxi.Common.csproj", "DuaTaxi.Common/"]
RUN dotnet restore "./DuaTaxi.CoreApi/DuaTaxi.CoreApi.csproj"
COPY . .
WORKDIR "/src/DuaTaxi.CoreApi"
RUN dotnet build "DuaTaxi.CoreApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DuaTaxi.CoreApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DuaTaxi.CoreApi.dll"]
#

#FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env
#WORKDIR /app
#EXPOSE 5000
#
## Copy csproj and restore as distinct layers
#COPY ./DuaTaxi.CoreApi/*.csproj ./
#COPY ./DuaTaxi.Common/*.csproj ./
#RUN dotnet restore DuaTaxi.CoreApi.csproj
#
## Copy everything else and build
#COPY . ./
#RUN dotnet publish DuaTaxi.CoreApi.csproj -c Release -o out
#
## Build runtime image
#FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
#WORKDIR /app
#COPY --from=build-env /app/out .
#ENTRYPOINT ["dotnet", "DuaTaxi.CoreApi.dll"]