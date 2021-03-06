#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["PoS.Sell.API/PoS.Sell.API.csproj", "PoS.Sell.API/"]
COPY ["PoS.Sell.Domain/PoS.Sell.Domain.csproj", "PoS.Sell.Domain/"]
COPY ["PoS.Sell.Infrastructure/PoS.Sell.Infrastructure.csproj", "PoS.Sell.Infrastructure/"]
COPY ["PoS.CC.Utilities/PoS.CC.Utilities.csproj", "PoS.CC.Utilities/"]
COPY ["PoS.CC.EventBus/PoS.CC.EventBus.csproj", "PoS.CC.EventBus/"]
RUN dotnet restore "PoS.Sell.API/PoS.Sell.API.csproj"
COPY . .
WORKDIR "/src/PoS.Sell.API"
RUN dotnet build "PoS.Sell.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PoS.Sell.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PoS.Sell.API.dll"]