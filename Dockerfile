# Usar a imagem base do .NET 8 SDK para a fase de construção
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copiar os arquivos csproj e restaurar dependências
COPY /src/ConsumidorPedidos/ConsumidorPedidos.csproj ./src/ConsumidorPedidos/
COPY /src/ConsumidorPedidos.Core/ConsumidorPedidos.Core.csproj ./src/ConsumidorPedidos.Core/
COPY /src/ConsumidorPedidos.Data.MySql/ConsumidorPedidos.Data.MySql.csproj ./src/ConsumidorPedidos.Data.MySql/
COPY /src/ConsumidorPedidos.Data.Messaging/ConsumidorPedidos.Data.Messaging.csproj ./src/ConsumidorPedidos.Data.Messaging/
COPY /src/ConsumidorPedidos.Tests/ConsumidorPedidos.Tests.csproj ./src/ConsumidorPedidos.Tests/

WORKDIR /app/src/ConsumidorPedidos
RUN dotnet restore

# Copiar o restante dos arquivos e compilar a aplicação
WORKDIR /app
COPY . .
RUN dotnet publish -c Release -o out

# Construir a imagem de produção
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Definir o ponto de entrada da aplicação
ENTRYPOINT ["dotnet", "ConsumidorPedidos.dll"]
