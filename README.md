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

---

## 🔍 Revisão de código (14/10/2026)

Lista dos problemas encontrados na revisão da API e o que foi feito em cada um. Use como checklist para revisar.

### Erros que quebrariam a API com o banco ligado

- [x] **1. Pacotes de dois bancos diferentes** — o `meuprojeto.csproj` tinha PostgreSQL (Npgsql) e MySQL (Pomelo 9.0.0). O Pomelo 9 é incompatível com o EF Core 10 (gerava o aviso `NU1608` no build).
  **Correção:** removido o pacote `Pomelo.EntityFrameworkCore.MySql`. O banco é só PostgreSQL.

- [x] **2. Não existia forma de criar usuário** — o `UsuariosController` não tinha `POST`, e a senha nunca era salva (o campo `Senha` do DTO era ignorado). Como as transações usam `usuarioId = 1` fixo, criar transação daria erro de chave estrangeira.
  **Correção:** criado `POST /api/usuarios`, com a senha salva como hash (`PasswordHasher<Usuario>` do ASP.NET Core Identity). O `PUT` também passou a atualizar a senha (com hash). Um e-mail duplicado retorna `409 Conflict`.

- [x] **3. Apagar categoria apagaria todas as transações dela** — por padrão, o EF configura exclusão em cascata em chaves estrangeiras obrigatórias.
  **Correção:** `OnDelete(DeleteBehavior.Restrict)` no `AppDbContext`. O `DELETE /api/categorias/{id}` retorna `409 Conflict` se a categoria tiver transações.

- [x] **4. `CategoriaId` inexistente gerava erro 500** — a categoria não era validada antes de salvar a transação.
  **Correção:** o `TransacaoService` verifica se a categoria existe (`CategoriaExisteAsync` no repository) e lança `CategoriaNaoEncontradaException`. O controller converte isso em `400 Bad Request`.

### Bugs de lógica

- [x] **5. Criar transação retornava a categoria vazia** — a resposta do `POST /api/transacoes` vinha com `NomeCategoria` e `TipoCategoria` como `""`.
  **Correção:** depois de salvar, a transação é buscada de novo com `Include(Categoria)`.

- [x] **6. Faltava checar o dono da transação** — só a listagem filtrava por usuário. `GET`, `PUT` e `DELETE` por Id acessavam transações de qualquer usuário.
  **Correção:** `usuarioId` passado em todos os métodos do service/repository. Transação de outro usuário retorna `404`.

### Melhorias

- [x] **7. Nomes dos arquivos dos controllers** — `Controllers/Transacao.cs` continha `TransacoesController` (fácil de confundir com `Models/Transacao.cs`).
  **Correção:** renomeados para `TransacoesController.cs`, `CategoriasController.cs` e `UsuariosController.cs`.

- [x] **8. DTOs sem validação** — era possível criar uma transação com descrição vazia ou valor negativo.
  **Correção:** atributos `[Required]`, `[StringLength]`, `[Range]` e `[EmailAddress]` nos DTOs. Requisições inválidas retornam `400` automaticamente.

- [x] **9. `CategoriasController` expunha a entidade direto** — recebia e devolvia `Categoria` (o cliente podia mandar até o `Id` no corpo).
  **Correção:** criados `CategoriaRequestDto` e `CategoriaResponseDto` em `DTOs/CategoriaDtos.cs`.

- [x] **10. Configurações do banco** — faltavam precisão do valor, tamanho das colunas e e-mail único.
  **Correção:** `OnModelCreating` no `AppDbContext`, com `Valor` em `numeric(18,2)`, limites de tamanho nos textos e índice único em `Usuario.Email`.

- [x] **11. `Categoria.Tipo` era texto livre** — aceitava qualquer valor.
  **Correção:** criado o enum `TipoCategoria` (`Receita`, `Despesa`). No banco ele é salvo como texto e no JSON aparece como `"Receita"`/`"Despesa"` (`JsonStringEnumConverter` no `Program.cs`).

- [x] **12. `_repository.Atualizar()` desnecessário** — a entidade já vem rastreada pelo EF, então `Update()` só marcava todas as colunas como alteradas.
  **Correção:** método removido. O service altera os campos e chama `SalvarAsync()`.

- [x] **13. Namespace diferente do nome do projeto** — o projeto se chama `meuprojeto`, mas o código usa `apicsharp` (o VS Code mostrava avisos).
  **Correção:** `<RootNamespace>apicsharp</RootNamespace>` no `.csproj`.

- [x] **14. Faltava a connection string** — adicionada em `appsettings.Development.json`, com valores de exemplo.

### ⏳ Pendências (ainda não feitas)

- [ ] **Ajustar a connection string** em `appsettings.Development.json` com a senha real do PostgreSQL. Para não subir a senha no Git, prefira o *user-secrets*:
  ```bash
  dotnet user-secrets init
  dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=controle_financeiro;Username=postgres;Password=SENHA_REAL"
  ```
- [ ] **Criar a primeira migration e o banco:**
  ```bash
  dotnet ef migrations add Inicial
  dotnet ef database update
  ```
- [ ] **Autenticação JWT** — o `TransacoesController` ainda usa `UsuarioIdFixo = 1`. Falta criar o login (`/api/auth/login`), gerar o token e ler o Id do usuário pelo token.
- [ ] **Proteger o `UsuariosController`** — depois do JWT, um usuário só deve poder ver, editar e apagar a própria conta.
- [ ] **Endpoints de relatórios** (`/api/relatorios/...`) citados neste README ainda não existem.
- [ ] **Filtros na listagem de transações** (período/categoria/tipo) citados neste README ainda não existem.
- [ ] **Swagger UI** — o README fala em `/swagger`, mas o projeto só expõe o JSON do OpenAPI (`/openapi/v1.json`). Para ter a tela, adicione um pacote como `Scalar.AspNetCore` ou `Swashbuckle.AspNetCore`.
- [ ] **README desatualizado:** cita .NET 8 (o projeto usa .NET 10) e a pasta `ControleFinanceiro.Api/` (a pasta real é `apicsharp/`).
