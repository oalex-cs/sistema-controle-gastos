# sistema-controle-gastos
# Gerenciador Financeiro

Aplicação full stack para gerenciamento financeiro de pessoas, desenvolvida com **ASP.NET Core**, **Entity Framework Core**, **SQLite**, **React** e **TypeScript**.

O sistema permite cadastrar pessoas, registrar receitas e despesas, excluir pessoas com suas respectivas transações e consultar totais financeiros individuais e gerais.

A implementação prioriza código legível, separação de responsabilidades, validação de dados, integridade no banco e uma interface responsiva.

---

## Sobre o projeto

O Gerenciador Financeiro foi desenvolvido para solucionar um desafio de controle de gastos com os seguintes recursos:

* Cadastro de pessoas;
* Listagem de pessoas;
* Exclusão de pessoas;
* Cadastro de receitas e despesas;
* Listagem de transações;
* Cálculo de receitas, despesas e saldo por pessoa;
* Cálculo dos totais gerais do sistema;
* Restrição de receitas para menores de idade;
* Exclusão automática das transações ao remover uma pessoa;
* Tratamento padronizado de erros;
* Interface responsiva integrada à API.

O projeto utiliza uma arquitetura simples e proporcional ao seu tamanho, evitando abstrações e camadas que não agregariam valor neste contexto.

---

## Tecnologias utilizadas

### Backend

* C#;
* .NET 10;
* ASP.NET Core Web API;
* Controllers;
* Entity Framework Core;
* SQLite;
* LINQ;
* Data Annotations;
* Problem Details;
* Swagger / OpenAPI;
* xUnit.

### Frontend

* React;
* TypeScript;
* Vite;
* Fetch API;
* CSS responsivo;
* Variáveis de ambiente;
* Componentização;
* Design system próprio.

---

## Funcionalidades

### Pessoas

* Cadastrar uma pessoa com nome e idade;
* Listar todas as pessoas cadastradas;
* Excluir uma pessoa;
* Remover automaticamente as transações relacionadas à pessoa excluída.

### Transações

* Cadastrar receitas e despesas;
* Relacionar cada transação a uma pessoa;
* Listar todas as transações;
* Exibir o nome da pessoa responsável por cada transação;
* Validar valores, tipos e identificadores enviados.

### Totais

* Calcular o total de receitas por pessoa;
* Calcular o total de despesas por pessoa;
* Calcular o saldo individual;
* Calcular as receitas gerais;
* Calcular as despesas gerais;
* Calcular o saldo geral da aplicação.

---

## Regras de negócio

### Pessoas menores de idade

Pessoas com menos de 18 anos podem cadastrar apenas despesas.

Uma tentativa de cadastrar uma receita para uma pessoa menor de idade resulta em uma resposta HTTP `422 Unprocessable Entity`.

### Valores das transações

Toda transação deve possuir valor maior que zero.

Valores negativos não são usados para representar despesas. A natureza da operação é definida pelo tipo da transação:

```text
1 - Despesa
2 - Receita
```

Exemplo de despesa válida:

```json
{
  "descricao": "Conta de energia",
  "valor": 150.00,
  "tipo": 1,
  "pessoaId": "identificador-da-pessoa"
}
```

### Exclusão em cascata

Uma pessoa pode possuir várias transações.

Quando uma pessoa é excluída, todas as suas transações também são removidas automaticamente pelo banco de dados através do comportamento `Cascade`.

### Integridade dos dados

O banco possui restrições para impedir:

* Idades negativas ou fora do intervalo definido;
* Transações com valor igual ou menor que zero;
* Tipos de transação inválidos;
* Transações relacionadas a pessoas inexistentes;
* Campos obrigatórios com valor nulo;
* Textos maiores que os limites permitidos.

---

## Arquitetura

O backend segue uma estrutura em camadas simples:

```text
Controller
    ↓
Service
    ↓
DbContext
    ↓
Banco de dados
```

### Controllers

Responsáveis por:

* Receber requisições HTTP;
* Encaminhar dados para os serviços;
* Converter resultados em respostas HTTP;
* Retornar códigos de status coerentes;
* Documentar os endpoints no Swagger.

Os Controllers não concentram regras de negócio.

### Services

Responsáveis por:

* Aplicar regras de negócio;
* Consultar e modificar os dados;
* Converter entidades em DTOs;
* Retornar resultados específicos para os Controllers.

### DTOs

Responsáveis por definir os contratos de entrada e saída da API.

São separados das entidades para:

