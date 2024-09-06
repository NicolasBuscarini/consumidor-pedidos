# 🛒 **ConsumidorPedidos**

**ConsumidorPedidos** é uma aplicação .NET que processa pedidos, consumindo mensagens de uma fila RabbitMQ, armazenando os dados em um banco de dados MySQL e expondo uma API REST para consulta de informações sobre pedidos.

## ⚙️ **Tecnologias Utilizadas**

- **.NET 8**: Plataforma de desenvolvimento utilizada para criar a API e o microserviço.
- **MySQL**: Banco de dados relacional utilizado para armazenar os pedidos processados.
- **RabbitMQ**: Message broker utilizado para gerenciar a fila de pedidos.
- **Docker**: Ferramenta de containerização utilizada para orquestrar os serviços necessários (RabbitMQ, MySQL, API).

## ✨ **Funcionalidades**

- **Consumo de Pedidos**: O microserviço consome mensagens de uma fila RabbitMQ contendo informações de pedidos.
- **Armazenamento em Banco de Dados**: Os dados dos pedidos são armazenados em um banco de dados MySQL.
- **API REST**: A aplicação expõe uma API para consultar:
  - Valor total de um pedido
  - Quantidade de pedidos por cliente
  - Lista de pedidos realizados por cliente

## 🏛️ **Arquitetura**

A arquitetura da aplicação segue o padrão de microserviços e utiliza RabbitMQ para comunicação assíncrona entre os componentes. O diagrama a seguir ilustra a estrutura da aplicação:

![Diagrama de Arquitetura](./docs/arq/isoflow.png)
- [📂 Mais informações](./docs/arq/arq.md)

## 📋 **Requisitos**

- **.NET 8 SDK**: Para compilar, testar e rodar o projeto.
- **Docker**: Para rodar os containers de RabbitMQ, MySQL e a aplicação .NET.

## 🚀 **Como Executar**

1. **Clone o Repositório**:
   ```bash
   git clone https://github.com/seu-usuario/ConsumidorPedidos.git
   cd ConsumidorPedidos
   ```

2. **Configuração do Ambiente**:
   - Certifique-se de ter o Docker instalado e funcionando.

3. **Subir os Serviços com Docker Compose**:
   - No diretório do projeto, execute o comando:
   ```bash
   docker-compose up -d
   ```

4. **Acessar a API**:
   - Após os serviços estarem em execução, a API REST estará disponível em `http://localhost:5005`.

## 🌐 **Portas dos Serviços**

- **RabbitMQ**: 
  - Porta 5672 (AMQP)
  - Porta 15672 (Management UI)
- **MySQL**:
  - Porta 3307 (Conexão com o banco de dados)

## 🧪 **Testes**

Para rodar os testes funcionais da aplicação:

```bash
dotnet test
```

---

## 🛠️ **Swagger e Ferramentas de Teste**

Recomendamos o uso do **Swagger** para explorar e testar a API REST. Ele facilita a navegação pelos endpoints, além de permitir a execução de requisições diretamente pelo navegador. Ao rodar o projeto, o Swagger estará disponível em:

- `http://localhost:5005/swagger`

Embora o **Swagger** seja a ferramenta preferida para interagir com a API, também é possível utilizar outras opções como o **Postman** ou o **Insomnia** para testar os endpoints e fazer chamadas REST, conforme sua preferência.

---

## 📥 **Formato de Mensagem JSON na Fila**

O microserviço consome mensagens JSON da fila RabbitMQ para processar os pedidos. Abaixo está um exemplo do formato de mensagem esperado:

```json
{
  //"codigoPedido": 1001,
  "codigoCliente": 1,
  "itens": [
    {
      "produto": "lápis",
      "quantidade": 100,
      "preco": 1.1
    },
    {
      "produto": "caderno",
      "quantidade": 10,
      "preco": 1
    }
  ]
}
```

### ⚠️ **Recomendação Importante**

- **Não recomendamos enviar o campo `codigoPedido`** (que é do tipo `int`) na mensagem, pois ele é um campo **auto-incremental**. Inserir um valor manualmente nesse campo pode causar erros, especialmente se você tentar gravar uma entidade com um ID já existente no banco de dados.
  
- Caso você tenha controle sobre o `codigoPedido` e garanta a unicidade do valor, pode incluí-lo. Contudo, na maioria dos casos, o ideal é deixar o banco de dados gerenciar esse campo automaticamente.

---

## 📚 **Documentação**

- [📄 API](./docs/API.md)
- [🐳 Erros Comuns no Docker Compose](./docs/erros-comuns/docker-compose.md)
- [📊 Arquitetura](./docs/arq/arq.md)

---
