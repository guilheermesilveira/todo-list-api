# To-do List API

### Objetivo

- Desenvolver uma API Rest usando .NET 6 para a criação de um sistema de gerenciamento de tarefas.

### Características

- Estrutura em quatro camadas (API, Application, Domain, Infra.Data): garante uma separação clara das responsabilidades e facilita a manutenção do código.
  
- Autenticação de usuários: o sistema possui registro e login de usuário.
  
- Acesso individualizado: cada usuário tem acesso exclusivo às suas próprias tarefas.

### Camadas

- API: lida com as solicitações HTTP, roteia essas solicitações para os serviços apropriados e fornece as respostas adequadas.
  
- Application: responsável por conter a lógica de negócios da aplicação.
  
- Domain: representa o modelo de domínio, as entidades. É independente de todas as outras camadas.
  
- Infra.Data: responsável por armazenar e acessar os dados no banco.

### Tecnologias utilizadas

- .NET 6: versão do .NET para desenvolver a aplicação.
  
- Entity Framework Core: essa tecnologia é a base de acesso a banco de dados, permitindo interação com o MySQL de forma eficiente.
  
- MySQL: sistema de gerenciamento de banco de dados escolhido para o projeto.
  
- AutoMapper: essa biblioteca ajuda a mapear objetos de diferentes estruturas, facilitando a transferência de dados entre camadas.
  
- FluentValidation: usado para validar os dados de entrada e garantir que apenas informações válidas sejam processadas pela aplicação.
  
- ScottBrady91.AspNetCore.Identity.Argon2PasswordHasher: essa biblioteca aprimora a segurança da autenticação de usuários por meio do uso do algoritmo Argon2 para o hash de senhas.
