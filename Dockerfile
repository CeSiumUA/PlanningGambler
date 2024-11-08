#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["PlanningGambler/Server/PlanningGambler.Server.csproj", "PlanningGambler/Server/"]
COPY ["PlanningGambler/Client/PlanningGambler.Client.csproj", "PlanningGambler/Client/"]
COPY ["PlanningGambler/Shared/PlanningGambler.Shared.csproj", "PlanningGambler/Shared/"]
RUN dotnet restore "PlanningGambler/Server/PlanningGambler.Server.csproj"
COPY . .
WORKDIR "/src/PlanningGambler/Server"
RUN dotnet build "PlanningGambler.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PlanningGambler.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PlanningGambler.Server.dll"]