using DemoAPI.Data;
using DemoMinimalAPIDotNet8.ExtensionsMethods;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace DemoMinimalAPIDotNet8;

public static class TodoItemsEndpoints
{
    public static void RegisterTodoItemsEndpoints(this WebApplication app)
    {
        var todoItems = app.MapGroup("/todoitems");

        todoItems.MapGet("/", GetTodos);
        todoItems.MapGet("/complete", GetCompleteTodos);
        todoItems.MapGet("/{id}", GetTodoById);
        todoItems.MapPost("/", PostTodoItem)
           .WithValidation<ToDo>();
        // .AddEndpointFilter<MyValidationFilter<ToDo>>();
        todoItems.MapPut("/{id}", PutToDoItem);
        todoItems.MapDelete("/{id}", DeleteToDoItem);
    }

    static async Task<Results<Ok<List<ToDo>>, NotFound>>
        GetTodos(ToDoDb db) => await db.ToDos.ToListAsync()
                is List<ToDo> todos
                    ? TypedResults.Ok(todos)
                    : TypedResults.NotFound();

    static async Task<Results<Ok<List<ToDo>>, NotFound>>
       GetCompleteTodos(ToDoDb db) => await db.ToDos.Where(x => x.IsComplete).ToListAsync()
               is List<ToDo> todos
                   ? TypedResults.Ok(todos)
                   : TypedResults.NotFound();

    static async Task<Results<Ok<ToDo>, NotFound>> GetTodoById(int id, ToDoDb db) =>
    await db.ToDos.FindAsync(id)
        is ToDo todo
            ? TypedResults.Ok(todo)
            : TypedResults.NotFound();

    static async Task<Created<ToDo>> PostTodoItem(ToDo todo, ToDoDb db)
    {
        db.ToDos.Add(todo);
        await db.SaveChangesAsync();
        return TypedResults.Created($"/todoitems/{todo.Id}", todo);
    }

    static async Task<Results<NotFound, NoContent>> PutToDoItem(int id, ToDo inputTodo, ToDoDb db)
    {
        var todo = await db.ToDos.FindAsync(id);

        if (todo is null) return TypedResults.NotFound();

        todo.Name = inputTodo.Name;
        todo.IsComplete = inputTodo.IsComplete;

        await db.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    static async Task<Results<NotFound, NoContent>> DeleteToDoItem(int id, ToDoDb db)
    {
        if (await db.ToDos.FindAsync(id) is ToDo todo)
        {
            db.ToDos.Remove(todo);
            await db.SaveChangesAsync();
            return TypedResults.NoContent();
        }

        return TypedResults.NotFound();
    }
}