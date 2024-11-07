using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using MyDbContext.Models;

namespace DemoMinimalAPIDotNet8.Models;

public static class TodoItemsEndpoints
{
    public static void RegisterTodoItemsEndpoints(this WebApplication app)
    {
        var todoItems = app.MapGroup("/todoitems");

        todoItems.MapGet("/",  GetTodos)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "This is a summary",
                Description = "This is a description"
            });
        todoItems.MapGet("/complete", GetCompleteTodos);
        todoItems.MapGet("/{id}", GetTodoById);
        todoItems.MapPost("/", PostTodoItem).
            WithValidation<ToDo>();
        // .AddEndpointFilter<MyValidationFilter<ToDo>>();
        todoItems.MapPut("/{id}", PutToDoItem);
        todoItems.MapDelete("/{id}", DeleteToDoItem );
    }

    static async Task<Results<Ok<List<ToDo>>, NotFound>> 
        GetTodos(TodoDb db) => await db.Todos.ToListAsync()
                is List<ToDo> todos
                    ? TypedResults.Ok(todos)
                    : TypedResults.NotFound();

    static async Task<Results<Ok<List<ToDo>>, NotFound>>
       GetCompleteTodos(TodoDb db) => await db.Todos.Where(x => x.IsComplete).ToListAsync()
               is List<ToDo> todos
                   ? TypedResults.Ok(todos)
                   : TypedResults.NotFound();

    static async Task<Results<Ok<ToDo>, NotFound>> GetTodoById(int id, TodoDb db) =>
    await db.Todos.FindAsync(id)
        is ToDo todo
            ? TypedResults.Ok(todo)
            : TypedResults.NotFound();

    static async Task<Created<ToDo>> PostTodoItem(ToDo todo, TodoDb db)
    {
        db.Todos.Add(todo);
        await db.SaveChangesAsync();
        return TypedResults.Created($"/todoitems/{todo.Id}", todo);
    }

    static async Task<Results<NotFound, NoContent>> PutToDoItem(int id, ToDo inputTodo, TodoDb db)
    {
        var todo = await db.Todos.FindAsync(id);

        if (todo is null) return TypedResults.NotFound();

        todo.Name = inputTodo.Name;
        todo.IsComplete = inputTodo.IsComplete;

        await db.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    static async Task<Results<NotFound, NoContent>> DeleteToDoItem(int id, TodoDb db)
    {
        if (await db.Todos.FindAsync(id) is ToDo todo)
        {
            db.Todos.Remove(todo);
            await db.SaveChangesAsync();
            return TypedResults.NoContent();
        }

        return TypedResults.NotFound();
    }

}