* Evitar exposição direta do modelo do banco;
* Controlar os campos aceitos em cada operação;
* Aplicar validações estruturais;
* Reduzir acoplamento entre API e persistência;
* Evitar alterações indevidas em propriedades internas.

### Models

Representam as entidades persistidas no banco:

* `Pessoa`;
* `Transacao`;
* `TipoTransacao`.

### AppDbContext

Responsável por:

* Mapear entidades para tabelas;
* Definir chaves primárias;
* Configurar campos obrigatórios;
* Definir limites de tamanho;
* Configurar relacionamentos;
* Criar índices;
* Aplicar restrições de integridade;
* Configurar exclusão em cascata.

### Tratamento de erros

A API possui um tratador global para exceções inesperadas.

Esse recurso:

* Registra a exceção completa nos logs;
* Retorna uma resposta segura ao cliente;
* Não expõe stack trace ou detalhes internos;
* Retorna um identificador `traceId`;
* Utiliza o formato padronizado `ProblemDetails`.

---

## Estrutura de pastas

```text
sistema-controle-gastos/
├── backend/
│   ├── src/
│   │   └── GerenciadorFinanceiro.Api/
│   │       ├── Controllers/
│   │       │   ├── PessoasController.cs
│   │       │   ├── TransacoesController.cs
│   │       │   └── TotaisController.cs
│   │       ├── Data/
│   │       │   └── AppDbContext.cs
│   │       ├── Dtos/
│   │       │   ├── Pessoas/
│   │       │   ├── Transacoes/
│   │       │   └── Totais/
│   │       ├── ErrorHandling/
│   │       │   └── ApiExceptionHandler.cs
│   │       ├── Migrations/
│   │       ├── Models/
│   │       │   ├── Pessoa.cs
│   │       │   ├── Transacao.cs
│   │       │   └── TipoTransacao.cs
│   │       ├── Services/
│   │       │   ├── Resultados/
│   │       │   ├── PessoaService.cs
│   │       │   ├── TransacaoService.cs
│   │       │   └── TotaisService.cs
│   │       ├── Program.cs
│   │       └── appsettings.json
│   └── tests/
│       └── GerenciadorFinanceiro.Api.Tests/
├── frontend/
│   ├── public/
│   ├── src/
│   │   ├── components/
│   │   ├── services/
│   │   ├── styles/
│   │   ├── types/
│   │   ├── App.tsx
│   │   └── main.tsx
│   ├── .env.example
│   ├── package.json
│   └── vite.config.ts
├── GerenciadorFinanceiro.slnx
└── README.md
```

Dependendo da versão do SDK utilizada para criar a solution, o arquivo pode possuir a extensão `.sln` em vez de `.slnx`.

---

## Pré-requisitos

Antes de executar o projeto, instale:

* .NET 10 SDK;
* Node.js;
* npm;
* Git;
* Um editor como Visual Studio Code ou Visual Studio.

Para confirmar as instalações:

```bash
dotnet --version
node --version
npm --version
git --version
```

Também é recomendado instalar a ferramenta do Entity Framework Core:

```bash
dotnet tool install --global dotnet-ef
```

Caso ela já esteja instalada:

```bash
dotnet tool update --global dotnet-ef
```

Verifique a instalação:

```bash
dotnet ef --version
```

---

# Instalação

## 1. Clonar o repositório

```bash
git clone https://github.com/oalex-cs/sistema-controle-gastos.git
```

Entre na pasta:

```bash
cd sistema-controle-gastos
```

---

## 2. Restaurar o backend

Caso a solution possua extensão `.slnx`:

```bash
dotnet restore GerenciadorFinanceiro.slnx
```

Caso possua extensão `.sln`:

```bash
dotnet restore GerenciadorFinanceiro.sln
```

Também é possível restaurar somente a API:

```bash
dotnet restore backend/src/GerenciadorFinanceiro.Api
```

---

## 3. Configurar o banco de dados

A aplicação utiliza SQLite.

A string de conexão fica no arquivo:

```text
backend/src/GerenciadorFinanceiro.Api/appsettings.json
```

Exemplo:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=gerenciador-financeiro.db"
  }
}
```

Aplique as migrations:

```bash
dotnet ef database update \
  --project backend/src/GerenciadorFinanceiro.Api \
  --startup-project backend/src/GerenciadorFinanceiro.Api
```

No PowerShell, também é possível executar:

```powershell
dotnet ef database update `
  --project backend/src/GerenciadorFinanceiro.Api `
  --startup-project backend/src/GerenciadorFinanceiro.Api
