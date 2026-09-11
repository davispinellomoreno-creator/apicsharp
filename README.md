💰 Controle Financeiro API

API REST desenvolvida em C# / ASP.NET Core para gerenciamento de finanças pessoais — cadastro de receitas e despesas, categorização e relatórios de saldo.

Projeto criado como estudo prático de C# e .NET, vindo de experiência prévia com Java e Spring Boot.

🚀 Tecnologias utilizadas
.NET 8+ (C#)
ASP.NET Core Web API
Entity Framework Core — ORM (equivalente ao Hibernate/JPA)
Npgsql — provider do PostgreSQL para EF Core
PostgreSQL — banco de dados relacional
JWT (JSON Web Token) — autenticação
Swagger / OpenAPI — documentação interativa da API
📁 Estrutura do projeto
ControleFinanceiro.Api/
│
├── Controllers/        # Endpoints da API (equivalente a @RestController)
├── Models/              # Entidades do banco de dados (equivalente a @Entity)
├── DTOs/                 # Objetos de entrada/saída da API
├── Data/                  # Configuração do DbContext (acesso ao banco)
├── Services/              # Regras de negócio (equivalente a @Service)
├── Migrations/            # Histórico de alterações do banco (gerado pelo EF Core)
├── Program.cs             # Ponto de entrada e configuração da aplicação
└── appsettings.json       # Configurações (connection string, etc)
⚙️ Pré-requisitos
.NET SDK 8.0+
PostgreSQL instalado localmente ou via Docker
dotnet-ef CLI tool:
bash
  dotnet tool install --global dotnet-ef
🐘 Subindo o PostgreSQL com Docker (opcional)
bash
docker run --name postgres-financeiro \
  -e POSTGRES_PASSWORD=sua_senha \
  -e POSTGRES_DB=controle_financeiro \
  -p 5432:5432 \
  -d postgres
🔧 Configuração
Clone o repositório:
bash
   git clone <url-do-repositorio>
   cd ControleFinanceiro.Api
Configure a connection string no appsettings.json:
json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=5432;Database=controle_financeiro;Username=postgres;Password=sua_senha"
     }
   }
Restaure as dependências:
bash
   dotnet restore
Aplique as migrations para criar as tabelas no banco:
bash
   dotnet ef database update
Rode a aplicação:
bash
   dotnet run
Acesse a documentação interativa:
   http://localhost:5104/swagger

(a porta pode variar — confira no terminal ao rodar dotnet run)

📌 Endpoints principais
Autenticação
Método	Rota	Descrição
POST	/api/auth/registrar	Cria um novo usuário
POST	/api/auth/login	Autentica e retorna um token JWT
Categorias
Método	Rota	Descrição
GET	/api/categorias	Lista todas as categorias
POST	/api/categorias	Cria uma nova categoria (Receita ou Despesa)
Transações
Método	Rota	Descrição
GET	/api/transacoes	Lista transações (com filtros por período/categoria/tipo)
POST	/api/transacoes	Cria uma nova transação
PUT	/api/transacoes/{id}	Atualiza uma transação existente
DELETE	/api/transacoes/{id}	Remove uma transação
Relatórios
Método	Rota	Descrição
GET	/api/relatorios/saldo	Retorna o saldo total (receitas − despesas)
GET	/api/relatorios/por-categoria	Totais agrupados por categoria
GET	/api/relatorios/mensal	Evolução de receitas/despesas por mês

⚠️ A maioria dos endpoints (exceto /auth) requer um token JWT válido no header Authorization: Bearer {token}.

🗄️ Modelo de dados
Usuario (Id, Nome, Email, SenhaHash)
Categoria (Id, Nome, Tipo)              // Tipo: Receita | Despesa
Transacao (Id, Descricao, Valor, Data, CategoriaId, UsuarioId)

Relacionamentos:

Um Usuario possui várias Transacoes (1:N)
Uma Categoria possui várias Transacoes (1:N)
🧪 Rodando testes
bash
dotnet test
📚 Status do projeto

🚧 Em desenvolvimento — projeto de estudo pessoal para aprendizado de C# e .NET.

🤝 Como contribuir

Este projeto é aberto! Qualquer pessoa pode contribuir, não é necessário pedir permissão antes.

Clone o repositório:
bash
   git clone <url-do-repositorio>
Crie uma branch para a sua alteração:
bash
   git checkout -b minha-feature
Faça suas alterações e commits:
bash
   git add .
   git commit -m "Descrição breve da alteração"
Envie sua branch:
bash
   git push origin minha-feature
Abra um Pull Request explicando o que foi feito.

Sugestões, correções e novas features são bem-vindas. Só evite dar push direto na branch main.

📝 Licença

Este projeto é livre para fins de estudo e aprendizado.
