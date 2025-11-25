# FitLife API — Explicação para Apresentação (versão iniciante)

Este README explica de forma simples e direta como o projeto está organizado e responde às perguntas mais prováveis do professor. Use isso como roteiro na apresentação.

## Visão geral
- Projeto: `FitLifeAPI` (ASP.NET Core Web API, .NET8)
- Objetivo: gerenciar usuários, treinos, refeições, hábitos e histórico simples.
- Banco: EF Core (SQL Server por padrão). Para iniciantes o projeto cria o banco automaticamente com `EnsureCreated()` e insere um seed mínimo.
- Documentação: Swagger disponível em `/swagger/index.html` após rodar a API.

## Como rodar (passos rápidos)
1. Restaurar pacotes: `dotnet restore`
2. Executar: `dotnet run --project FitLifeAPI`
3. Abrir Swagger: `https://localhost:{PORT}/swagger/index.html` (veja a porta no terminal)

> Nota: para apresentação, não é obrigatório executar migrações. O projeto usa `EnsureCreated()` para criar o DB automaticamente.

---

## Estrutura principal (o que mostrar e explicar)

- `Program.cs`
 - Configura serviços, Swagger, EF Core e CORS.
 - Contém lógica de *seed* simples (cria1 usuário +1 treino se o DB estiver vazio).
 - Para apresentação: explique que aqui é o "ponto de entrada" e onde registramos dependências.

- `Dados/FitLifeContexto.cs`
 - Herda de `DbContext` e expõe `DbSet<T>` para as tabelas (Usuários, Treinos, Refeições, etc.).
 - Explica o relacionamento entre entidades (One-to-Many: `Usuario` -> `Treino`).

- `Modelos/` (ex.: `Usuario.cs`, `Treino.cs`, `TreinoForca.cs`, `TreinoCardio.cs`)
 - `Usuario`: propriedades básicas (Id, Nome, Email, Peso, Altura, Objetivo) e método auxiliar `CalcularIMC()`.
 - `Treino` (classe abstrata) e classes filhas (`TreinoForca`, `TreinoCardio`): exemplo de POO (herança e polimorfismo).
 - Como explicar: mostre que `Treino` define contratos (métodos abstratos) e as classes filhas implementam comportamentos diferentes.

- `DTOs/` (Data Transfer Objects)
 - Objetivo: controlar o que a API envia/recebe (evita expor diretamente entidades do DB quando necessário).
 - Exemplos: `CriarUsuarioDTO`, `UsuarioDTO`, `CriarTreinoDTO`, `TreinoDTO`.

- `Controllers/` (ex.: `UsuariosController`, `TreinosController`, `HistoricoController`)
 - **Modelo do professor (simplificado)**: controllers podem injetar `DbContext` e executar operações EF diretamente — fácil de explicar.
 - Neste projeto usamos uma mistura: controllers simples para CRUD e um `HistoricoController` que usa `IServicoLinq` para lógica LINQ/IA (mantém controller limpo).
 - Aponte exemplos: `CreatedAtAction` ao criar recursos, `Include` para carregar relacionamentos.

- `Servicos/` (opcional para lógica)
 - `ServicoUsuario`, `ServicoTreino`, `IServicoLinq` etc. Extraem lógica do controller quando fica muito complexa.
 - Para apresentação: explique que serviços deixam o controller menor e facilitam testes (mas não são obrigatórios para trabalho de aula).

---

## Perguntas que o professor provavelmente fará (e respostas curtas)

1. Por que usar EF Core? 
 Resposta: facilita trabalhar com banco de dados usando classes C# e LINQ; evita escrever SQL manualmente para operações CRUD básicas.

2. O que é `DbContext`? 
 Resposta: é a classe que representa a sessão com o banco; expõe `DbSet<T>` para cada tabela e aplica configurações de modelo.

3. Por que usar DTOs? 
 Resposta: para controlar o formato de entrada/saída da API, evitar expor campos sensíveis e manter contrato estável entre cliente e servidor.

4. Por que há serviços além dos controllers? 
 Resposta: para separar responsabilidades — controllers cuidam de HTTP; serviços cuidam da lógica de negócio. Mas para iniciantes, é aceitável usar DbContext direto no controller.

5. Como funciona a relação Usuário ? Treinos? 
 Resposta: é One-to-Many. Um `Usuario` possui uma coleção de `Treino`s; cada `Treino` tem `UsuarioId` como FK.

6. O que faz o seed no `Program.cs`? 
 Resposta: cria o banco (se não existir) e insere dados iniciais para testar a API sem passos adicionais.

7. Como demonstrar uma requisição durante a apresentação? 
 Resposta: abra Swagger e execute: criar usuário (POST), criar treino (POST), listar treinos por usuário (GET). Mostre o `CreatedAtAction` retornando a URL do recurso.

---

## Pontos que você deve praticar antes da apresentação
- Rodar a API e abrir Swagger para executar os endpoints principais.
- Entender e explicar3 endpoints: criar usuário, criar treino, obter histórico/ranking.
- Explicar brevemente o que é herança (mostre `TreinoForca` e `TreinoCardio`) e LINQ (no `HistoricoController`).

---

Se quiser, eu aplico automaticamente comentários inline nos arquivos principais (`Program.cs`, `Dados/FitLifeContexto.cs`, `Modelos/*.cs`, `Controllers/*.cs`) ou refatoro `UsuariosController`/`TreinosController` para o modelo do professor (usar `DbContext` diretamente). Diga qual opção prefere.