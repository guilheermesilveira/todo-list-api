# To-do List API ✅

## 🎯 Objetivo
Este projeto consiste em uma API RESTful para um sistema de gerenciamento de tarefas, fornecendo aos usuários um meio para organizar suas atividades.

## 🏛️ Arquitetura
O projeto segue uma arquitetura em camadas, organizada da seguinte forma:
- API: Responsável por receber e responder as requisições HTTP. É a porta de entrada do sistema.
- Application: Contém as regras de negócio e validações. Atua como intermediária entre a API e o domínio.
- Domain: Define as entidades, interfaces e a lógica essencial da aplicação. Representa o coração do sistema.
- Infra.Data: Responsável pela persistência de dados. Aqui ficam as implementações para acesso ao banco de dados.

## 💻 Algumas tecnologias e dependências utilizadas
- C# e .NET 6
- Entity Framework Core
- MySQL
- AutoMapper
- FluentValidation
- ScottBrady91.AspNetCore.Identity.Argon2PasswordHasher
