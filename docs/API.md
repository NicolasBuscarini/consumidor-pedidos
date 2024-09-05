# ConsumidorPedidosAPI - v1

## Visão Geral

A `ConsumidorPedidosAPI` fornece endpoints para gerenciar pedidos no sistema. Esta API permite consultar pedidos filtrados por cliente, listar todos os pedidos com paginação, recuperar pedidos específicos por ID e postar novos pedidos em uma fila para processamento assíncrono. 

## Endpoints

### 1. Listar Pedidos por Cliente

- **URL:** `/api/Order/by-client`
- **Método:** `GET`
- **Descrição:** Recupera uma lista de pedidos filtrados pelo código do cliente especificado. Esse endpoint é útil para obter todos os pedidos relacionados a um cliente específico.

#### Parâmetros de Consulta

| Nome        | Tipo     | Formato | Padrão | Descrição                |
|-------------|----------|---------|--------|--------------------------|
| `clientCode` | inteiro  | int32   | -      | O código do cliente para filtrar os pedidos. Este parâmetro é obrigatório para especificar de qual cliente os pedidos devem ser recuperados. |
| `pageNumber` | inteiro  | int32   | 1      | O número da página para paginação. Determina qual página de resultados deve ser retornada. |
| `pageSize`   | inteiro  | int32   | 10     | O número de itens a serem retornados por página. Controla o tamanho de cada página de resultados. |

#### Respostas

- **200 OK**
  - **Content-Type:** `application/json`
  - **Schema:** `OrderListBaseResponse`
  - **Descrição:** Retorna uma resposta bem-sucedida com a lista de pedidos para o cliente especificado, incluindo metadados de paginação e possíveis links HATEOAS.

- **400 Bad Request**
  - **Content-Type:** `application/json`
  - **Schema:** `OrderListBaseResponse`
  - **Descrição:** Indica que a solicitação foi inválida, como parâmetros ausentes ou valores inválidos.

### 2. Listar Todos os Pedidos

- **URL:** `/api/Order`
- **Método:** `GET`
- **Descrição:** Recupera uma lista de todos os pedidos com paginação. Este endpoint é usado para obter uma lista paginada de todos os pedidos, sem filtro por cliente.

#### Parâmetros de Consulta

| Nome        | Tipo     | Formato | Padrão | Descrição              |
|-------------|----------|---------|--------|------------------------|
| `pageNumber` | inteiro  | int32   | 1      | O número da página para paginação. Determina qual página de resultados deve ser retornada. |
| `pageSize`   | inteiro  | int32   | 10     | O número de itens a serem retornados por página. Controla o tamanho de cada página de resultados. |

#### Respostas

- **200 OK**
  - **Content-Type:** `application/json`
  - **Schema:** `OrderListBaseResponse`
  - **Descrição:** Retorna uma resposta bem-sucedida com uma lista paginada de todos os pedidos, incluindo metadados de paginação e possíveis links HATEOAS.

- **404 Not Found**
  - **Content-Type:** `application/json`
  - **Schema:** `OrderListBaseResponse`
  - **Descrição:** Indica que nenhum pedido foi encontrado. Isso pode ocorrer se não houver pedidos no sistema.

### 3. Recuperar Pedido por ID

- **URL:** `/api/Order/{id}`
- **Método:** `GET`
- **Descrição:** Recupera um pedido específico pelo seu ID único. Este endpoint é usado para obter detalhes sobre um pedido em particular.

#### Parâmetros de Caminho

| Nome | Tipo    | Formato | Obrigatório | Descrição             |
|------|---------|---------|-------------|-----------------------|
| `id` | inteiro | int32   | Sim         | O ID único do pedido a ser recuperado. |

#### Respostas

- **200 OK**
  - **Content-Type:** `application/json`
  - **Schema:** `OrderBaseResponse`
  - **Descrição:** Retorna uma resposta bem-sucedida com os detalhes do pedido especificado, incluindo possíveis links HATEOAS e metadados.

