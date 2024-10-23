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

# Etapa de migrações
FROM publish AS migration
WORKDIR /src/UniConnect.API
COPY --from=publish /app/publish .
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"
RUN dotnet ef database update --project ../UniConnect.Infrastructure --startup-project .
# Copia o arquivo .db gerado para a próxima etapa
RUN cp *.db /app/publish/

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
# Copia o banco de dados atualizado para a imagem final
COPY --from=migration /app/publish/*.db .

# Define as variáveis de ambiente, se necessário
ENV ASPNETCORE_ENVIRONMENT=Production

# Define o comando para iniciar a aplicação
ENTRYPOINT ["dotnet", "UniConnect.API.dll"]
