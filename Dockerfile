# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia os arquivos do projeto e restaura as dependências
COPY ["UniConnect.API/UniConnect.API.csproj", "UniConnect.API/"]
COPY ["UniConnect.Application/UniConnect.Application.csproj", "UniConnect.Application/"]
COPY ["UniConnect.Domain/UniConnect.Domain.csproj", "UniConnect.Domain/"]
COPY ["UniConnect.Infrastructure/UniConnect.Infrastructure.csproj", "UniConnect.Infrastructure/"]
RUN dotnet restore "UniConnect.API/UniConnect.API.csproj"

# Copia o restante dos arquivos e realiza o build
COPY . .
WORKDIR "/src/UniConnect.API"
RUN dotnet build "UniConnect.API.csproj" -c Release -o /app/build

# Etapa de publicação
FROM build AS publish
RUN dotnet publish "UniConnect.API.csproj" -c Release -o /app/publish

# Etapa final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Executa a migration ao iniciar o contêiner
ENTRYPOINT ["dotnet", "UniConnect.API.dll"]
