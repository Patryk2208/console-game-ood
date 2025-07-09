FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["GameServer/GameServer.csproj", "GameServer/"]
COPY ["Model/Model.csproj", "Model/"]
RUN dotnet restore "Model/Model.csproj"
RUN dotnet restore "GameServer/GameServer.csproj"
COPY ./GameServer GameServer/
COPY ./Model Model/
WORKDIR "/src/GameServer"
RUN dotnet build "./GameServer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./GameServer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=publish /app/publish .

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1 \
    DOTNET_CLI_TELEMETRY_OPTOUT=1 \
    DOTNET_NOLOGO=true \
    PORT=7777 \
    AGONES_SDK_GRPC_PORT=9357

EXPOSE 7000-8000 9357

RUN groupadd -r gs && useradd -r -g gs gs
USER gs
ENTRYPOINT ["dotnet", "GameServer.dll"]
