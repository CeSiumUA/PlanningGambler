﻿FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["PlanningGambler/PlanningGambler.csproj", "PlanningGambler/"]
RUN dotnet restore "PlanningGambler/PlanningGambler.csproj"
COPY . .
WORKDIR "/src/PlanningGambler"
RUN dotnet build "PlanningGambler.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PlanningGambler.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet PlanningGambler.dll
