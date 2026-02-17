using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;



var builder = WebApplication.CreateBuilder(args);

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// In-memory task storage
var tasks = new ConcurrentDictionary<int, TaskItem>();
var nextId = 1;

// Group all endpoints under /api/v1/tasks
var taskRoutes = app.MapGroup("/api/v1/tasks");

// GET /api/v1/tasks/ - Retrieve all tasks
taskRoutes.MapGet("/", () =>
{
    return Results.Ok(tasks.Values);
})
.WithName("GetAllTasks")
.WithOpenApi(); // <- Swagger için açıklama eklemesi

// GET /api/v1/tasks/{id} - Retrieve a specific task
taskRoutes.MapGet("/{id:int}", (int id) =>
{
    if (tasks.TryGetValue(id, out var task))
        return Results.Ok(task);

    return Results.NotFound();
})
.WithName("GetTaskById")
.WithOpenApi();

// POST /api/v1/tasks/ - Create a new task
taskRoutes.MapPost("/", ([FromBody] TaskCreateRequest request) =>
{
    if (string.IsNullOrWhiteSpace(request.Title))
        return Results.BadRequest("Title is required.");

    var task = new TaskItem(nextId++, request.Title, request.IsCompleted, request.Description);
    tasks.TryAdd(task.Id, task);

    return Results.Created($"/api/v1/tasks/{task.Id}", task);
})
.WithName("CreateTask")
.WithOpenApi();

// PUT /api/v1/tasks/{id} - Update an existing task
taskRoutes.MapPut("/{id:int}", (int id, [FromBody] TaskUpdateRequest request) =>
{
    if (!tasks.ContainsKey(id))
        return Results.NotFound();

    if (string.IsNullOrWhiteSpace(request.Title))
        return Results.BadRequest("Title is required.");

    var updatedTask = new TaskItem(id, request.Title, request.IsCompleted, request.Description);
    tasks[id] = updatedTask;

    return Results.Ok(updatedTask);
})
.WithName("UpdateTask")
.WithOpenApi();

// DELETE /api/v1/tasks/{id} - Delete a task
taskRoutes.MapDelete("/{id:int}", (int id) =>
{
    if (tasks.TryRemove(id, out _))
        return Results.NoContent();

    return Results.NotFound();
})
.WithName("DeleteTask")
.WithOpenApi();

app.Run();

// Task model
public record TaskItem(int Id, string Title, bool IsCompleted, string? Description = null);

// Request models
public record TaskCreateRequest(string Title, bool IsCompleted, string? Description);
public record TaskUpdateRequest(string Title, bool IsCompleted, string? Description);