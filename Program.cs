using Daymark.Api.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

var tasks = new List<DaymarkTask>
{
    new(1, "Revisar proposta da Aurora", "Trabalho", "09:00", true),
    new(2, "Caminhar por 30 minutos", "Bem-estar", "12:30", false),
    new(3, "Ler 20 paginas", "Pessoal", "18:00", false),
    new(4, "Planejar amanha", "Trabalho", "20:30", false)
};

app.MapGet("/api/health", () => Results.Ok(new { status = "ok", service = "daymark-api" }))
    .WithName("GetHealth")
    .WithOpenApi();

app.MapGet("/api/tasks", (string? tag, bool? completed) =>
{
    var result = tasks.AsEnumerable();
    if (!string.IsNullOrWhiteSpace(tag)) result = result.Where(task => task.Tag.Equals(tag, StringComparison.OrdinalIgnoreCase));
    if (completed.HasValue) result = result.Where(task => task.Completed == completed.Value);
    return Results.Ok(result);
})
    .WithName("ListTasks")
    .WithOpenApi();

app.MapGet("/api/tasks/{id:int}", (int id) =>
    tasks.FirstOrDefault(task => task.Id == id) is { } task
        ? Results.Ok(task)
        : Results.NotFound(new { message = "Tarefa nao encontrada." }))
    .WithName("GetTask")
    .WithOpenApi();

app.MapPost("/api/tasks", (CreateTaskRequest request) =>
{
    if (string.IsNullOrWhiteSpace(request.Title)) return Results.BadRequest(new { message = "O titulo e obrigatorio." });

    var task = new DaymarkTask(
        tasks.Count == 0 ? 1 : tasks.Max(item => item.Id) + 1,
        request.Title.Trim(),
        string.IsNullOrWhiteSpace(request.Tag) ? "Pessoal" : request.Tag.Trim(),
        string.IsNullOrWhiteSpace(request.Time) ? "Agora" : request.Time.Trim(),
        false);
    tasks.Add(task);
    return Results.Created($"/api/tasks/{task.Id}", task);
})
    .WithName("CreateTask")
    .WithOpenApi();

app.MapPatch("/api/tasks/{id:int}", (int id, UpdateTaskRequest request) =>
{
    var index = tasks.FindIndex(task => task.Id == id);
    if (index < 0) return Results.NotFound(new { message = "Tarefa nao encontrada." });

    var current = tasks[index];
    var updated = current with
    {
        Title = request.Title?.Trim() ?? current.Title,
        Tag = request.Tag?.Trim() ?? current.Tag,
        Time = request.Time?.Trim() ?? current.Time,
        Completed = request.Completed ?? current.Completed
    };
    tasks[index] = updated;
    return Results.Ok(updated);
})
    .WithName("UpdateTask")
    .WithOpenApi();

app.MapDelete("/api/tasks/{id:int}", (int id) =>
{
    var removed = tasks.RemoveAll(task => task.Id == id);
    return removed > 0 ? Results.NoContent() : Results.NotFound(new { message = "Tarefa nao encontrada." });
})
    .WithName("DeleteTask")
    .WithOpenApi();

app.Run();