```

O comando criará o arquivo do banco SQLite caso ele ainda não exista.

---

## 4. Executar a API

```bash
dotnet run --project backend/src/GerenciadorFinanceiro.Api
```

Caso exista um perfil chamado `http`:

```bash
dotnet run \
  --project backend/src/GerenciadorFinanceiro.Api \
  --launch-profile http
```

A URL utilizada durante o desenvolvimento é:

```text
http://localhost:5214
```

A documentação Swagger poderá ser acessada em:

```text
http://localhost:5214/swagger
```

Caso a porta exibida no terminal seja diferente, utilize a URL informada durante a inicialização.

---

## 5. Configurar o frontend

Abra outro terminal e entre na pasta do frontend:

```bash
cd frontend
```

Instale as dependências:

```bash
npm install
```

Crie o arquivo `.env` a partir do exemplo.

No Windows PowerShell:

```powershell
Copy-Item .env.example .env
```

No Linux ou macOS:

```bash
cp .env.example .env
```

Conteúdo esperado:

```env
VITE_API_URL=http://localhost:5214
```

A variável deve apontar para a URL em que o backend está sendo executado.

---

## 6. Executar o frontend

```bash
npm run dev
```

O Vite exibirá a URL da aplicação, normalmente:

```text
http://localhost:5173
```

Acesse essa URL pelo navegador.

---

# Como usar

## Cadastrar uma pessoa

1. Acesse a seção de pessoas;
2. Informe o nome;
3. Informe a idade;
4. Clique no botão de cadastro.

Exemplo:

```text
Nome: Maria da Silva
Idade: 25
```

Após o cadastro, a pessoa ficará disponível para receber transações.

---

## Cadastrar uma transação

1. Acesse a seção de transações;
2. Informe uma descrição;
3. Informe um valor maior que zero;
4. Escolha entre receita e despesa;
5. Selecione a pessoa responsável;
6. Confirme o cadastro.

Exemplo de receita:

```text
Descrição: Salário
Valor: 2500,00
Tipo: Receita
Pessoa: Maria da Silva
```

Exemplo de despesa:

```text
Descrição: Conta de energia
Valor: 180,50
Tipo: Despesa
Pessoa: Maria da Silva
```

Uma pessoa menor de 18 anos não poderá receber uma transação do tipo receita.

---

## Consultar os totais

A seção de totais apresenta:

* Receitas de cada pessoa;
* Despesas de cada pessoa;
* Saldo de cada pessoa;
* Total geral de receitas;
* Total geral de despesas;
* Saldo geral.

O saldo é calculado da seguinte forma:

```text
Saldo = Receitas - Despesas
```

---

## Excluir uma pessoa

Ao excluir uma pessoa:

1. O frontend solicita a exclusão à API;
2. A API remove a pessoa;
3. O banco remove automaticamente as transações relacionadas;
4. As listagens e os totais são atualizados.

Essa operação não pode ser desfeita pela aplicação.

---

# Endpoints da API

## Pessoas

### Cadastrar pessoa

```http
POST /api/pessoas
```

Corpo da requisição:

```json
{
  "nome": "Maria da Silva",
  "idade": 25
}
```

Resposta de sucesso:

```http
201 Created
```

Exemplo:

```json
{
  "id": "991f349f-7c52-4f40-b765-fdc8f2331c81",
  "nome": "Maria da Silva",
  "idade": 25
}
```

---

### Listar pessoas

```http
GET /api/pessoas
```

Resposta:

```http
200 OK
```

---

### Excluir pessoa

```http
DELETE /api/pessoas/{id}
```

Possíveis respostas:

```text
204 No Content
404 Not Found
```

---

## Transações

### Cadastrar transação

```http
POST /api/transacoes
```

Corpo da requisição:

```json
{
  "descricao": "Conta de energia",
  "valor": 180.50,
  "tipo": 1,
  "pessoaId": "991f349f-7c52-4f40-b765-fdc8f2331c81"
}
```

Possíveis respostas:

```text
201 Created
400 Bad Request
404 Not Found
422 Unprocessable Entity
```

O status `422` é retornado quando uma pessoa menor de idade tenta cadastrar uma receita.

---

### Listar transações

```http
GET /api/transacoes
```

Resposta:

```http
200 OK
```

---

## Totais

### Consultar totais

```http
GET /api/totais
```

Resposta:

```http
200 OK
```

Exemplo simplificado:

```json
{
  "totaisPorPessoa": [
    {
      "pessoaId": "991f349f-7c52-4f40-b765-fdc8f2331c81",
      "pessoaNome": "Maria da Silva",
      "totalReceitas": 2500.00,
      "totalDespesas": 180.50,
      "saldo": 2319.50
    }
  ],
  "totalGeral": {
    "totalReceitas": 2500.00,
    "totalDespesas": 180.50,
    "saldo": 2319.50
  }
}
```

---

# Códigos HTTP utilizados

| Código | Significado           | Uso                                              |
| ------ | --------------------- | ------------------------------------------------ |
| `200`  | OK                    | Consultas realizadas com sucesso                 |
| `201`  | Created               | Recurso criado com sucesso                       |
| `204`  | No Content            | Exclusão realizada sem corpo de resposta         |
| `400`  | Bad Request           | Dados enviados são estruturalmente inválidos     |
| `404`  | Not Found             | Pessoa ou recurso não encontrado                 |
| `422`  | Unprocessable Entity  | Requisição válida que viola uma regra de negócio |
| `500`  | Internal Server Error | Erro inesperado na aplicação                     |

---

# Validações

## Pessoa

O cadastro de pessoa valida:

* Nome obrigatório;
* Nome com no máximo 100 caracteres;
* Idade obrigatória;
* Idade não negativa;
* Limite máximo de idade definido pela aplicação.

## Transação

O cadastro de transação valida:

* Descrição obrigatória;
* Descrição com no máximo 200 caracteres;
* Valor maior que zero;
* Tipo de transação válido;
* Identificador de pessoa obrigatório;
* Existência da pessoa no banco;
* Restrição de receita para menores de idade.

As validações são divididas em dois grupos.

### Validações estruturais

São realizadas nos DTOs:

* Campos obrigatórios;
* Tamanhos máximos;
* Intervalos;
* Formatos;
* Valores padrão inválidos.

### Regras de negócio

São realizadas nos Services:

* Verificar se a pessoa existe;
* Impedir receita para menores;
* Aplicar comportamentos que dependem de consultas ao banco.

---

# Frontend e tratamento de erros

O frontend centraliza as chamadas HTTP em uma função genérica:

```ts
apiRequest<T>()
```

Essa função é responsável por:

* Montar a URL da API;
* Configurar os headers;
* Definir JSON como formato padrão;
* Executar a chamada com `fetch`;
* Tratar respostas sem conteúdo;
* Interpretar erros no formato `ProblemDetails`;
* Lançar um erro personalizado `ApiError`.

O tipo `ApiError` mantém:

* Status HTTP;
* Mensagem;
* Detalhes retornados pela API;
* Erros de validação;
* Identificador de rastreamento.

Isso permite que os componentes exibam mensagens adequadas sem repetir a lógica de tratamento HTTP.

---

# Design system

A interface utiliza um design system inspirado em uma linguagem editorial, minimalista e tecnológica.

Os principais elementos são:

* Alto contraste entre preto, branco e off-white;
* Verde-limão como cor de destaque;
* Tipografia de grande escala;
* Bordas marcadas;
* Poucas sombras;
* Espaçamento generoso;
* Inputs lineares;
* Layout responsivo;
* Componentes reutilizáveis;
* Estados vazios, carregamento e feedback;
* Suporte a redução de movimento.

Os tokens visuais estão centralizados em:

```text
frontend/src/styles/design-tokens.css
```

Os estilos globais estão em:

```text
frontend/src/styles/globals.css
```

Os estilos específicos da aplicação estão em:

```text
frontend/src/styles/app.css
```

Essa organização reduz duplicação e mantém consistência visual entre os componentes.

---

# Testes

O projeto possui testes automatizados para comportamentos relevantes, como:

* Validação de idade;
* Validação de campos obrigatórios;
* Pessoa menor tentando cadastrar receita;
* Pessoa menor cadastrando despesa;
* Exclusão em cascata;
* Cálculo de receitas;
* Cálculo de despesas;
* Cálculo de saldo.

Execute os testes com:

```bash
dotnet test
```

Ou apontando para a solution:

```bash
dotnet test GerenciadorFinanceiro.slnx
```

Caso utilize `.sln`:

```bash
dotnet test GerenciadorFinanceiro.sln
```

---

# Validação do projeto

## Backend

Restaurar dependências:

```bash
dotnet restore
```

Compilar:

```bash
dotnet build
```

Executar testes:

```bash
dotnet test
```

Verificar formatação:

```bash
dotnet format --verify-no-changes
```

## Frontend

Instalar dependências:

```bash
npm install
```

Executar lint:

```bash
npm run lint
```

Gerar build de produção:

```bash
npm run build
```

Executar ambiente de desenvolvimento:

```bash
npm run dev
```

---

# Decisões técnicas

## Uso de Services

As regras de negócio foram concentradas em Services para evitar Controllers extensos e facilitar testes e manutenção.

## Ausência de Repository Pattern genérico

O Entity Framework Core já fornece abstrações adequadas através de `DbContext` e `DbSet`.

Adicionar um repositório genérico neste projeto criaria uma camada adicional sem benefício proporcional à complexidade atual.

## Uso de DTOs

As entidades do banco não são usadas diretamente como contratos externos.

Isso protege o modelo interno e evita mass assignment, acoplamento e exposição de propriedades que não deveriam ser alteradas pelo cliente.

## Uso de `AsNoTracking`

Consultas somente de leitura utilizam `AsNoTracking()` para evitar o rastreamento desnecessário de entidades pelo Entity Framework Core.

## Uso de projeções

As consultas selecionam apenas os campos necessários para cada operação.

Isso reduz o volume de dados carregado e evita consultas desnecessárias.

## Uso de `CancellationToken`

As operações assíncronas propagam o `CancellationToken` recebido pela requisição HTTP.

Assim, consultas e gravações podem ser interrompidas caso o cliente cancele a requisição.

## Uso de `decimal`

Valores financeiros utilizam `decimal`, evitando problemas de precisão comuns em tipos como `float` e `double`.

## Uso de `Guid`

Os identificadores utilizam `Guid`, permitindo geração independente e reduzindo exposição de sequências numéricas do banco.

---

# Possíveis melhorias futuras

* Autenticação e autorização;
* Usuários com dados financeiros separados;
* Edição de pessoas;
* Edição e exclusão de transações;
* Filtros por pessoa, tipo ou período;
* Paginação nas listagens;
* Categorias de despesas e receitas;
* Gráficos financeiros;
* Exportação para CSV ou PDF;
* Dashboard com indicadores;
* Testes de integração dos endpoints;
* Docker;
* Logs com persistência;
* Deploy automatizado;
* CI/CD com GitHub Actions;
* Banco PostgreSQL em produção.

Essas funcionalidades não foram adicionadas ao núcleo atual para preservar a simplicidade e manter o projeto adequado ao escopo proposto.

---

# Solução de problemas

## A solution não foi encontrada

Erro:

```text
MSBUILD : error MSB1009: Arquivo de projeto não existe.
```

Verifique qual arquivo existe:

```powershell
Get-ChildItem *.sln*
```

Caso o arquivo seja:

```text
GerenciadorFinanceiro.slnx
```

Execute:

```bash
dotnet restore GerenciadorFinanceiro.slnx
```

---

## A API não conecta ao banco

Execute:

```bash
dotnet ef database update \
  --project backend/src/GerenciadorFinanceiro.Api \
  --startup-project backend/src/GerenciadorFinanceiro.Api
