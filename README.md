# Daymark API

API REST em C# para o projeto Daymark, criada com ASP.NET Core 8. Ela fornece uma base para o dashboard Angular consultar, criar, atualizar e remover tarefas.

## Recursos

- `GET /api/health` para verificar a disponibilidade
- `GET /api/tasks` com filtros opcionais `tag` e `completed`
- `GET /api/tasks/{id}` para consultar uma tarefa
- `POST /api/tasks` para criar uma tarefa
- `PATCH /api/tasks/{id}` para atualizar campos da tarefa
- `DELETE /api/tasks/{id}` para remover uma tarefa
- Swagger/OpenAPI em ambiente de desenvolvimento

Os dados ficam em memoria nesta primeira versao e sao reiniciados quando a API e reiniciada.

## Executar localmente

Requisito: .NET 8 SDK.

```powershell
dotnet restore
dotnet run
```

A API sera iniciada em `http://localhost:5042` quando executada pelo perfil HTTP. A documentacao interativa fica em `/swagger`.

## Exemplo

```powershell
Invoke-RestMethod http://localhost:5042/api/tasks

Invoke-RestMethod http://localhost:5042/api/tasks `
  -Method Post `
  -ContentType 'application/json' `
  -Body '{"title":"Estudar C#","tag":"Pessoal","time":"21:00"}'
```

## Publicar no GitHub

Crie um repositorio vazio chamado `daymark-api` no GitHub e execute:

```powershell
git init
git add .
git commit -m "feat: create Daymark CSharp API"
git branch -M main
git remote add origin https://github.com/EstelaAPC/daymark-api.git
git push -u origin main
```