- **404 Not Found**
  - **Content-Type:** `application/json`
  - **Schema:** `OrderBaseResponse`
  - **Descrição:** Indica que o pedido com o ID especificado não foi encontrado.

### 4. Postar Pedido na Fila

- **URL:** `/api/Order/Queue`
- **Método:** `POST`
- **Descrição:** Posta um novo pedido na fila para processamento. Este endpoint é usado para enviar um pedido para ser processado de forma assíncrona.

#### Corpo da Solicitação

- **Content-Type:** `application/json`
- **Schema:** `Order`
- **Descrição:** O corpo da solicitação deve conter os detalhes do pedido a ser postado. O esquema `Order` inclui informações como o código do pedido, código do cliente e itens.

#### Respostas

- **201 Created**
  - **Content-Type:** `application/json`
  - **Schema:** `BooleanBaseResponse`
  - **Descrição:** Indica que o pedido foi postado com sucesso na fila. A resposta incluirá um campo `data` definido como `true`.

- **400 Bad Request**
  - **Content-Type:** `application/json`
  - **Schema:** `BooleanBaseResponse`
  - **Descrição:** Indica que a solicitação foi inválida, como campos obrigatórios ausentes nos detalhes do pedido ou dados inválidos.

## Esquemas

### BooleanBaseResponse

```json
{
  "type": "object",
  "properties": {
    "data": { "type": "boolean" },
    "links": {
      "type": "array",
      "items": { "$ref": "#/components/schemas/LinkInfo" },
      "nullable": true
    },
    "meta": { "$ref": "#/components/schemas/MetaData" },
    "error": { "$ref": "#/components/schemas/ErrorResponse" }
  },
  "additionalProperties": false
}
```

### ErrorResponse

```json
{
  "type": "object",
  "properties": {
    "code": { "type": "integer", "format": "int32" },
    "message": { "type": "string", "nullable": true },
    "details": {
      "type": "array",
      "items": { "type": "string" },
      "nullable": true
    }
  },
  "additionalProperties": false
}
```

### Item

```json
{
  "type": "object",
  "properties": {
    "produto": { "type": "string", "nullable": true },
    "quantidade": { "type": "integer", "format": "int32" },
    "preco": { "type": "number", "format": "float" }
  },
  "additionalProperties": false
}
```

### LinkInfo

```json
{
  "type": "object",
  "properties": {
    "rel": { "type": "string", "nullable": true },
    "href": { "type": "string", "nullable": true },
    "method": { "type": "string", "nullable": true }
  },
  "additionalProperties": false
}
```

### MetaData

```json
{
  "type": "object",
  "properties": {
    "totalItems": { "type": "integer", "format": "int32" },
    "itemsPerPage": { "type": "integer", "format": "int32" },
    "currentPage": { "type": "integer", "format": "int32" },
    "totalPages": { "type": "integer", "format": "int32" }
  },
  "additionalProperties": false
}
```

### Order

```json
{
  "type": "object",
  "properties": {
    "codigoPedido": { "type": "integer", "format": "int32" },
    "codigoCliente": { "type": "integer", "format": "int32" },
    "itens": {
      "type": "array",
      "items": { "$ref": "#/components/schemas/Item" },
      "nullable": true
    }
  },
  "additionalProperties": false
}
```

### OrderBaseResponse

```json
{
  "type": "object",
  "properties": {
    "data": { "$ref": "#/components/schemas/Order" },
    "links": {
      "type": "array",
      "items": { "$ref": "#/components/schemas/LinkInfo" },
      "nullable": true
    },
    "meta": { "$ref": "#/components/schemas/MetaData" },
    "error": { "$ref": "#/components/schemas/ErrorResponse" }
  },
  "additionalProperties": false
}
```

### OrderListBaseResponse

```json
{
  "type": "object",
  "properties": {
    "data": {
      "type": "array",
      "items": { "$ref": "#/components/schemas/Order" },
      "nullable": true
    },
    "links": {
      "type": "array",
      "items": { "$ref": "#/components/schemas/LinkInfo" },
      "nullable": true
    },
    "meta": { "$ref": "#/components/schemas/MetaData"
```