```

Também confirme a string de conexão em `appsettings.json`.

---

## O frontend não conecta à API

Confira o arquivo:

```text
frontend/.env
```

Ele deve possuir:

```env
VITE_API_URL=http://localhost:5214
```

Após alterar o `.env`, reinicie o Vite:

```bash
npm run dev
```

Também confirme se a API está em execução e se a porta está correta.

---

## Erro de CORS

Confirme que a origem do frontend está autorizada no backend.

Durante o desenvolvimento, normalmente é:

```text
http://localhost:5173
```

---

## A porta já está em uso

Para o backend, utilize outro perfil ou altere a URL configurada em `launchSettings.json`.

Para o frontend:

```bash
npm run dev -- --port 5174
```

Nesse caso, atualize também a configuração de CORS no backend.

---

# Status do projeto

* [x] Cadastro de pessoas;
* [x] Listagem de pessoas;
* [x] Exclusão de pessoas;
* [x] Exclusão automática de transações;
* [x] Cadastro de transações;
* [x] Regra para menores de idade;
* [x] Listagem de transações;
* [x] Totais por pessoa;
* [x] Total geral;
* [x] Validação de entrada;
* [x] Tratamento global de erros;
* [x] Swagger;
* [x] Interface React;
* [x] Layout responsivo;
* [x] Design system;
* [x] Testes automatizados.

---

# Autor

Desenvolvido por **Alex Castilho**.

GitHub: `@oalex-cs`

