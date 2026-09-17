# 👨‍💼 Web API de Funcionários

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-Web_API-512BD4?logo=dotnet)
![Entity Framework Core](https://img.shields.io/badge/Entity_Framework_Core-10.0-512BD4?logo=dotnet)
![SQL Server](https://img.shields.io/badge/SQL_Server-2022-CC2927?logo=microsoftsqlserver)
![Docker](https://img.shields.io/badge/Docker-Containerized-2496ED?logo=docker)
![Tests](https://img.shields.io/badge/Tests-11_passing-success)
![CI](https://github.com/andersondom/WebApi_RepositoryPattern/actions/workflows/ci.yml/badge.svg)

API REST para gerenciamento de funcionários desenvolvida com **ASP.NET Core**, **Entity Framework Core** e **SQL Server**.

O projeto evoluiu de uma implementação acadêmica para uma API modernizada, com separação entre contratos HTTP e entidades de persistência, operações assíncronas, validação, tratamento global de erros, testes automatizados, containerização e integração contínua.

---

## 🎯 Objetivo

Demonstrar a construção e evolução de uma Web API em .NET aplicando práticas utilizadas em aplicações modernas:

- API REST com ASP.NET Core
- persistência com Entity Framework Core
- SQL Server
- DTOs para entrada e saída
- validação de requisições
- operações assíncronas
- `CancellationToken`
- tratamento global de exceções
- respostas HTTP adequadas
- Health Check
- testes de serviço
- testes de integração
- Docker
- Docker Compose
- GitHub Codespaces
- GitHub Actions

---

## 🧰 Tecnologias

| Tecnologia | Utilização |
|---|---|
| .NET 10 | Plataforma principal |
| ASP.NET Core | Construção da Web API |
| C# | Linguagem |
| Entity Framework Core 10 | Persistência e acesso a dados |
| SQL Server 2022 | Banco de dados relacional |
| Swagger / OpenAPI | Documentação e exploração da API |
| xUnit | Testes automatizados |
| EF Core InMemory | Banco utilizado nos testes |
| Docker | Containerização da API |
| Docker Compose | Orquestração da API e SQL Server |
| GitHub Codespaces | Ambiente de desenvolvimento |
| GitHub Actions | Integração contínua |

---

## 🏗️ Arquitetura

A aplicação está organizada em responsabilidades distintas:

```text
WebApi_RepositoryPattern
│
├── .devcontainer/
│   └── devcontainer.json
│
├── .github/
│   └── workflows/
│       └── ci.yml
│
├── .config/
│   └── dotnet-tools.json
│
├── WebApi_Funcionarios/
│   ├── Controllers/
│   ├── DataContext/
│   ├── DTOs/
│   ├── Enums/
│   ├── Migrations/
│   ├── Models/
│   ├── Service/
│   └── Program.cs
│
├── WebApi_ASPNETCore.Tests/
│   ├── Integration/
│   └── Services/
│
├── Dockerfile
├── compose.yaml
├── .dockerignore
├── .env.example
└── README.md
```

### Fluxo principal

```text
HTTP Request
     │
     ▼
Controller
     │
     ▼
DTO / Validation
     │
     ▼
Service
     │
     ▼
Entity Framework Core
     │
     ▼
SQL Server
```

Os **DTOs** evitam que as entidades de persistência sejam utilizadas diretamente como contratos HTTP.

---

## 📡 Endpoints

Rota base:

```text
/api/funcionarios
```

| Método | Endpoint | Descrição |
|---|---|---|
| GET | `/api/funcionarios` | Lista os funcionários |
| GET | `/api/funcionarios/{id}` | Consulta um funcionário |
| POST | `/api/funcionarios` | Cadastra um funcionário |
| PUT | `/api/funcionarios/{id}` | Atualiza um funcionário |
| PATCH | `/api/funcionarios/{id}/deactivate` | Desativa um funcionário |
| DELETE | `/api/funcionarios/{id}` | Exclui um funcionário |
| GET | `/health` | Verifica a disponibilidade da aplicação |

O cadastro retorna **HTTP 201 Created**, incluindo referência ao recurso criado.

Recursos inexistentes consultados pelas operações correspondentes retornam **HTTP 404 Not Found**.

Requisições inválidas retornam **HTTP 400 Bad Request** com detalhes de validação.

---

## 📦 Exemplo de requisição

### Criar funcionário

```http
POST /api/funcionarios
Content-Type: application/json
```

```json
{
  "nome": "João",
  "sobrenome": "Silva",
  "departamento": 1,
  "turno": 1,
  "ativo": true
}
```

---

## ⚠️ Tratamento de erros

A aplicação possui tratamento global para exceções não tratadas.

Em caso de erro interno, a API utiliza respostas no formato **Problem Details** (`application/problem+json`).

Exemplo:

```json
{
  "title": "Erro interno do servidor",
  "status": 500,
  "detail": "Ocorreu um erro inesperado ao processar a requisição.",
  "instance": "/api/funcionarios",
  "traceId": "..."
}
```

O `traceId` auxilia na correlação entre a resposta enviada ao cliente e os logs da aplicação.

---

## ❤️ Health Check

A aplicação disponibiliza:

```http
GET /health
```

Uma aplicação saudável responde com:

```text
HTTP 200 OK
```

Esse endpoint pode ser utilizado por plataformas de hospedagem, containers, sistemas de monitoramento e orquestradores.

---

## 🧪 Testes automatizados

O projeto possui testes de serviço e testes de integração utilizando **xUnit**.

Atualmente a suíte contém:

```text
11 testes
11 aprovados
0 falhas
```

Entre os cenários verificados estão:

- listagem de funcionários
- consulta por ID
- funcionário inexistente
- criação
- atualização
- desativação
- exclusão
- validação de requisição
- códigos HTTP
- Health Check

Para executar:

```bash
dotnet test WebApi_ASPNETCore.Tests/WebApi_ASPNETCore.Tests.csproj -c Release
```

---

## 🔄 CancellationToken

As operações assíncronas propagam `CancellationToken` da camada HTTP até as operações de acesso aos dados.

Isso permite cancelar processamento quando uma requisição é interrompida pelo cliente ou pelo servidor.

---

## 🗄️ Banco de dados

A aplicação utiliza **SQL Server** com **Entity Framework Core**.

A configuração é obtida através de:

```text
ConnectionStrings:ConexaoPadrao
```

Em variáveis de ambiente do ASP.NET Core, a mesma configuração pode ser informada como:

```text
ConnectionStrings__ConexaoPadrao
```

O repositório contém `.env.example` apenas como referência de configuração.

**Credenciais reais não devem ser versionadas.**

---

## 🔄 Migrations

O projeto utiliza migrations do Entity Framework Core.

Restaure a ferramenta local:

```bash
dotnet tool restore
```

Para aplicar as migrations:

```bash
dotnet ef database update \
  --project WebApi_Funcionarios/WebApi_ASPNETCore.csproj
```

---

## 🐳 Docker

A API possui `Dockerfile` multi-stage.

Para criar a imagem:

```bash
docker build -t webapi-funcionarios .
```

O container da API utiliza a porta:

```text
8080
```

---

## 🐳 Docker Compose

O arquivo `compose.yaml` define:

```text
API
 │
 └── SQL Server 2022
```

Antes de iniciar o ambiente, configure localmente a senha do SQL Server:

```bash
export SQLSERVER_SA_PASSWORD='SUA_SENHA_FORTE'
```

Depois:

```bash
docker compose up -d
```

Para acompanhar os containers:

```bash
docker compose ps
```

Para encerrar:

```bash
docker compose down
```

> Dependendo do ambiente remoto utilizado para executar Docker, características específicas de rede podem afetar a comunicação entre containers. A configuração do Compose mantém o padrão portátil de comunicação pelo nome do serviço.

---

## ☁️ GitHub Codespaces

O repositório possui configuração de **Dev Container**.

Ao criar ou reconstruir um Codespace, o ambiente disponibiliza as ferramentas necessárias ao desenvolvimento do projeto e executa a restauração das dependências.

A configuração também encaminha as portas:

```text
8080 - Web API
1433 - SQL Server
```

O segredo utilizado pelo ambiente deve ser configurado nos **Codespaces secrets** e nunca armazenado no código-fonte.

---

## ⚙️ Integração contínua

O projeto utiliza **GitHub Actions**.

O workflow executa automaticamente em alterações configuradas para as branches principais de desenvolvimento.

Pipeline:

```text
Checkout
   │
   ▼
Setup .NET 10
   │
   ▼
Restore
   │
   ▼
Build
   │
   ▼
Tests
```

Assim, alterações enviadas ao GitHub são verificadas automaticamente quanto à compilação e aos testes.

---

## ▶️ Executando sem Docker

### 1. Restaurar dependências

```bash
dotnet restore WebApi_Funcionarios/WebApi_ASPNETCore.csproj
dotnet restore WebApi_ASPNETCore.Tests/WebApi_ASPNETCore.Tests.csproj
```

### 2. Configurar a conexão

Configure:

```text
ConnectionStrings__ConexaoPadrao
```

com uma conexão válida para SQL Server.

### 3. Restaurar ferramentas

```bash
dotnet tool restore
```

### 4. Aplicar migrations

```bash
dotnet ef database update \
  --project WebApi_Funcionarios/WebApi_ASPNETCore.csproj
```

### 5. Executar a API

```bash
dotnet run \
  --project WebApi_Funcionarios/WebApi_ASPNETCore.csproj
```

---

## 🔐 Segurança de configuração

O projeto segue algumas práticas para evitar exposição acidental de credenciais:

- connection string real fora do repositório
- `.env` ignorado pelo Git
- `.env.example` apenas como modelo
- secrets do Codespaces para valores sensíveis
- ausência de senhas reais nos arquivos versionados

---

## 🚀 Evolução técnica

A modernização deste projeto incluiu:

```text
.NET 7
  ↓
.NET 10

Código síncrono
  ↓
Entity Framework Core assíncrono

Entidades expostas pela API
  ↓
DTOs de entrada e saída

Tratamento repetitivo de exceções
  ↓
Tratamento global

Rotas implícitas
  ↓
Rotas REST explícitas

Sem testes
  ↓
Testes de serviço + integração

Ambiente manual
  ↓
Docker + Codespaces

Validação somente local
  ↓
CI com GitHub Actions
```

O objetivo da evolução foi melhorar **manutenibilidade, testabilidade, confiabilidade e experiência de desenvolvimento**, evitando complexidade arquitetural sem necessidade.

---

## 👨‍💻 Autor

**Anderson Domingos**

Desenvolvedor .NET, profissional de tecnologia e instrutor de educação profissional na área de desenvolvimento de sistemas.

Este repositório também representa um processo contínuo de estudo, modernização e aplicação prática de boas práticas no ecossistema .NET.

---

## 📄 Licença

Este projeto está disponível para fins de estudo e demonstração técnica.
