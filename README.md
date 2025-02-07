# Gerenciamento de Usuários - API

### Descrição

Este projeto é uma API para gerenciamento de usuários, implementada utilizando ASP.NET Core e seguindo boas práticas de desenvolvimento, como Dependency Injection, Middleware, Filters e CORS.

A API conta com funcionalidades de autenticação e autorização via JWT, caching, rate limiting, documentação OpenAPI e versionamento.

### Tecnologias Utilizadas

- ASP.NET Core

- Entity Framework Core

- JWT (JSON Web Token) para autenticação

- Swagger/OpenAPI para documentação

- Caching e Rate Limiting para otimização

- Dependency Injection para gestão de dependências

- CORS para controle de acesso

## Funcionalidades

### Autenticação e Autorizacão:

- Login com JWT

- Proteção de rotas com base em permissões

- Gerenciamento de Usuários:

        Criar novo usuário (POST /usuarios)

        Listar usuários (GET /usuarios)

        Atualizar usuário (PATCH /usuarios/{id})

        Excluir usuário (DELETE /usuarios/{id})

### Documentação OpenAPI:

Disponível em `/swagger` para explorar e testar os endpoints

- Controle de Versão:

        Suporte a v1 e v2 via versionamento de API.
