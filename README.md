# Todo List API ✅

## 🎯 Objetivo
Esse projeto é uma API Rest para um sistema de gerenciamento de tarefas, fornecendo aos usuários um meio para organizar suas atividades. O sistema foi projetado apenas com acesso de usuário.

## 🌐 Funcionalidades
- Registro e login de usuário.
- Cada usuário tem acesso exclusivo às suas próprias listas de tarefas.
- Adição de novas tarefas a uma lista.
- Marcação de tarefas como concluídas.
- Visualização completa de todas as tarefas em uma lista.

## 🏛️ Arquitetura
O projeto está dividido nas seguintes camadas: API, Application, Domain, Infra.Data.

## 💻 Tecnologias e dependências utilizadas
- C# e .NET 6
- Entity Framework Core
- MySQL
- AutoMapper
- FluentValidation
- ScottBrady91.AspNetCore.Identity.Argon2PasswordHasher

## ▶️ Como rodar o projeto
1. Clone este repositório.
2. Abra o projeto em sua IDE favorita.
3. Navegue até o arquivo [appsettings.Example.json](src/TodoList.API/appsettings.Example.json).
4. Configure a conexão com o banco de dados MySQL na seção ``ConnectionStrings``.
5. Informe a chave secreta na seção ``JwtSettings``.
6. Após terminar a configuração do ``appsettings.Example.json``, lembre-se de modificar a extensão "Example" para o nome do ambiente desejado (por exemplo, appsettings.Development.json).
7. Faça o restore dos pacotes NuGet. Use o comando: ``dotnet restore``.
8. Utilize um sistema gerenciador de banco de dados como o MySQL Workbench.
9. Certifique-se de que o Entity Framework Core Tools está instalado. Caso não esteja, instale com o comando: ``dotnet tool install --global dotnet-ef``.
10. Aplique as migrações do Entity Framework Core para atualizar o banco de dados. Utilize o comando: ``dotnet ef database update``.
11. Abra o terminal e navegue até a pasta TodoList.API.
12. Execute o comando ``dotnet run`` para iniciar a aplicação.
13. Acesse a API documentada pelo Swagger.
